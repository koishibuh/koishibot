using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser;

namespace Koishibot.Core.Features.Moderation;


// == ⚫ COMMAND == //


/// <param name="args"></param>
public record SuspiciousUserUpdatedCommand
	(SuspiciousUserUpdatedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// <para><see href="https://help.twitch.tv/s/article/suspicious-user-controls?language=en_US"/>Twitch Help</para>
/// Ban Evader message will still appear in chat<br/>
/// Likely Ban Evaders will be auto restricted and not shown to chat<br/>
/// </summary>
public record SuspiciousUserUpdatedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<SuspiciousUserUpdatedCommand>
{
	public async Task Handle(SuspiciousUserUpdatedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO Alert on UI
	}
}