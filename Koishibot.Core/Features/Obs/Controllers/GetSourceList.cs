using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Scenes;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
public class GetSceneItemsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Obs"])]
	[HttpGet("scene-items")]
	public async Task<ActionResult> GetSceneItems()
	{
		await Mediator.Send(new GetSceneItemsQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetSourceListHandler(
IObsService ObsService,
IAppCache Cache
) : IRequestHandler<GetSceneItemsQuery>
{
	public async Task Handle(GetSceneItemsQuery query, CancellationToken cancel)
	{
		var result = Cache.GetStatusByServiceName(ServiceName.ObsWebsocket);
		if (result is false)
		{
			throw new CustomException("ObsWebsocket not connected");
		}

		var request = query.CreateRequest();
		await ObsService.SendRequest(request);
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetSceneItemsQuery : IRequest
{
	public ObsRequest<GetSceneItemListRequest> CreateRequest()
	{
		var request = new RequestWrapper<GetSceneItemListRequest>
		{
			RequestType = ObsRequests.GetSceneItemList,
			RequestId = new Guid(),
			RequestData = new GetSceneItemListRequest
			{
				SceneUuid = ""
			}
		};

		return new ObsRequest<GetSceneItemListRequest>{ Data = request};
	}
};