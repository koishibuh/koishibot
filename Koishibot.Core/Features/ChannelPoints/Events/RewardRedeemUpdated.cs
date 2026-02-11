using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

namespace Koishibot.Core.Features.ChannelPoints.Events;

// /*═══════════════════【 HANDLER 】═══════════════════*/
// /// <summary>
// /// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionupdate">Twitch Documentation</see></para>
// /// </summary>
// public record RewardRedeemUpdatedHandler(
// 	ITwitchUserHub TwitchUserHub,
// 	KoishibotDbContext Database,
// 	ISignalrService Signalr
// 	) : IRewardRedeemUpdateHandler
// {
// 	public async Task Handle
// 		(RewardRedemptionUpdatedEvent e)
// 	{
// 		// TODO: Save to DB
//
// 		await Signalr.SendInfo($"{e.Reward.Title} has been updated");
//
// 		await Task.CompletedTask;
// 	}
// }
//
//
// 	
// /*══════════════════【 INTERFACE 】══════════════════*/
// public interface IRewardRedeemUpdateHandler
// {
// 	Task Handle(RewardRedemptionUpdatedEvent e);
// }