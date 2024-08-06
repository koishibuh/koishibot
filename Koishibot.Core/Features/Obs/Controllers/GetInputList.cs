using Koishibot.Core.Services.OBS.Common;

namespace Koishibot.Core.Features.Obs.Controllers;

// == ⚫ GET == //

public class GetInputListController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Obs"])]
	[HttpGet("/api/obs/input")]
	public async Task<ActionResult> GetInputList()
	{
		await Mediator.Send(new GetInputListQuery());
		return Ok();
	}
}

// == ⚫ HANDLER == //

public record GetAdScheduleHandler(
	IOptions<Settings> Settings,
	IObsService ObsServiceNew
) : IRequestHandler<GetInputListQuery>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task Handle
		(GetInputListQuery query, CancellationToken cancel)
	{
		var requestType = ObsRequestStrings.GetSceneList;

		await ObsServiceNew.SendRequest(requestType);
	
	}
}

// == ⚫ QUERY == //

public record GetInputListQuery() : IRequest
{

};
