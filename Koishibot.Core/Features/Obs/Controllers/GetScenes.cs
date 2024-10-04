using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
public class GetScenesController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Obs"])]
	[HttpGet("scenes")]
	public async Task<ActionResult> GetSceneItems()
	{
		await Mediator.Send(new GetScenesQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetScenesHandler(
IObsService ObsService,
IAppCache Cache
) : IRequestHandler<GetScenesQuery>
{
	public async Task Handle(GetScenesQuery query, CancellationToken cancel)
	{
		var result = Cache.GetStatusByServiceName(ServiceName.ObsWebsocket);
		if (result is false)
		{
			throw new CustomException("ObsWebsocket not connected");
		}

		var request = CreateRequest();
		await ObsService.SendRequest(request);
	}

	private ObsRequest CreateRequest() =>
		new()
		{
			Data = new RequestWrapper
			{
				RequestType = ObsRequests.GetSceneList
			}
		};
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetScenesQuery : IRequest;