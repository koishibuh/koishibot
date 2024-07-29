using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ COMMAND == //

public record HypeTrainStartedCommand(HypeTrainEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Channel Point Auto Reward Redemption Add EventSub Documentation</para>
/// </summary>
public record HypeTrainStartedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<HypeTrainStartedCommand>
{
	public async Task Handle(HypeTrainStartedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}