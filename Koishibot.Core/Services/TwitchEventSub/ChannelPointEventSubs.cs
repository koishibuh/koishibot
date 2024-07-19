using Koishibot.Core.Features.ChannelPoints.Interfaces;
namespace Koishibot.Core.Services.TwitchEventSub;

public record ChannelPointEventSubs(
	IOptions<Settings> Settings,
	IPointRewardCreated PointRewardCreated,
	IPointRewardUpdated PointRewardUpdated,
	IPointRewardDeleted PointRewardDeleted,
	IRewardRedeemed RewardRedeemed,
	IRewardRedeemUpdated RedeemedRewardUpdated
	) : IChannelPointEventSubs
{
	// == ⚫ == //

	public async Task SetupAndSubscribe()
	{
		if (Settings.Value.DebugMode)
		{
			return;
		}

		await PointRewardCreated.SetupHandler();
		await PointRewardUpdated.SetupHandler();
		await PointRewardDeleted.SetupHandler();

		await RewardRedeemed.SetupHandler();
		await RedeemedRewardUpdated.SetupHandler();

	}

	// == ⚫ == //

	public async Task SubscribeToEvents()
	{
		if (Settings.Value.DebugMode)
		{
			return;
		}

		await PointRewardCreated.SubToEvent();
		await PointRewardUpdated.SubToEvent();
		await PointRewardDeleted.SubToEvent();
		await RewardRedeemed.SubToEvent();
		await RedeemedRewardUpdated.SubToEvent();

		// Not added to TwitchLib yet
		//Subscriber.CreateEventSubBroadcaster("channel.channel_points_automatic_reward_redemption.add", "1")
	}
}