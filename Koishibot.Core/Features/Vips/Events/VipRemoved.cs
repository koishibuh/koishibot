using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Vip;
namespace Koishibot.Core.Features.Vips.Events;


public record VipRemovedCommand
	(VipEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record VipRemovedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<VipRemovedCommand>
{
	public async Task Handle(VipRemovedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}