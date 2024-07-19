namespace Koishibot.Core.Services.TwitchIrcClient.Interfaces;

public interface IChatMessageService
{
    Task BotSend(string message);
    Task StreamerSend(string message);
}