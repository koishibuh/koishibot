namespace Koishibot.Core.Services.TwitchEventSub.Interfaces;
public interface ISupportEventSubs
{
	Task SetupAndSubscribe();
	Task SubscribeToEvents();
}
