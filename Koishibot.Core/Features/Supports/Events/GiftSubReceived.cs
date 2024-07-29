using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;

namespace Koishibot.Core.Features.Supports.Events;


// == ⚫ COMMAND == //

public record GiftSubReceivedCommand
	(GiftedSubEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record GiftSubReceivedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<GiftSubReceivedCommand>
{
	public async Task Handle(GiftSubReceivedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}