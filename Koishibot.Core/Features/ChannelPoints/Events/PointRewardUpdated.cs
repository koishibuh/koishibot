using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

namespace Koishibot.Core.Features.ChannelPoints.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardupdate">ChannelPoints Custom Reward Update EventSub</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record PointRewardUpdatedHandler(
ILogger<PointRewardUpdatedHandler> Log,
KoishibotDbContext Database,
ISignalrService Signalr
) : IPointRewardUpdatedHandler
{
	public async Task Handle(CustomRewardUpdatedEvent e)
	{
		var pointReward = e.CreateModel();
		await Database.UpdateReward(pointReward);

		await Signalr.SendInfo($"Point Reward '{e.Title}' has been updated");
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public static class PointRewardUpdatedHandlerExtensions
{
	public static ChannelPointReward CreateModel(this CustomRewardUpdatedEvent e)
		=> new()
		{
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
};

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IPointRewardUpdatedHandler
{
	Task Handle(CustomRewardUpdatedEvent e);
}