using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;

namespace Koishibot.Core.Features.Moderation;

// == ⚫ COMMAND == //

public record UserBannedCommand
	(BannedUserEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ResubReceivedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<UserBannedCommand>
{
	public async Task Handle(UserBannedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}