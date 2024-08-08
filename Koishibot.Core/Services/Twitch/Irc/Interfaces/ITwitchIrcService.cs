namespace Koishibot.Core.Services.Twitch.Irc.Interfaces;

public interface ITwitchIrcService
{
	Task CreateWebSocket();
	Task BotSend(string message);
}