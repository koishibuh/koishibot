using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderator;

namespace Koishibot.Core.Features.Moderation;


// == ⚫ COMMAND == //

public record ModeratorAddedCommand
	(ModAddedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ModeratorAddedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ModeratorAddedCommand>
{
	public async Task Handle(ModeratorAddedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}