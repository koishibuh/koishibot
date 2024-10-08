using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelGoal;

namespace Koishibot.Core.Features.Supports.Events;


public record ChannelGoalStartedCommand
	(ChannelGoalStartedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ChannelGoalStartedHandler(
IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ChannelGoalStartedCommand>
{
	public async Task Handle(ChannelGoalStartedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO Alert on UI
	}
}