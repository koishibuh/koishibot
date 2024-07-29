using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Shoutout;
namespace Koishibot.Core.Features.Shoutouts;


// == ⚫ COMMAND == //

public record ShoutoutReceivedCommand
	(ShoutoutReceivedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href=""/>Twitch Documentation</para>
/// </summary>
public record ShoutoutReceivedHandler(
	IAppCache Cache,
	ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database
	) : IRequestHandler<ShoutoutReceivedCommand>
{
	public async Task Handle(ShoutoutReceivedCommand command, CancellationToken cancel)
	{
		await Task.CompletedTask;
		// TODO
	}
}