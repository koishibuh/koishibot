using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;
namespace Koishibot.Core.Features.ChannelPoints.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardadd">ChannelPoints Custom Reward Add EventSub</see></para>
/// Ideally all rewards should be made through the client as they can't be modified otherwise.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record PointRewardCreatedHandler(
	ILogger<PointRewardCreatedHandler> Log,
	KoishibotDbContext Database,
	ISignalrService Signalr
	) : IRequestHandler<PointRewardCreatedCommand>
{
	public async Task Handle
		(PointRewardCreatedCommand command, CancellationToken cancel)
	{
		var pointReward = command.ConvertToModel();
		await Database.UpdateReward(pointReward);

		await Signalr.SendInfo($"{command.args.Title} has been added");
	}
}

// == ⚫ COMMAND == //

public record PointRewardCreatedCommand
	(CustomRewardCreatedEvent args) : IRequest
{
	public ChannelPointReward ConvertToModel()
	{
		return new ChannelPointReward
		{
			CreatedOn = DateTimeOffset.UtcNow,
			TwitchId = args.RewardId,
			Title = args.Title,
			Description = args.Description,
			Cost = args.Cost,
			BackgroundColor = args.BackgroundColor,
			IsEnabled = args.IsEnabled,
			IsUserInputRequired = args.IsUserInputRequired,
			IsMaxPerStreamEnabled = args.MaxPerStream.IsEnabled,
			IsMaxPerUserPerStreamEnabled = args.MaxPerUserPerStream.IsEnabled,
			IsGlobalCooldownEnabled = args.GlobalCooldown.IsEnabled,
			GlobalCooldownSeconds = args.GlobalCooldown.CooldownInSeconds,
			IsPaused = args.IsPaused,
			ShouldRedemptionsSkipRequestQueue = args.ShouldRedemptionsSkipRequestQueue,
			ImageUrl = args.CustomImage is null ? args.DefaultImage.Url4X : args.CustomImage.Url4X
		};
	}
}