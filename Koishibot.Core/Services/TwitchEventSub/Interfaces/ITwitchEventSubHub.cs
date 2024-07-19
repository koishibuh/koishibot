namespace Koishibot.Core.Services.TwitchEventSub.Interfaces;

public interface ITwitchEventSubHub
{
	Task Start();
	Task Disconnect();
	Task SubscribeToEvents();
	Task ResubscribeToEvents();
}