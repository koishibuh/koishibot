using Koishibot.Core.Features.Obs.Models;
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
namespace Koishibot.Core.Features.Obs;

public record ObsService(
		IAppCache Cache,
		IOptions<Settings> Settings,
		ISignalrService Signalr,
		ILogger<ObsService> Log,
		IServiceScopeFactory ScopeFactory
	) : IObsService
{
	public WebSocketClient? ObsWebSocket { get; set; }

	private JsonSerializerOptions _options =
			new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

	public async Task CreateWebSocket(CancellationToken cancel)
	{
		if (ObsWebSocket is not null) { return; }
		
		var factory = new WebSocketFactory();

		ObsWebSocket = await factory.Create(
			$"ws://{Settings.Value.ObsSettings.WebsocketUrl}:{Settings.Value.ObsSettings.Port}",
			3, Error, ProcessMessage);
	}

	public async Task ProcessMessage(WebSocketMessage message)
	{
		if (message.IsNullOrEmpty()) { return; }

		var jsonObj = message.ConvertToJsonObject();

		var opCode = GetOpCode(jsonObj);

		if (opCode == OpCodeType.Event) { return; }
		if (opCode == OpCodeType.RequestBatchResponse) { return; }

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

		if (opCode == OpCodeType.Hello || opCode == OpCodeType.Reidentify)
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


	public async Task Disconnect()
	{
		await ObsWebSocket.Disconnect();
	}

	public async Task OnAuthorized()
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


	public async Task Error(WebSocketMessage error)
	{
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, ServiceStatusString.Offline);
		await Signalr.SendError(error.Message);
		Log.LogInformation(error.Message);
		if (ObsWebSocket.IsDisposed is false)
		{
			await ObsWebSocket.Disconnect();
		}
	}

	public async Task OnDisconnected()
	{
		await Signalr.SendInfo("Obs WebSocket Disconnected");
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, ServiceStatusString.Offline);
	}

	public async Task OnSceneReceived(GetSceneListResponse args)
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
			var result = await database.ObsItems.Where(x => x.ObsId == scene.ObsId).FirstOrDefaultAsync();
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

	public KoishibotDbContext CreateScopedDatabase()
	{
		using var scope = ScopeFactory.CreateScope();
		return scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
	}


	public string GetRequestType(JsonObject jsonObj) =>
		jsonObj["d"]?["requestType"].Deserialize<string>()
			?? throw new Exception("cry" + jsonObj);

	public OpCodeType GetOpCode(JsonObject jsonObj) =>
		jsonObj["op"].Deserialize<OpCodeType>();
}


public interface IObsService
{
	Task CreateWebSocket(CancellationToken cancel);
	Task SendRequest<T>(ObsRequest<T> request);
	Task SendRequest(ObsRequest request);
	Task Disconnect();
}