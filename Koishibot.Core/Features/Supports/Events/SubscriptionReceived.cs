using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;

namespace Koishibot.Core.Features.Supports.Events;


// == ⚫ COMMAND == //

public record SubscriptionReceivedCommand
	(SubscriptionEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record SubscriptionReceivedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<SubscriptionReceivedCommand>
{
	public async Task Handle(SubscriptionReceivedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}