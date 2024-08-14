using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/ads")]
public class GetAdScheduleController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpGet("twitch")]
	public async Task<ActionResult> GetAdSchedule()
	{
		var result = await Mediator.Send(new GetAdScheduleQuery());
		return Ok(result);
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetAdScheduleHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetAdScheduleQuery, AdScheduleDto>
{
	public async Task<AdScheduleDto> Handle
		(GetAdScheduleQuery query, CancellationToken cancel)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;

		var parameters = query.CreateParameters(streamerId);
		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		return result.ConvertToDto();
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetAdScheduleQuery : IRequest<AdScheduleDto>
{
	public GetAdScheduleRequestParameters CreateParameters(string streamerId)
		=> new() { BroadcasterId = streamerId };
};