using Koishibot.Core.Features.TwitchUsers;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Shoutout;
namespace Koishibot.Core.Features.Shoutouts;


/*═══════════════════【 HANDLER 】═══════════════════*/
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

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ShoutoutReceivedCommand
(ShoutoutReceivedEvent args) : IRequest;