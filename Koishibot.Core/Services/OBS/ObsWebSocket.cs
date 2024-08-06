using Koishibot.Core.Services.OBS.Authentication;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Enums;
using Koishibot.Core.Services.OBS.Scenes;
using Koishibot.Core.Services.Websockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Koishibot.Core.Services.OBS;
public class ObsWebSocket : WebSocketHandlerBase
{
	public ObsWebSocket(
		string url,
		IOptions<Settings> Settings,
		CancellationToken cancel,
		byte maxReconnectAttempts) : base(url, cancel, maxReconnectAttempts)
	{
		_settings = Settings;

		MessageReceived += async (message) => await ProcessMessage(message);
	}

	private IOptions<Settings> _settings;
	private int _keepaliveTimeoutSeconds;
	private Timer _keepaliveTimer;
	private JsonSerializerOptions _options = new()
		{	PropertyNamingPolicy = JsonNamingPolicy.CamelCase	};


	public Action OnConnected { get; set; }	
	public Action<ObsResponse<GetSceneListResponse>>? OnSceneListReceived {get; set;}



	private async Task ProcessMessage(string message)
	{
		if (string.IsNullOrEmpty(message)) { return; }

		var jsonObj = ConvertToJsonObject(message);

		var opCode = GetOpCode(jsonObj);

		if (opCode == OpCodeType.Event)	{	return;	}
		if (opCode == OpCodeType.RequestBatchResponse)	{	return;	}
		
		if (opCode == OpCodeType.RequestResponse)
		{
			var requestType = GetRequestType(jsonObj);

			switch (requestType)
			{
				case ObsRequestStrings.GetSceneList:
					var sceneList = jsonObj.Deserialize<ObsResponse<GetSceneListResponse>>(_options);
					OnSceneListReceived?.Invoke(sceneList);
					break;
				default:
					break;
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
			// Do things when Obs has connected
			return;
		}
	}

	public async Task SendRequest(string type)
	{
		try
		{
			var request = new ObsRequestSimple
			{
				Op = OpCodeType.Request,
				RequestData = new RequestData
				{
					RequestType = type,
					RequestId = Guid.NewGuid()
				}
			};

			var message = JsonSerializer.Serialize(request, _options);
			await SendMessage(message);
		}
		catch (Exception)
		{
			throw;
		}
	}

	//private async Task GetSceneItemList()
	//{
	//	var request = new ObsRequest<GetSceneItemListRequest>
	//	{
	//		Op = OpCodeType.Request,
	//		RequestData = new RequestWrapper<GetSceneItemListRequest>
	//		{
	//			RequestId = Guid.NewGuid(),
	//			RequestType = ObsRequestStrings.GetSceneList,
	//			//RequestData = new GetSceneItemListRequest
	//			//{
	//			//	SceneName = _obsSettings.SceneName!
	//			//}
	//		}
	//	};
	//	var message = JsonSerializer.Serialize(request, _options);
	//	await SendMessage(message);
	//}

	private async Task SendIdentifyRequest(string message)
	{
		var response = JsonSerializer.Deserialize<ObsResponse<HelloResponse>>(message, _options)
				?? throw new Exception("response" + message);

		var authResponse = response.Response.Authentication;

		var ps = _settings.Value.ObsSettings.Password + authResponse.Salt;
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
		await SendMessage(json);
	}

	public JsonObject ConvertToJsonObject(string message) =>
		JsonNode.Parse(message)?.AsObject()
			?? throw new Exception("cry" + message);

	public string GetRequestType(JsonObject jsonObj) =>
		jsonObj["d"]?["requestType"].Deserialize<string>()
			?? throw new Exception("cry" + jsonObj);

	public OpCodeType GetOpCode(JsonObject jsonObj) =>
		jsonObj["op"].Deserialize<OpCodeType>();
}