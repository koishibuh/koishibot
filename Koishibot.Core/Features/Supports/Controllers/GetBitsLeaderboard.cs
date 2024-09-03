using Koishibot.Core.Services.TwitchApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/bits-leaderboard")]
public class GetBitsLeaderboardController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Bits Leaderboard"])]
	[HttpGet("twitch")]
	public async Task<ActionResult> GetBitsLeaderboard()
	{
		await Mediator.Send(new GetBitsLeaderboardQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetBitsLeaderboardHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetBitsLeaderboardQuery>
{
	public async Task Handle
	(GetBitsLeaderboardQuery query, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;

		var parameters = query.CreateParameters();
		var result = await TwitchApiRequest.GetBitsLeaderboard(parameters);
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetBitsLeaderboardQuery : IRequest
{
	public GetBitsLeaderboardRequestParameters CreateParameters() => new();
}