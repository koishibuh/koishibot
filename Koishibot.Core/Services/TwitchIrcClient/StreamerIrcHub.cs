//using Koishibot.Core.Configurations;
//using Koishibot.Core.Features.ChatMessages.Interfaces;
//using Koishibot.Core.Persistence.Cache.Enums;
//using System.Net.WebSockets;
//using TwitchLib.Client.Events;
//using TwitchLib.Client.Models;
//namespace Koishibot.Core.Services.TwitchIrcClient;


//public record StreamerIrcHub(IOptions<Settings> Settings,
//		IAppCache Cache, StreamerTwitchClient Client,
//		ILogger<StreamerIrcHub> Log,
//		IChatMessageReceived ChatMessageReceived,
//		IChatMessageSent ChatMessageSent
//		) : IStreamerIrcHub
//{
//	public TwitchTokens Streamer => Settings.Value.StreamerTokens;

//	public async Task Start()
//	{
//		InitializeStreamerClient();
//		SetupEventHandlers();
//		await Client.ConnectAsync();
//	}

//	public void InitializeStreamerClient()
//	{
//		var credentials = new ConnectionCredentials(Streamer.Username, Streamer.AccessToken);
//		Client.Initialize(credentials, Streamer.Username);
//	}

//	public void SetupEventHandlers()
//	{
//		Client.OnConnected += OnConnected;
//		Client.OnDisconnected += OnDisconnected;
//		Client.OnReconnected += OnReconnected;
//		Client.OnConnectionError += OnConnectionError;
//		Client.OnJoinedChannel += OnJoinedChannel;

//		ChatMessageReceived.SetupHandler();
//		ChatMessageSent.SetupHandler();
//	}

//	// == ⚫ EVENT HANDLERS == //

//	public async Task OnConnected(object? sender, OnConnectedEventArgs e)
//	{
//		try
//		{ Log.LogInformation("StreamerIrc Connected"); }
//		catch (Exception ex)
//		{ Log.LogError(ex, $"There was an error connecting to StreamerIrc"); }
//		await Task.CompletedTask;
//	}

//	public async Task OnDisconnected(object? sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
//	{
//		Log.LogError("StreamerIrc disconnected, attempting to reconnect");
//		await Cache.UpdateServiceStatus(ServiceName.StreamerIrc, false);

//		var retryCount = 0;
//		while (retryCount < 3)
//		{
//			try
//			{
//				await Client.ConnectAsync();
//				retryCount =  0;
//			}
//			catch (WebSocketException)
//			{
//				var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
//				await Task.Delay(delay);
//				retryCount++;
//			}
//		}
//		if (retryCount >= 3)
//		{
//			throw new WebSocketException($"Failed to connect websocket");
//		}
//	}

//	public async Task OnReconnected(object? sender, OnConnectedEventArgs e)
//	{
//		try
//		{
//			Log.LogInformation("StreamerIrc Reconnected");
//			await Cache.UpdateServiceStatus(ServiceName.StreamerIrc, true);
//		}
//		catch (Exception ex)
//		{
//			Log.LogError(ex, $"There was an error reconnecting to StreamerIrc");
//		}

//	}

//	public async Task OnConnectionError(object? sender, OnConnectionErrorArgs e)
//	{
//		try
//		{
//			Log.LogError("StreamerIrc Connection Error");
//			await Task.CompletedTask;
//		}
//		catch (Exception ex)
//		{
//			Log.LogError(ex, $"There was an connection error StreamerIrc");
//		}
//	}

//	//

//	public async Task OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
//	{
//		try
//		{
//			await Cache.UpdateServiceStatus(ServiceName.StreamerIrc, true);
//			Log.LogInformation("StreamerIrc joined {e.Channel}", e);
//			await Task.CompletedTask;
//		}
//		catch (Exception ex)
//		{
//			Log.LogError(ex, $"There was an error while joining channel with StreamerIrc");
//		}

//	}
//}

//////Client.OnNewSubscriber += Client_OnNewSubscriber;
//////Client.OnReSubscriber += Client_OnReSubscriber;
//////Client.OnPrimePaidSubscriber += Client_OnPrimePaidSubscriber;
//////Client.OnGiftedSubscription += Client_OnGiftedSubscription;
//////Client.OnContinuedGiftedSubscription += Client_OnContinuedGiftedSubscription;
//////Client.OnCommunitySubscription += Client_OnCommunitySubscription;
//////Client.OnCommunityPayForward += Client_OnCommunityPayForward;
//////Client.OnStandardPayForward += Client_OnStandardPayForward;
//////Client.OnAnonGiftPaidUpgrade += Client_OnAnonGiftPaidUpgrade;
//////Client.OnBitsBadgeTier += Client_OnBitsBadgeTier;

//////Client.OnUserIntro += Client_OnUserIntro;
//////Client.OnUserTimedout += Client_OnUserTimedout;
//////Client.OnUserBanned += Client_OnUserBanned;
