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
) : IRewardRedeemedHandler
{
	public async Task Handle(RewardRedemptionAddedEvent e)
	{
		var reward = e.ConvertToDto();
		var user = await TwitchUserHub.Start(reward.User);

		if (reward.Title == "Dragon Egg Quest")
		{
			await DragonQuestService.GetResult(user, reward.RedeemedAt);
			return;
		}

		// Find Id
		var channelPointReward = await Database.GetChannelRewardByName(e.Reward.Title);
		if (channelPointReward.NotInDatabase())
		{
			// query the api for custom reward
			var parameters = new GetCustomRewardsParameters
			{
				BroadcasterId = Settings.Value.StreamerTokens.UserId,
				Id = e.Reward.Id
			};

			var response = await TwitchApiRequest.GetCustomRewards(parameters);
			if (response.IsEmpty())
			{
				// cry
				channelPointReward = new ChannelPointReward
				{
					CreatedOn = DateTimeOffset.UtcNow,
					TwitchId = e.Reward.Id,
					Title = e.Reward.Title,
					Cost = e.Reward.Cost,
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
			Timestamp = e.RedeemedAt,
			UserId = user.Id,
			WasSuccesful = true
		};
		await Database.UpdateEntry(redemption);

		var vm = e.CreateVm();
		await Signalr.SendStreamEvent(vm);
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class RewardRedemptionAddedEventExtensions
{
	public static RedeemedRewardDto ConvertToDto(this RewardRedemptionAddedEvent e)
		=> new(
			new TwitchUserDto(e.ViewerId, e.ViewerLogin, e.ViewerName),
			e.UserInput,
			e.RedeemedAt,
			e.Reward?.Title,
			e.Reward?.Id,
			e.Reward?.Description,
			e.Status.ToString(),
			e.Reward?.Cost);

	public static StreamEventVm CreateVm(this RewardRedemptionAddedEvent e) =>
		new()
		{
			EventType = StreamEventType.ChannelPoint,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{e.ViewerName} has redeemed {e.Reward.Title}"
		};
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IRewardRedeemedHandler
{
	Task Handle(RewardRedemptionAddedEvent e);
}