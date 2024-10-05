using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;
namespace Koishibot.Core.Features.Predictions.Events;

public record PredictionReceivedCommand
	(PredictionProgressEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record PredictionReceivedHandler(
IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<PredictionReceivedCommand>
{
	public async Task Handle(PredictionReceivedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}