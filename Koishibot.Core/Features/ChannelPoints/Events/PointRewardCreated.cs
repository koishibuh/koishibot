using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

namespace Koishibot.Core.Features.ChannelPoints.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardadd">ChannelPoints Custom Reward Add EventSub</see></para>
/// Ideally all rewards should be made through the client as they can't be modified otherwise.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record PointRewardCreatedHandler(
KoishibotDbContext Database,
ISignalrService Signalr
) : IPointRewardCreatedHandler
{
	public async Task Handle(CustomRewardCreatedEvent e)
	{
		var pointReward = e.ConvertToModel();
		await Database.UpdateReward(pointReward);
		await Signalr.SendInfo($"Point Reward '{e.Title}' has been added");
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/

public static class StreamUpdatedEventExtensions
{
	public static ChannelPointReward ConvertToModel(this CustomRewardCreatedEvent e) =>
		new() {
			CreatedOn = DateTimeOffset.UtcNow,
			TwitchId = e.RewardId,
			Title = e.Title,
			Description = e.Description,
			Cost = e.Cost,
			BackgroundColor = e.BackgroundColor,
			IsEnabled = e.IsEnabled,
			IsUserInputRequired = e.IsUserInputRequired,
			IsMaxPerStreamEnabled = e.MaxPerStream.IsEnabled,
			IsMaxPerUserPerStreamEnabled = e.MaxPerUserPerStream.IsEnabled,
			IsGlobalCooldownEnabled = e.GlobalCooldown.IsEnabled,
			GlobalCooldownSeconds = e.GlobalCooldown.CooldownInSeconds,
			IsPaused = e.IsPaused,
			ShouldRedemptionsSkipRequestQueue = e.ShouldRedemptionsSkipRequestQueue,
			ImageUrl = e.CustomImage is null ? e.DefaultImage.Url4X : e.CustomImage.Url4X
		};
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IPointRewardCreatedHandler
{
	Task Handle(CustomRewardCreatedEvent e);
}