//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Polls.Models;
//namespace Koishibot.Core.Features.Polls;

//// == ⚫ POST == //

//public class CreatePollController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Polls"])]
//	[HttpPost("/api/polls/twitch")]
//	public async Task<ActionResult> CreatePoll
//		([FromBody] CreatePollCommand command)
//	{
//		await Mediator.Send(command);
//		return Ok();
//	}
//}

//// == ⚫ COMMAND == //

//public record CreatePollCommand(
//	string Title,
//	List<string> Choices,
//	int Duration
//	) : IRequest;


//// == ⚫ HANDLER == //

//public record CreatePollHandler(
//	ITwitchPollApi CreatePollApi
//	) : IRequestHandler<CreatePollCommand>
//{
//	public async Task Handle(CreatePollCommand c, CancellationToken cancel)
//	{
//		var poll = new PendingPoll();
//		poll.Set(c);

//		await CreatePollApi.StartPoll(poll);
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record TwitchPollApi : ITwitchPollApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-poll">Create Poll Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<string?> StartPoll(PendingPoll pendingPoll)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var request = pendingPoll.CreatePollRequest(StreamerId);

//		var result = await TwitchApi.Helix.Polls.CreatePollAsync(request);
//		return result is null || result.Data.Length == 0
//			? throw new Exception("Error while trying to Create A Poll via Api")
//			: result.Data[0].Id;
//	}
//}