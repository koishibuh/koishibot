using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;

namespace Koishibot.Core.Features.Predictions.Events;

public record PredictionLockedCommand
	(PredictionLockedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record SHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<PredictionLockedCommand>
{
	public async Task Handle(PredictionLockedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}