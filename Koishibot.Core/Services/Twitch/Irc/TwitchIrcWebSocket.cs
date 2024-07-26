using Koishibot.Core.Services.Websockets;

namespace Koishibot.Core.Services.Twitch.Irc;

public class TwitchIrcWebSocket(string url, CancellationToken cancel, string channelName, byte maxReconnectAttempts)
		: WebSocketHandlerBase(url, cancel, maxReconnectAttempts)
{
	public async Task JoinChannel()
	{
		var ircMessage = $"JOIN ${channelName}";
		await SendMessage(ircMessage);
	}

	public async Task SendMessageToChat(string message)
	{
		var ircMessage = $"PRIVMSG #{channelName} :{message}";
		await SendMessage(ircMessage);
	}
}
