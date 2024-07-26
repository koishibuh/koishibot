//using Koishibot.Core.Features.AdBreak.Interfaces;
//using Koishibot.Core.Features.StreamInformation.Interfaces;
//namespace Koishibot.Core.Services.TwitchEventSub;

//public record ChannelEventSubs(
//	IOptions<Settings> Settings,
//	IStreamOnline StreamOnline,
//	IAdBreakStarted AdBreakStarted,
//	IStreamUpdated StreamInfoUpdated,
//	IStreamOffline StreamOffline
//	) : IChannelEventSubs
//{
//	// == ⚫ == //

//	public async Task SetupAndSubscribe()
//	{
//		if (Settings.Value.DebugMode)
//		{
//			return;
//		}

//		await StreamOnline.SetupMethod();
//		await AdBreakStarted.SetupHandler();
//		await StreamInfoUpdated.SetupHandler();
//		await StreamOffline.SetupMethod();
//	}

//	// == ⚫ == //

//	public async Task SubscribeToEvents()
//	{
//		if (Settings.Value.DebugMode)
//		{
//			return;
//		}

//		await StreamOnline.SubToEvent();
//		await AdBreakStarted.SubToEvent();
//		await StreamInfoUpdated.SubToEvent();
//		await StreamOffline.SubToEvent();
//	}
//}
