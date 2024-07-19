using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

namespace Koishibot.Core.Services.TwitchEventSub.Interfaces;

public interface IChannelPointEventSubs
{
	Task SetupAndSubscribe();
	Task SubscribeToEvents();
}

