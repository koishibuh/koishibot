using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Scenes;
using Microsoft.AspNetCore.Authorization;
namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
public class GetSceneItemsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Obs"])]
	[AllowAnonymous]
	[HttpGet("scene-items")]
	public async Task<ActionResult> GetSceneItems()
	{
		await Mediator.Send(new GetSceneItemsQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetSourceListHandler(
IObsService ObsService
) : IRequestHandler<GetSceneItemsQuery>
{
	public async Task Handle(GetSceneItemsQuery query, CancellationToken cancel)
	{
		// TODO: Check if OBS is connected first

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
				SceneUuid = "bc7908df-6e98-41ec-b79b-3378d198bb12"
			}
		};

		return new ObsRequest<GetSceneItemListRequest>{ Data = request};
	}
};