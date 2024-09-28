using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

namespace Koishibot.Core.Features.ChannelPoints.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">ChannelPoints Custom Reward Redeemed EventSub</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record RewardRedeemedHandler(
ITwitchUserHub TwitchUserHub,
IDragonQuestService DragonQuestService
) : IRequestHandler<RedeemedRewardCommand>
{
	public async Task Handle
	(RedeemedRewardCommand command, CancellationToken cancellationToken)
	{
		var reward = command.ConvertToDto();
		var user = await TwitchUserHub.Start(reward.User);

		if (reward.Title == "Dragon Egg Quest")
		{
			await DragonQuestService.GetResult(user, reward.RedeemedAt);
		}
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record RedeemedRewardCommand(RewardRedemptionAddedEvent E) : IRequest
{
	public RedeemedRewardDto ConvertToDto()
		=> new(
		new TwitchUserDto(E.ViewerId, E.ViewerLogin, E.ViewerName),
		E.UserInput,
		E.RedeemedAt,
		E.Reward?.Title,
		E.Reward?.Id,
		E.Reward?.Description,
		E.Status.ToString(),
		E.Reward?.Cost);
};