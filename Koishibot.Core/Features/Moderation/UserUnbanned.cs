using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.BanUser;

namespace Koishibot.Core.Features.Moderation;

// == ⚫ COMMAND == //

public record UserUnbannedCommand
	(UnbannedUserEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record UserUnbannedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<UserUnbannedCommand>
{
	public async Task Handle(UserUnbannedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}