using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Controllers;

// == ⚫ GET == //

public class GetAdScheduleController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Ad Schedule"])]
	[HttpGet("/api/ads/twitch")]
	public async Task<ActionResult> GetAdSchedule()
	{
		var result = await Mediator.Send(new GetAdScheduleQuery());
		return Ok(result);
	}
}

// == ⚫ HANDLER == //

public record GetAdScheduleHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetAdScheduleQuery, AdScheduleDto>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task<AdScheduleDto> Handle
		(GetAdScheduleQuery query, CancellationToken cancel)
	{
		var parameters = query.CreateParameters(StreamerId);
		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		return result.ConvertToDto();
	}
}

// == ⚫ QUERY == //

public record GetAdScheduleQuery() : IRequest<AdScheduleDto>
{
	public GetAdScheduleRequestParameters CreateParameters(string streamerId) 
		=> new GetAdScheduleRequestParameters	{BroadcasterId = streamerId };
};
