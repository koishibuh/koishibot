using Koishibot.Core.Persistence.Cache.Enums;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
namespace Koishibot.Core.Services.TwitchEventSub;

public record TwitchEventSubHub(
	IOptions<Settings> Settings,
	ILogger<TwitchEventSubHub> Log, IRefreshAccessTokenService TokenProcessor,
	EventSubWebsocketClient EventSubClient, IAppCache Cache,
	IChannelEventSubs Channel,
	IChannelPointEventSubs ChannelPoints,
	IRaidEventSubs Raids,
	IPollEventSubs Polls,
	ISupportEventSubs Support
	) : ITwitchEventSubHub
{
	public async Task Start()
	{
		EventSubClient.WebsocketConnected += OnWebsocketConnected;
		EventSubClient.WebsocketDisconnected += OnWebsocketDisconnected;
		EventSubClient.WebsocketReconnected += OnWebsocketReconnected;
		EventSubClient.ErrorOccurred += OnErrorOccurred;

		await EventSubClient.ConnectAsync();
	}

	public async Task Disconnect()
	{
		await EventSubClient.DisconnectAsync();
	}

	// == ⚫ == //

	public async Task SubscribeToEvents()
	{
		await Channel.SetupAndSubscribe();
		await ChannelPoints.SetupAndSubscribe();
		await Raids.SetupAndSubscribe();
		await Polls.SetupAndSubscribe();
		await Support.SetupAndSubscribe();

		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, true);
		Log.LogInformation($"TwitchEventSubHub Connected");
	}

	// == ⚫  == //

	public async Task ResubscribeToEvents()
	{
		await Channel.SubscribeToEvents();
		await ChannelPoints.SubscribeToEvents();
		await Raids.SubscribeToEvents();
		await Polls.SubscribeToEvents();
		await Support.SubscribeToEvents();

		//await Task.WhenAll(
		//	Channel.SubscribeToEvents(),
		//	Polls.SubscribeToEvents(),
		////ChannelGoals.SubscribeToEvents(),
		////ChannelPoints.SubscribeToEvents(),
		////HypeTrain.SubscribeToEvents(),
		////Moderation.SubscribeToEvents(),
		////Polls.SubscribeToEvents(),
		////Predictions.SubscribeToEvents(),
		//Raids.SubscribeToEvents()
		////Shoutouts.SubscribeToEvents(),
		////Support.SubscribeToEvents()
		//);

		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, true);
		Log.LogInformation($"TwitchEventSubHub Reconnected");
	}

	// == ⚫ EVENT HANDLERS == //

	private async Task OnWebsocketConnected(object sender, WebsocketConnectedArgs e)
	{
		Settings.Value.TwitchEventSubSessionId = EventSubClient.SessionId;

		try
		{
			if (!e.IsRequestedReconnect)
			{
				await SubscribeToEvents();
			}
			else
			{
				await ResubscribeToEvents();
			}
		}
		catch (Exception ex)
		{
			Log.LogError($"{ex}");
		}

	}

	private async Task OnErrorOccurred(object sender, ErrorOccuredArgs args)
	{
		Log.LogError($"TwitchEventSubHub ErrorOccured {args}");
		await Task.CompletedTask;
	}

	private async Task OnWebsocketReconnected(object sender, EventArgs args)
	{
		Log.LogInformation($"TwitchEventSubHub Reconnected");
		await Task.CompletedTask;
	}

	private async Task OnWebsocketDisconnected(object sender, EventArgs args)
	{
		await Cache.UpdateServiceStatus(ServiceName.TwitchWebsocket, false);
		Log.LogError($"TwitchEventSubHub Disconnected");

		await TokenProcessor.Start();

		await EventSubClient.ReconnectAsync();
	}
}