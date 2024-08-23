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
) : IRequestHandler<PointRewardUpdatedCommand>
{
	public async Task Handle
	(PointRewardUpdatedCommand command, CancellationToken cancellationToken)
	{
		var pointReward = command.CreateModel();
		await Database.UpdateReward(pointReward);

		await Signalr.SendInfo($"Point Reward '{command.Args.Title}' has been updated");
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record PointRewardUpdatedCommand(
CustomRewardUpdatedEvent Args
) : IRequest
{
	public ChannelPointReward CreateModel()
		=> new()
		{
		CreatedOn = DateTimeOffset.UtcNow,
		TwitchId = Args.RewardId,
		Title = Args.Title,
		Description = Args.Description,
		Cost = Args.Cost,
		BackgroundColor = Args.BackgroundColor,
		IsEnabled = Args.IsEnabled,
		IsUserInputRequired = Args.IsUserInputRequired,
		IsMaxPerStreamEnabled = Args.MaxPerStream.IsEnabled,
		IsMaxPerUserPerStreamEnabled = Args.MaxPerUserPerStream.IsEnabled,
		IsGlobalCooldownEnabled = Args.GlobalCooldown.IsEnabled,
		GlobalCooldownSeconds = Args.GlobalCooldown.CooldownInSeconds,
		IsPaused = Args.IsPaused,
		ShouldRedemptionsSkipRequestQueue = Args.ShouldRedemptionsSkipRequestQueue,
		ImageUrl = Args.CustomImage is null ? Args.DefaultImage.Url4X : Args.CustomImage.Url4X
		};
};