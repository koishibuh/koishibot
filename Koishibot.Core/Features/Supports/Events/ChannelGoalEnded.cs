using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelGoal;
namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ COMMAND == //

public record ChannelGoalEndedCommand(ChannelGoalEndedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ChannelGoalEndedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ChannelGoalEndedCommand>
{
	public async Task Handle(ChannelGoalEndedCommand command, CancellationToken cancel)
	{
		// TODO Alert on UI
		await Task.CompletedTask;
	}
}