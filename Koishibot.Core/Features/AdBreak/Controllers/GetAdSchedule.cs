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

// == ⚫ QUERY == //

public record GetAdScheduleQuery(): IRequest<AdScheduleDto>;

// == ⚫ HANDLER == //

public record GetAdScheduleHandler(
	IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest
) : IRequestHandler<GetAdScheduleQuery, AdScheduleDto>
{
	public async Task<AdScheduleDto> Handle
		(GetAdScheduleQuery query, CancellationToken cancel)
	{
		var parameters = CreateParameters();
		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		return result.ConvertToDto();
	}

	public GetAdScheduleRequestParameters CreateParameters()
	{
		return new GetAdScheduleRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId
		};
	}
}