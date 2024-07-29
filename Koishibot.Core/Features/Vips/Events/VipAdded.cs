using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Vip;

namespace Koishibot.Core.Features.Vips.Events;

// == ⚫ COMMAND == //

public record VipAddedCommand
	(VipEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record VipAddedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<VipAddedCommand>
{
	public async Task Handle(VipAddedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}