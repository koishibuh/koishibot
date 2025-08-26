using Koishibot.Core.Exceptions;
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
	private WebSocketFactory Factory { get; set; } = new();
	private WebSocketHandler? BotIrc { get; set; }

	public async Task CreateWebSocket()
	{
		const string url = "wss://irc-ws.chat.twitch.tv:443";

		try
		{
			BotIrc = await Factory.Create(url, 3, ProcessMessage, Error, Closed);
			var bot = Settings.Value.BotTokens;
			await OnConnected(bot.AccessToken, bot.UserLogin);
		}
		catch (Exception e)
		{
			Log.LogError("An error has occured: {e}", e);
			await SignalrService.SendError(e.Message);
			throw new CustomException(e.Message);
		}

	}

	private async Task ProcessMessage(WebSocketMessage message)
	{
		if (message.IsPing())
		{
			await BotIrc!.SendMessage("PONG :tmi.twitch.tv");
		}
		if (message.IsUnauthorized())
		{
			await SignalrService.SendError(message.Message);
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
		if (BotIrc.IsDisposed is true)
		{
			// TODO: Check if login info is correct
			return;
		}

		await BotIrc.SendMessage($"JOIN ${streamer}");
		// await SendMessageToChat(streamer, "Joined channel");

		await Task.Delay(3000);

		await Cache.UpdateServiceStatus(ServiceName.BotIrc, Status.Online);
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


	private async Task Error(WebSocketMessage message)
	{
		Log.LogError("Websocket error: {message}", message);
		await SignalrService.SendError(message.Message);
		if (BotIrc?.IsDisposed is false)
		{
			await DisconnectWebSocket();
		}
	}

	private async Task Closed(WebSocketMessage message)
	{
		Log.LogInformation($"TwitchIrc Websocket closed {message.Message}");
		if (BotIrc is not null  && BotIrc.IsDisposed is false)
		{
			await DisconnectWebSocket();
		}
	}

	public async Task DisconnectWebSocket()
	{
		await Factory.Disconnect();
		await Cache.UpdateServiceStatus(ServiceName.BotIrc, Status.Offline);
		await SignalrService.SendInfo("BotIrc Websocket Disconnected");
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