using Koishibot.Core.Services.TwitchApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Koishibot.Core.Features.Supports.Controllers;


/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/stream-goals")]
[AllowAnonymous]
public class GetStreamGoalsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Goals"])]
	[HttpGet("twitch")]
	public async Task<ActionResult> GetStreamGoals()
	{
		await Mediator.Send(new GetStreamGoalsQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetStreamGoalsHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetStreamGoalsQuery>
{
	public async Task Handle
	(GetStreamGoalsQuery query, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;

		var parameters = query.CreateParameters(streamerId);
		var result = await TwitchApiRequest.GetCreatorGoals(parameters);
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetStreamGoalsQuery : IRequest
{
	public GetCreatorGoalsRequestParameters CreateParameters(string streamerId)
		=> new(){ BroadcasterId = streamerId };
}