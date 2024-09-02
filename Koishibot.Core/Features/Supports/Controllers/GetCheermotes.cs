using Koishibot.Core.Services.TwitchApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/cheermotes")]
public class GetCheermotesController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Bits"])]
	[HttpGet("twitch")]
	public async Task<ActionResult> GetCheermotes()
	{
		await Mediator.Send(new GetCheermotesQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetCheermotesHandler(
IOptions<Settings> Settings,
ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetCheermotesQuery>
{
	public async Task Handle
	(GetCheermotesQuery query, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;

		var parameters = query.CreateParameters(streamerId);
		var result = await TwitchApiRequest.GetCheermotes(parameters);
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetCheermotesQuery : IRequest
{
	public GetCheermotesRequestParameters CreateParameters(string streamerId)
		=> new(){ BroadcasterId = streamerId };
}