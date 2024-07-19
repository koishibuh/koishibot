namespace Koishibot.Core.Services.TwitchEventSub.Interfaces;

public interface IPollEventSubs
{
	Task SetupAndSubscribe();
	Task SubscribeToEvents();
}
