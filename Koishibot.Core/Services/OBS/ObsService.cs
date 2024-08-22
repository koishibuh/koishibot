﻿using Koishibot.Core.Features.Obs.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.OBS.Authentication;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Enums;
using Koishibot.Core.Services.OBS.Inputs;
using Koishibot.Core.Services.OBS.Scenes;
using Koishibot.Core.Services.Websockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Koishibot.Core.Exceptions;
namespace Koishibot.Core.Features.Obs;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record ObsService(
IAppCache Cache,
IOptions<Settings> Settings,
ISignalrService Signalr,
ILogger<ObsService> Log,
IServiceScopeFactory ScopeFactory
) : IObsService
{
	public CancellationToken? Cancel { get; set; }
	private WebSocketFactory Factory { get; set; } = new();
	private WebSocketHandler? ObsWebSocket { get; set; }

	private readonly JsonSerializerOptions _options =
	new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

	public async Task CreateWebSocket()
	{
		// if (ObsWebSocket is not null) return;

		var url = $"ws://{Settings.Value.ObsSettings.WebsocketUrl}:{Settings.Value.ObsSettings.Port}";

		try
		{
			ObsWebSocket = await Factory.Create(url, 3, ProcessMessage, Error, Closed);
		}
		catch (Exception e)
		{
			Log.LogError("An error has occured: {e}", e);
			await Signalr.SendError(e.Message);
			throw new CustomException(e.Message);
		}
	}

	private async Task ProcessMessage(WebSocketMessage message)
	{
		try
		{
			if (message.IsNullOrEmpty())
				throw new NullException("ObsWS received a null message");

			var jsonObj = message.ConvertToJsonObject();
			var opCode = GetOpCode(jsonObj);

			// TODO
			if (opCode == OpCodeType.Event) return;
			if (opCode == OpCodeType.RequestBatchResponse) return;

			if (opCode == OpCodeType.RequestResponse)
			{
				var requestType = GetRequestType(jsonObj);

				switch (requestType)
				{
					case ObsRequests.GetCurrentProgramScene:
						var currentScene = jsonObj["d"].Deserialize<RequestResponse<GetCurrentProgramSceneResponse>>(_options);
						break;
					case ObsRequests.GetSceneList:
						var sceneList = jsonObj["d"].Deserialize<RequestResponse<GetSceneListResponse>>(_options);
						await OnSceneReceived(sceneList.ResponseData);
						break;
					case ObsRequests.GetInputList:
						var inputList = jsonObj["d"].Deserialize<RequestResponse<GetInputListResponse>>(_options);
						break;
					case ObsRequests.GetInputKindList:
						//var inputKindList = jsonObj.Deserialize<ObsResponse<GetInputKindListResponse>>(_options);
						break;
					default:
						break;
					// When Scene/Source is updated/removed/added
				}

				return;
			}

			if (opCode is OpCodeType.Hello or OpCodeType.Reidentify)
			{
				await SendIdentifyRequest(message);
				return;
			}

			if (opCode == OpCodeType.Identified)
			{
				await OnAuthorized();
				return;
			}
		}
		catch (Exception e)
		{
			Log.LogError("An error has occured: {e}", e);
			await Signalr.SendError(e.Message);
		}
	}

	private async Task OnAuthorized()
	{
		await Signalr.SendInfo("Obs WebSocket Connected");
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, ServiceStatusString.Online);

		// Do things when Obs has connected
		var request = new RequestWrapper
		{
			RequestType = ObsRequests.GetSceneList
		};

		await SendRequest(new ObsRequest { Data = request });
	}

	public void SetCancellationToken(CancellationToken cancel)
		=> Cancel = cancel;

	private async Task OnSceneReceived(GetSceneListResponse args)
	{
		var scenes = args.Scenes.Select(x => new ObsItem
		{
		ObsId = x.SceneUuid ?? string.Empty,
		ObsName = x.SceneName ?? string.Empty,
		Type = ObsItemType.Scene.ToString()
		}).ToList();

		//var database = CreateScopedDatabase();
		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		foreach (var scene in scenes)
		{
			var result = await database.ObsItems.FirstOrDefaultAsync(x => x.ObsId == scene.ObsId);
			if (result is null)
			{
				database.Add(scene);
			}
			else if (result.ObsName != scene.ObsName)
			{
				result.ObsName = scene.ObsName;
				database.Update(result);
			}
		}

		await database.SaveChangesAsync();


		await Signalr.SendInfo(scenes[0].ObsName);
	}

	public async Task OnInputReceived(GetInputListResponse args)
	{
		var audio = args.Inputs
		.Where(x => x.UnversionedInputKind == InputTypes.AudioOutputCapture ||
		            x.UnversionedInputKind == InputTypes.AudioInputCapture)
		.Select(x => new ObsItem
		{
		ObsId = x.InputUuid ?? string.Empty,
		ObsName = x.InputName ?? string.Empty,
		Type = ObsItemType.Audio.ToString()
		});

		using var scope = ScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();

		foreach (var item in audio)
		{
			var result = await database.ObsItems.Where(x => x.ObsId == item.ObsId).FirstOrDefaultAsync();
			if (result is null)
			{
				database.Add(item);
			}
			else if (result.ObsName != item.ObsName)
			{
				result.ObsName = item.ObsName;
				database.Update(result);
			}
		}

		await database.SaveChangesAsync();
	}


	public async Task SendRequest<T>(ObsRequest<T> request)
	{
		if (ObsWebSocket is null)
		{
			return;
		}

		try
		{
			var message = JsonSerializer.Serialize(request, _options);
			await ObsWebSocket.SendMessage(message);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task SendRequest(ObsRequest request)
	{
		if (ObsWebSocket is null)
		{
			return;
		}

		try
		{
			var message = JsonSerializer.Serialize(request, _options);
			await ObsWebSocket.SendMessage(message);
		}
		catch (Exception)
		{
			throw;
		}
	}


	//public async Task ChangeBrowserSource(string sourceName, string url)
	//{
	//	// https://github.com/obs-websocket-community-projects/obs-websocket-js/discussions/319
	//	// Todo: Needs testing
	//	await ObsClient.SetInputSettings("nameofsource", new Dictionary<string, object> { { "url", "theurl" } });
	//	Log.LogInformation($"Obs updated browser source.");
	//}

	private async Task SendIdentifyRequest(WebSocketMessage message)
	{
		if (ObsWebSocket is null)
		{
			return;
		}

		var response = JsonSerializer.Deserialize<ObsBase<HelloResponse>>(message.Message, _options)
		               ?? throw new Exception("response" + message);

		var authResponse = response.Response.Authentication;

		var ps = Settings.Value.ObsSettings.Password + authResponse.Salt;
		var binaryHash = SHA256.HashData(Encoding.UTF8.GetBytes(ps));
		var b64Hash = Convert.ToBase64String(binaryHash);
		var challengeHash = SHA256.HashData(Encoding.UTF8.GetBytes(b64Hash + authResponse.Challenge));
		var b64Challenge = Convert.ToBase64String(challengeHash);

		var req = new IdentifyRequest
		{
		Op = OpCodeType.Identify,
		Data = new IdentifyData
		{
		RpcVersion = 1,
		Authentication = b64Challenge
		}
		};

		var json = JsonSerializer.Serialize(req, _options);
		await ObsWebSocket.SendMessage(json);
	}

	private async Task Error(WebSocketMessage message)
	{
		Log.LogError("Websocket error: {message}", message);
		await Signalr.SendError(message.Message);
		if (ObsWebSocket is not null && ObsWebSocket.IsDisposed is false)
		{
			await Disconnect();
		}
	}

	private async Task Closed(WebSocketMessage message)
	{
		Log.LogInformation($"Websocket closed {message}");
		if (ObsWebSocket is not null && ObsWebSocket.IsDisposed is false)
		{
			await Disconnect();
		}
	}

	public async Task Disconnect()
	{
		await Factory.Disconnect();
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, ServiceStatusString.Offline);
		await Signalr.SendInfo("OBS Websocket Disconnected");
	}

	private static string GetRequestType(JsonObject jsonObj) =>
	jsonObj["d"]?["requestType"].Deserialize<string>()
	?? throw new Exception("cry" + jsonObj);

	private static OpCodeType GetOpCode(JsonObject jsonObj) =>
	jsonObj["op"].Deserialize<OpCodeType>();
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IObsService
{
	void SetCancellationToken(CancellationToken cancel);
	Task CreateWebSocket();
	Task SendRequest<T>(ObsRequest<T> request);
	Task SendRequest(ObsRequest request);
	Task Disconnect();
}