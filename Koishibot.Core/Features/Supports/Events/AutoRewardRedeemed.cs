using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
namespace Koishibot.Core.Features.Supports.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_automatic_reward_redemptionadd-v2"/>Channel Point Auto Reward Redemption Add EventSub Documentation</para>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-points-automatic-reward-redemption-add-v2-event">Event</see></para><br/>
/// PowerUps: MessageEffect, GigantifyAnEmote, Celebration</br>
/// PointRedeems: BypassSubOnly, HighlightedMessage, RandomSubEmote, ChosenSubEmote, ChosenModifySubEmote 
/// </summary>
public record AutoRewardRedeemedHandler(
IAppCache Cache, ISignalrService Signalr,
ITwitchUserHub TwitchUserHub,
KoishibotDbContext Database
) : IAutoRewardRedeemedHandler
{
	public async Task Handle(AutomaticRewardRedemptionEvent e)
	{
		var userDto = e.CreateUserDto();
		var user = await TwitchUserHub.Start(userDto);

		var channelPointReward = await Database.GetChannelRewardByName(e.Reward.Type.ToString());
		if (channelPointReward.NotInDatabase())
		{
			channelPointReward = new ChannelPointReward
			{
				CreatedOn = DateTimeOffset.UtcNow,
				TwitchId = e.Reward.Type.ToString(),
				Title = e.Reward.Type.ToString(),
				Cost = e.Reward.Cost,
				IsEnabled = true,
				IsUserInputRequired = false,
				IsMaxPerStreamEnabled = false,
				IsMaxPerUserPerStreamEnabled = false,
				IsGlobalCooldownEnabled = false,
				IsPaused = false,
				ShouldRedemptionsSkipRequestQueue = true
			};

			await Database.UpdateEntry(channelPointReward);
		}

		var redemption = e.CreateRedemption(channelPointReward.Id, user);
		await Database.UpdateEntry(redemption);

		var vm = e.CreateVm();
		await Signalr.SendStreamEvent(vm);
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class AutomaticRewardRedemptionEventExtensions
{
	public static TwitchUserDto CreateUserDto(this AutomaticRewardRedemptionEvent e) =>
		new(
			e.BroadcasterId,
			e.BroadcasterLogin,
			e.BroadcasterName);

	public static RedeemedRewardDto ConvertToDto(this AutomaticRewardRedemptionEvent e) => new(
		new TwitchUserDto(e.RedeemedById, e.RedeemedByLogin, e.RedeemedById),
		e.Message.Text,
		e.RedeemedAt,
		e.Reward?.Type.ToString(),
		"0",
		e.Reward.Type.ToString(),
		true.ToString(),
		e.Reward?.Cost);

	public static ChannelPointRedemption CreateRedemption(this AutomaticRewardRedemptionEvent e, int id, TwitchUser user) =>
		new()
		{
			ChannelPointRewardId = id,
			Timestamp = e.RedeemedAt,
			UserId = user.Id,
			WasSuccesful = true
		};

	public static StreamEventVm CreateVm(this AutomaticRewardRedemptionEvent e)
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.ChannelPoint,
			Timestamp = Toolbox.CreateUITimestamp(),
			Message = $"{e.RedeemedByUsername} has redeemed {e.Reward.Type.ToString()}"
		};
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IAutoRewardRedeemedHandler
{
	Task Handle(AutomaticRewardRedemptionEvent e);
}