using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Shoutout;

namespace Koishibot.Core.Features.Shoutouts.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
public record ShoutoutSentHandler(
IAppCache Cache,
ILogger<ShoutoutReceivedHandler> Log
) : IRequestHandler<ShoutoutSentCommand>
{
	public async Task Handle(ShoutoutSentCommand command, CancellationToken cancel)
	{
		var user = command.args.ShoutedoutBroadcasterName;
		Log.LogInformation($"{user} has been shouted out");
		await Task.CompletedTask;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record ShoutoutSentCommand(
ShoutoutCreatedEvent args
) : IRequest;