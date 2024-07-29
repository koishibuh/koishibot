using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;

namespace Koishibot.Core.Features.Predictions.Events;

public record PredictionStartedCommand
	(PredictionBeginEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record Handler(
IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<PredictionStartedCommand>
{
	public async Task Handle(PredictionStartedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}