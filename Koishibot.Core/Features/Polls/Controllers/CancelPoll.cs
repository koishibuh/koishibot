using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;
using Microsoft.AspNetCore.Routing;

namespace Koishibot.Core.Features.Polls;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/polls")]
public class CancelPollController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Polls"])]
	[HttpDelete("twitch")]
	public async Task<ActionResult> CancelPoll()
	{
		await Mediator.Send(new CancelPollCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record CancelPollHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest,
IAppCache Cache,
ISignalrService SignalrService,
IChatReplyService ChatReplyService
) : IRequestHandler<CancelPollCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle
		(CancelPollCommand command, CancellationToken cancel)
	{
		var pollId = Cache.GetCurrentPollId();
		if (pollId is null)
		{
			await SignalrService.SendError("Poll Id was null");
			return;
		}

		var body = command.CreateRequestBody(StreamerId, pollId);
		await TwitchApiRequest.EndPoll(body);

		await SignalrService.SendInfo("Poll was cancelled");
		await ChatReplyService.App(Command.PollCancelled);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record CancelPollCommand : IRequest
{
	public EndPollRequestBody CreateRequestBody(string streamerId, string pollId) =>
		new()
		{
			BroadcasterId = streamerId,
			PollId = pollId,
			PollStatus = SetPollStatus.Archive
		};
}