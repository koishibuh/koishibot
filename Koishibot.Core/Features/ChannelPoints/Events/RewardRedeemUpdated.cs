using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

namespace Koishibot.Core.Features.ChannelPoints.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionupdate">Twitch Documentation</see></para>
/// </summary>
public record RewardRedeemUpdatedHandler(
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database,
	ISignalrService Signalr
	) : IRequestHandler<RedeemedRewardUpdatedCommand>
{
	public async Task Handle
		(RedeemedRewardUpdatedCommand command, CancellationToken cancel)
	{
		// TODO: Save to DB

		await Signalr.SendInfo($"{command.args.Reward.Title} has been updated");

		await Task.CompletedTask;
	}
}

// == ⚫ COMMAND == //

public record RedeemedRewardUpdatedCommand
	(RewardRedemptionUpdatedEvent args) : IRequest;