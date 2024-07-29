using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelGoal;

namespace Koishibot.Core.Features.Supports.Events;

public record ChannelGoalProgressCommand
	(ChannelGoalProgressEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ChannelGoalProgressHandler(
IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ChannelGoalProgressCommand>
{
	public async Task Handle(ChannelGoalProgressCommand command, CancellationToken cancel)
	{
		// TODO Alert on UI
		await Task.CompletedTask;
	}
}