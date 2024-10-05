using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Warning;

namespace Koishibot.Core.Features.Moderation;

public record UserWarningAcknowledgedCommand
	(UserWarningAcknowledgedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record UserWarningAcknowledgedHandler(
IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<UserWarningAcknowledgedCommand>
{
	public async Task Handle(UserWarningAcknowledgedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO Alert on UI
	}
}