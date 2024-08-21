using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Websockets;
namespace Koishibot.Core.Services.Twitch.Irc;

/*═══════════════════【 SERVICE 】═══════════════════*/
/// <summary>
/// <see href="https://dev.twitch.tv/docs/chat/irc/">Twitch Documentation</see>
/// </summary>
public record TwitchIrcService(
IOptions<Settings> Settings,
IAppCache Cache,
ISignalrService SignalrService,
ILogger<TwitchIrcService> Log
) : ITwitchIrcService
{
	public CancellationToken? Cancel { get; set; }
	private WebSocketHandler? BotIrc { get; set; }

	public async Task CreateWebSocket()
	{
		if (BotIrc is not null) { return; }

		const string url = "wss://irc-ws.chat.twitch.tv:443";

		var factory = new WebSocketFactory();
		BotIrc = await factory.Create(url, 3, Error, ProcessMessage);

		var streamer = Settings.Value.StreamerTokens;
		var bot = Settings.Value.BotTokens;

		await OnConnected(bot.AccessToken, bot.UserLogin);
	}

	private async Task Error(WebSocketMessage message)
	{
		await DisconnectWebSocket();
	}

	private async Task ProcessMessage(WebSocketMessage message)
	{
		if (message.IsPing())
		{
			await BotIrc!.SendMessage("PONG :tmi.twitch.tv");
		}
	}

	private async Task OnConnected(string accessToken, string username)
	{
		var streamer = Settings.Value.StreamerTokens.UserLogin;

		await SignalrService.SendInfo("Connecting to Twitch Chat");
		await SignalrService.SendInfo("Connection to Twitch Chat successful");

		await BotIrc!.SendMessage($"CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands");
		await BotIrc.SendMessage($"PASS oauth:{accessToken}");
		await BotIrc.SendMessage($"NICK {username}");

		await Task.Delay(3000);

		await BotIrc.SendMessage($"JOIN ${streamer}");
		// await SendMessageToChat(streamer, "Joined channel");

		await Task.Delay(3000);

		await Cache.UpdateServiceStatus(ServiceName.BotIrc, ServiceStatusString.Online);
	}

	private async Task SendMessageToChat(string channelName, string message)
	{
		var ircMessage = $"PRIVMSG #{channelName} :{message}";
		await BotIrc!.SendMessage(ircMessage);
	}


	public async Task BotSend(string message)
	{
		try
		{
			var streamer = Settings.Value.StreamerTokens.UserLogin;
			await SendMessageToChat(streamer, message);
		}
		catch (Exception ex)
		{
			Log.LogError(ex.Message);
		}
	}

	public void SetCancellationToken(CancellationToken cancel)
		=> Cancel = cancel;

	public async Task DisconnectWebSocket()
	{
		await BotIrc!.Disconnect();
		await Cache.UpdateServiceStatus(ServiceName.BotIrc, ServiceStatusString.Offline);
		BotIrc = null;
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface ITwitchIrcService
{
	void SetCancellationToken(CancellationToken cancel);
	Task CreateWebSocket();
	Task BotSend(string message);
	Task DisconnectWebSocket();
}