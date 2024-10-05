using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Warning;

namespace Koishibot.Core.Features.Moderation;


public record UserWarningSentCommand
	(UserWarningSentEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record UserWarningSentCommandHandler(
IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<UserWarningSentCommand>
{
	public async Task Handle(UserWarningSentCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO Alert on UI
	}
}