using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Polls;

// == ⚫ POST == //

public class CreatePollController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Polls"])]
	[HttpPost("/api/polls/twitch")]
	public async Task<ActionResult> CreatePoll
		([FromBody] CreatePollCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

// == ⚫ HANDLER == //

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

// == ⚫ COMMAND == //

public record CreatePollCommand(
	string Title,
	List<string> Choices,
	int Duration
	) : IRequest
{
	public CreatePollRequestBody CreateRequestBody(string streamerId)
	{
		return new CreatePollRequestBody
		{
			BroadcasterId = streamerId,
			PollTitle = Title,
			Choices = Choices
				.Select(title => title )
				.ToList(),
			DurationInSeconds = Duration
		};
	}
};
