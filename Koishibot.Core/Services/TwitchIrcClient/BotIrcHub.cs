using Koishibot.Core.Configurations;
using Koishibot.Core.Persistence.Cache.Enums;
using System.Net.WebSockets;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
namespace Koishibot.Core.Services.TwitchIrcClient;

public record BotIrcHub(
	IOptions<Settings> Settings, BotTwitchClient Client,
	IAppCache Cache, ILogger<BotIrcHub> Log
	) : IBotIrcHub
{
	public TwitchTokens Bot => Settings.Value.BotTokens;
	public TwitchTokens Streamer => Settings.Value.StreamerTokens;

	public async Task Start()
	{
		InitializeStreamerClient();
		SetupEventHandlers();
		await Client.ConnectAsync();
	}

	public void InitializeStreamerClient()
	{
		var credentials = new ConnectionCredentials(Bot.Username, Bot.AccessToken);
		Client.Initialize(credentials, Streamer.Username);
	}

	public void SetupEventHandlers()
	{
		Client.OnConnected += OnConnected;
		Client.OnDisconnected += OnDisconnected;
		Client.OnReconnected += OnReconnected;
		Client.OnConnectionError += OnConnectionError;

		Client.OnJoinedChannel += OnJoinedChannel;
		Client.OnWhisperReceived += OnWhisperReceived;
	}

	// == ⚫ EVENT HANDLERS == //

	public async Task OnConnected(object? sender, OnConnectedEventArgs e)
	{
		Log.LogInformation("BotIrc Connected");
		await Task.CompletedTask;
	}

	public async Task OnDisconnected(object? sender,
		TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
	{
		Log.LogInformation("BotIrc disconnected, attempting to reconnect");
		await Cache.UpdateServiceStatus(ServiceName.BotIrc, false);

		var retryCount = 0;
		while (retryCount < 3)
		{
			try
			{
				await Client.ConnectAsync();
				retryCount = 0;
			}
			catch (WebSocketException)
			{
				var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
				await Task.Delay(delay);
				retryCount++;
			}
		}
		if (retryCount >= 3)
		{
			throw new WebSocketException($"Failed to connect websocket");
		}
	}

	public async Task OnReconnected(object? sender, OnConnectedEventArgs e)
	{
		Log.LogInformation("BotIrc Reconnected");
		await Cache.UpdateServiceStatus(ServiceName.BotIrc, true);
		await Task.CompletedTask;
	}

	public async Task OnConnectionError(object? sender, OnConnectionErrorArgs e)
	{
		// TODO: Update icon on UI
		Log.LogError("BotIrc Connection Error");
		await Task.CompletedTask;
	}


	public async Task OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
	{
		Log.LogInformation($"BotIrc joined {e.Channel}");
		//await Client.SendMessageAsync("elysiagriffin", "Connected");
		await Cache.UpdateServiceStatus(ServiceName.BotIrc, true);
	}

	//public async Task OnMessageReceived(object? sender, OnMessageReceivedArgs e)
	//{
	//	Log.LogInformation($"BotIrc {e.ChatMessage.Message}");
	//	await Task.CompletedTask;
	//}

	//public async Task OnMessageSent(object? sender, OnMessageSentArgs e)
	//{
	//	Log.LogInformation($"BotIrc {e.SentMessage}");
	//	await Task.CompletedTask;
	//}

	public async Task OnWhisperReceived(object? sender, OnWhisperReceivedArgs e)
	{
		Log.LogInformation($"BotIrc Whisper {e.WhisperMessage.Message}");
		await Task.CompletedTask;
		// When user whispers the bot
	}
}


