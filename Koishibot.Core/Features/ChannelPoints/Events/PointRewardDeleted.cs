using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

namespace Koishibot.Core.Features.ChannelPoints.Events;

// == ⚫ COMMAND == //

public record PointRewardDeletedCommand
	(CustomRewardRemovedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_rewardremove">Twitch Documentation</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record CustomRewardDeletedHandler(
		ILogger<PointRewardUpdatedHandler> Log
	) : IRequestHandler<PointRewardDeletedCommand>
{
	public async Task Handle(PointRewardDeletedCommand command,
		CancellationToken cancel)
	{
		// TODO: Alert on UI, remove from Database
		await Task.CompletedTask;
	}
}
