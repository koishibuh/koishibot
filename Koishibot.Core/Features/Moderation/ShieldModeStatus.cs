using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ShieldMode;
namespace Koishibot.Core.Features.Moderation;

// == ⚫ COMMAND == //

public record ShieldModeEndedCommand
	(ShieldModeEndedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ShieldModeEndedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ShieldModeEndedCommand>
{
	public async Task Handle(ShieldModeEndedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}