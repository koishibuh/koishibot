using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;

namespace Koishibot.Core.Features.Predictions.Events;

// == ⚫ COMMAND == //

public record PredictionEndedCommand
	(PredictionEndedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record PredictionEndedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<PredictionEndedCommand>
{
	public async Task Handle(PredictionEndedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}