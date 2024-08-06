using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Scenes;
using System.Text.Json;
namespace Koishibot.Core.Features.Obs;

public record ObsService(
		IAppCache Cache,
		IOptions<Settings> Settings,
		ISignalrService Signalr,
		ILogger<ObsService> Log) : IObsService
{
	public CancellationToken? Cancel { get; set; }
	public ObsWebSocket ObsWebSocket { get; set; }

	private JsonSerializerOptions _options =
			new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


	public async Task CreateWebSocket(CancellationToken cancel)
	{
		ObsWebSocket = new ObsWebSocket(
				$"ws://{Settings.Value.ObsSettings.WebsocketUrl}:{Settings.Value.ObsSettings.Port}",
				Settings, cancel, 3);

		await SetupEventHandlers();
		await ObsWebSocket.Connect();
	}

	public async Task SetupEventHandlers()
	{
		if (ObsWebSocket is null) { return; }

		ObsWebSocket.Connected += async () => await Connected();
		ObsWebSocket.OnConnected += async () => await OnAuthorized();
		ObsWebSocket.Error += async (error) => await Error(error);

		ObsWebSocket.Disconnected += async () => await OnDisconnected();

		ObsWebSocket.OnSceneListReceived += async args => await OnSceneReceived(args);

	}



	public async Task Disconnect()
	{
		if (ObsWebSocket is null) { return; }
		await ObsWebSocket.Disconnect();
	}

	public async Task OnAuthorized()
	{
		await SendRequest(ObsRequestStrings.GetSceneList);
	}

	public async Task Connected()
	{
		await Signalr.SendLog(new LogVm("Obs WebSocket Connected", "Info"));
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, true);
	}

	public async Task Error(string error)
	{
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, false);
		Log.LogInformation(error);
	}

	public async Task OnDisconnected()
	{
		await Signalr.SendLog(new LogVm("Obs WebSocket Disconnected", "Info"));
		await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, false);
	}

	public async Task OnSceneReceived(ObsResponse<GetSceneListResponse> args)
	{
		// Save to Database
		await Signalr.SendLog(new LogVm("Scene", "Info"));
	}



	public async Task SendRequest(string type)
	{
		if (ObsWebSocket is null) { return; }

		await ObsWebSocket.SendRequest(type);
	}

	//public async Task SendRequest<T>(ObsRequest<T> request) where T : class
	//{
	//	try
	//	{
	//		var message = JsonSerializer.Serialize(request, _options);
	//		await ObsWebSocket.SendMessage(message);
	//	}
	//	catch (Exception)
	//	{

	//		throw;
	//	}
	//}
	//public async Task EnableTimer()
	//{
	//	await ObsClient.SetSourceFilterEnabled("BRB Timer", "Start", true);
	//	Log.LogInformation($"Enabled OBS timer");
	//}

	//public async Task ChangeBrowserSource(string sourceName, string url)
	//{
	//	// https://github.com/obs-websocket-community-projects/obs-websocket-js/discussions/319
	//	// Todo: Needs testing
	//	await ObsClient.SetInputSettings("nameofsource", new Dictionary<string, object> { { "url", "theurl" } });
	//	Log.LogInformation($"Obs updated browser source.");
	//}
}


public interface IObsService
{
	Task CreateWebSocket(CancellationToken cancel);
	Task SendRequest(string type);
	Task Disconnect();
}