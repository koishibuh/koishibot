using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.Polls;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/polls")]
public class CreatePollController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Polls"])]
	[HttpPost("twitch")]
	public async Task<ActionResult> CreatePoll
		([FromBody] CreatePollCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record CreatePollHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<CreatePollCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle(CreatePollCommand command, CancellationToken cancel)
	{
		var body = command.CreateRequestBody(StreamerId);
		await TwitchApiRequest.CreatePoll(body);

		// Get response from StreamPollBegin Eventsub
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record CreatePollCommand(
string Title,
List<string> Choices,
int Duration
) : IRequest
{
	public CreatePollRequestBody CreateRequestBody(string streamerId) => new()
	{
		BroadcasterId = streamerId,
		PollTitle = Title,
		Choices = Choices
			.Select(x => new ChoiceTitle{Title = x})
			.ToList(),
		DurationInSeconds = Duration
	};
}