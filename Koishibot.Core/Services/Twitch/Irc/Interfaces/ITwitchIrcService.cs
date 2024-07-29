namespace Koishibot.Core.Services.Twitch.Irc.Interfaces;

public interface ITwitchIrcService
{
	Task CreateWebSocket(CancellationToken cancel);
	Task BotSend(string message);
}