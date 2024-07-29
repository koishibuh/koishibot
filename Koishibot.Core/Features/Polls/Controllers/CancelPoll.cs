using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;


namespace Koishibot.Core.Features.Polls;

// == ⚫ DELETE == //

public class CancelPollController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Polls"])]
	[HttpDelete("/api/polls/twitch")]
	public async Task<ActionResult> CancelPoll
		([FromBody] CancelPollCommand command)
	{
		await Mediator.Send(command);
		return Ok();
	}
}

// == ⚫ HANDLER == //

public record CancelPollHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest,
	IAppCache Cache
	) : IRequestHandler<CancelPollCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle
		(CancelPollCommand command, CancellationToken cancel)
	{

		var body = command.CreateRequestBody(StreamerId);
		await TwitchApiRequest.EndPoll(body);

		// TODO: Update poll in cache?
	}
}

// == ⚫ COMMAND == //

public record CancelPollCommand(
	string PollId
	) : IRequest
{
	public EndPollRequestBody CreateRequestBody(string streamerId)
	{

		return new EndPollRequestBody
		{
			BroadcasterId = streamerId,
			PollId = PollId,
			PollStatus = SetPollStatus.Archive
		};
	}
}
