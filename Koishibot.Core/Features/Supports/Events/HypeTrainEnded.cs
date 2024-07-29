using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

namespace Koishibot.Core.Features.Supports.Events;

// == ⚫ COMMAND == //

public record HypeTrainEndedCommand(HypeTrainEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/></para>
/// </summary>
public record HypeTrainEndedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<HypeTrainEndedCommand>
{
	public async Task Handle(HypeTrainEndedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}