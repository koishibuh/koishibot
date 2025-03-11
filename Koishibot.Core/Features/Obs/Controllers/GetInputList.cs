using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
namespace Koishibot.Core.Features.Obs.Controllers;

/// <summary>
/// Requests list of sources in OBS
/// </summary>
/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
public class GetInputListController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Obs"])]
	[HttpGet("input")]
	public async Task<ActionResult> GetInputList()
	{
		await Mediator.Send(new GetInputListQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#getinputlist
/// </summary>
/// <param name="ObsService"></param>
public record GetInputListHandler(
IObsService ObsService
) : IRequestHandler<GetInputListQuery>
{
	public async Task Handle
	(GetInputListQuery query, CancellationToken cancel)
	{
		var request = new ObsRequest
		{
			Data = new RequestWrapper
			{
				RequestType = ObsRequests.GetInputList
			}
		};

		await ObsService.SendRequest(request);
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetInputListQuery : IRequest;