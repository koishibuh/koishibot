using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.Websockets;
namespace Koishibot.Core.Services.Twitch.Irc;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/chat/irc/">Twitch Documentation</see>
/// </summary>
public record TwitchIrcService(
	IOptions<Settings> Settings,
	IAppCache Cache,
	IServiceScopeFactory ScopeFactory,
	ISignalrService SignalrService
	) : ITwitchIrcService
{
	public WebSocketClient BotIrc { get; set; }

	public async Task CreateWebSocket()
	{
		var factory = new WebSocketFactory();

		BotIrc = await factory.Create(
			"wss://irc-ws.chat.twitch.tv:443",
			3, Error, ProcessMessage);

		var streamer = Settings.Value.StreamerTokens;
		var bot = Settings.Value.BotTokens;

		await OnConnected(bot.AccessToken, bot.UserLogin);
	}

	public async Task Error(WebSocketMessage message)
	{
		//factory.Remove(WebSocketName.TwitchIrc);
		await BotIrc.Disconnect();
	}

	public async Task ProcessMessage(WebSocketMessage message)
	{
		if (message.IsPing())
		{
			await BotIrc.SendMessage("PONG :tmi.twitch.tv");
		}
	}

	public async Task OnConnected(string accessToken, string username)
	{
		var streamer = Settings.Value.StreamerTokens.UserLogin;

		await SignalrService.SendInfo("Connecting to Twitch Chat");
		await SignalrService.SendInfo("Connection to Twitch Chat sucessful");

		await BotIrc.SendMessage($"CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands");
		await BotIrc.SendMessage($"PASS oauth:{accessToken}");
		await BotIrc.SendMessage($"NICK {username}");

		await Task.Delay(3000);

		await BotIrc.SendMessage($"JOIN ${streamer}");
		await SendMessageToChat(streamer, "Joined channel");

		await Cache.UpdateServiceStatus(ServiceName.BotIrc, true);
	}

	public async Task SendMessageToChat(string channelName, string message)
	{
		var ircMessage = $"PRIVMSG #{channelName} :{message}";
		await BotIrc.SendMessage(ircMessage);
	}


	public async Task BotSend(string message)
	{
		if (BotIrc is not null)
		{
			await SendMessageToChat("elysiargiffin", message);
		}
	}
}