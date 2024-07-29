using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

namespace Koishibot.Core.Features.Supports.Events;


// == ⚫ COMMAND == //

public record HypeTrainProgressedCommand(HypeTrainEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/></para>
/// </summary>
public record HypeTrainProgressedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<HypeTrainProgressedCommand>
{
	public async Task Handle(HypeTrainProgressedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}