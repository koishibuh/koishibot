using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderator;

namespace Koishibot.Core.Features.Moderation;

// == ⚫ COMMAND == //

public record ModeratorRemovedCommand
	(ModRemovedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ModeratorRemovedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ModeratorRemovedCommand>
{
	public async Task Handle(ModeratorRemovedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}