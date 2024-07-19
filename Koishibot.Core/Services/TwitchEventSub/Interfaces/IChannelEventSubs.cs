namespace Koishibot.Core.Services.TwitchEventSub.Interfaces;

public interface IChannelEventSubs
{
	Task SetupAndSubscribe();
	Task SubscribeToEvents();
}

