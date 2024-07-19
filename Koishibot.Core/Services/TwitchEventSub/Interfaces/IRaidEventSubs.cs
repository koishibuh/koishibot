namespace Koishibot.Core.Services.TwitchEventSub.Interfaces;

public interface IRaidEventSubs
{
	Task SetupAndSubscribe();
	Task SubscribeToEvents();
}