using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser;

namespace Koishibot.Core.Features.Moderation;


// == ⚫ COMMAND == //


/// <param name="args"></param>
public record SuspiciousUserAlertCommand
	(SuspiciousUserMessageEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// <para><see href="https://help.twitch.tv/s/article/suspicious-user-controls?language=en_US"/>Twitch Help</para>
/// Ban Evader message will still appear in chat<br/>
/// Likely Ban Evaders will be auto restricted and not shown to chat<br/>
/// </summary>
public record SuspiciousUserAlertHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<SuspiciousUserAlertCommand>
{
	public async Task Handle(SuspiciousUserAlertCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO Alert on UI
	}
}