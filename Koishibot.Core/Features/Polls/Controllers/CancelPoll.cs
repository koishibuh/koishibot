//using Koishibot.Core.Features.Common;
//using Swashbuckle.AspNetCore.Annotations;
//using TwitchLib.Api.Core.Enums;

//namespace Koishibot.Core.Features.Polls;

//// == ⚫ DELETE == //

//public class CancelPollController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Polls"])]
//	[HttpDelete("/api/polls/twitch")]
//	public async Task<ActionResult> CancelPoll
//		([FromBody] CancelPollCommand command)
//	{
//		var result = await Mediator.Send(command);
//		return Ok(result);
//	}
//}

//// == ⚫ COMMAND == //

//public record CancelPollCommand(string PollId) : IRequest<bool>;

//// == ⚫ HANDLER == //

//public record CancelPollHandler(
//	IAppCache Cache, ITwitchPollApi TwitchApi
//	) : IRequestHandler<CancelPollCommand, bool>
//{
//	public async Task<bool> Handle
//		(CancelPollCommand command, CancellationToken cancel)
//	{
//		return await TwitchApi.EndPoll(command.PollId);
//		// TODO: Update poll in cache?
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record TwitchPollApi : ITwitchPollApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#end-poll">End Poll Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<bool> EndPoll(string pollId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Polls.EndPollAsync
//			(StreamerId, pollId, PollStatusEnum.ARCHIVED);

//		return result.Data.Length == 0
//			? false
//			: result.IsPollStatusArchived();
//	}
//}