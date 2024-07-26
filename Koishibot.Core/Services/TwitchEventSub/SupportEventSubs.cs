//using Koishibot.Core.Features.Supports.Interfaces;

//namespace Koishibot.Core.Services.TwitchEventSub;

//public record SupportEventSubs(
//	IOptions<Settings> Settings,
//	IChannelFollowed ChannelFollowed,
//	ICheerReceived CheerReceived
//	//IPowerUpReceived PowerUpReceived
//	) : ISupportEventSubs
//{
//	// == ⚫ == //

//	public async Task SetupAndSubscribe()
//	{
//		if (Settings.Value.DebugMode)
//		{
//			//await PowerUpReceived.SetupMethod();
//			//return;
//		}

//		await ChannelFollowed.SetupMethod();
//		await CheerReceived.SetupMethod();


//	}

//	// == ⚫ == //

//	public async Task SubscribeToEvents()
//	{
//		if (Settings.Value.DebugMode)
//		{
//			//await PowerUpReceived.SubToEvent();
//			//return;
//		}

//		await ChannelFollowed.SubToEvent();
//		await CheerReceived.SubToEvent();
//	}
//}
