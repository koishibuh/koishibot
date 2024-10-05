using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.ChannelPoints.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">ChannelPoints Custom Reward Redeemed EventSub</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record RewardRedeemedHandler(
ITwitchUserHub TwitchUserHub,
IDragonQuestService DragonQuestService,
KoishibotDbContext Database,
ITwitchApiRequest TwitchApiRequest,
IOptions<Settings> Settings,
ISignalrService Signalr
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
			return;
		}

		// Find Id
		var channelPointReward = await Database.GetChannelRewardByName(command.E.Reward.Title);
		if (channelPointReward.NotInDatabase())
		{
			// query the api for custom reward
			var parameters = new GetCustomRewardsParameters
			{
				BroadcasterId = Settings.Value.StreamerTokens.UserId,
				Id = command.E.Reward.Id
			};

			var response = await TwitchApiRequest.GetCustomRewards(parameters);
			if (response.IsEmpty())
			{
				// cry
				channelPointReward = new ChannelPointReward
				{
					CreatedOn = DateTimeOffset.UtcNow,
					TwitchId = command.E.Reward.Id,
					Title = command.E.Reward.Title,
					Cost = command.E.Reward.Cost,
					IsEnabled = true,
					IsUserInputRequired = false,
					IsMaxPerStreamEnabled = false,
					IsMaxPerUserPerStreamEnabled = false,
					IsGlobalCooldownEnabled = false,
					IsPaused = false,
					ShouldRedemptionsSkipRequestQueue = true
				};

				await Signalr.SendError($"Info for Reward {channelPointReward.Title} not found by Twitch");
			}
			else
			{
				var theReward = response[0];

				channelPointReward = new ChannelPointReward
				{
					CreatedOn = DateTimeOffset.UtcNow,
					TwitchId = theReward.RewardId,
					Title = theReward.Title,
					Description = theReward.Description,
					Cost = theReward.Cost,
					BackgroundColor = theReward.BackgroundColor,
					IsEnabled = theReward.IsEnabled,
					IsUserInputRequired = theReward.IsUserInputRequired,
					IsMaxPerStreamEnabled = theReward.MaxPerStreamSetting.IsEnabled,
					MaxPerStream = theReward.MaxPerStreamSetting.MaxPerStream,
					IsMaxPerUserPerStreamEnabled = theReward.MaxPerUserPerStreamSetting.IsEnabled,
					MaxPerUserPerStream = theReward.MaxPerUserPerStreamSetting.MaxPerUserPerStream,
					IsGlobalCooldownEnabled = theReward.GlobalCooldownSetting.IsEnabled,
					GlobalCooldownSeconds = theReward.GlobalCooldownSetting.GlobalCooldownSeconds,
					IsPaused = theReward.IsPaused,
					ShouldRedemptionsSkipRequestQueue = theReward.ShouldRedemptionsSkipRequestQueue,
					ImageUrl = theReward.CustomImage is null ? theReward.DefaultImage.Url4X : theReward.CustomImage.Url4X
				};
			}

			await Database.UpdateEntry(channelPointReward);
		}

		// save redemption to database
		var redemption = new ChannelPointRedemption()
		{
			ChannelPointRewardId = channelPointReward.Id,
			Timestamp = command.E.RedeemedAt,
			UserId = user.Id,
			WasSuccesful = true
		};
		await Database.UpdateEntry(redemption);

		var vm = command.CreateVm();
		await Signalr.SendStreamEvent(vm);
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

	public StreamEventVm CreateVm() =>
		new()
		{
			EventType = StreamEventType.ChannelPoint,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{E.ViewerName} has redeemed {E.Reward.Title}"
		};
}