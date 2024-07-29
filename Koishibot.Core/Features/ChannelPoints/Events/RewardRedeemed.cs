using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
namespace Koishibot.Core.Features.ChannelPoints.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">ChannelPoints Custom Reward Redeemed EventSub</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record RewardRedeemedHandler(
	ITwitchUserHub TwitchUserHub,
	IDragonEggQuestService DragonEggQuestService
	) : IRequestHandler<RedeemedRewardCommand>
{
	public async Task Handle
		(RedeemedRewardCommand command, CancellationToken cancellationToken)
	{
		var reward = command.ConvertToDto();

		var user = await TwitchUserHub.Start(reward.User);

		if (reward.Title == "Dragon Egg Quest")
		{
			await DragonEggQuestService.GetResult(user, reward.RedeemedAt);
		}
	}
}

// == ⚫ COMMAND == //

public record RedeemedRewardCommand
	(RewardRedemptionAddedEvent e) : IRequest
{
	public RedeemedRewardDto ConvertToDto()
	{
		return new RedeemedRewardDto(
			new TwitchUserDto(e.ViewerId, e.ViewerLogin, e.ViewerName),
			e.UserInput,
			e.RedeemedAt,
			e.Reward?.Title,
			e.Reward?.Id,
			e.Reward?.Description,
			e.Status.ToString(),
			e.Reward?.Cost);
	}
};