using Koishibot.Core.Services.OBS;
using Koishibot.Core.Services.OBS.Common;
using Koishibot.Core.Services.OBS.Inputs;
namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs/input")]
public class GetInputSettingsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Obs"])]
	[HttpGet("settings")]
	public async Task<ActionResult> GetInputSettings()
	{
		await Mediator.Send(new GetInputSettingsQuery());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record GetInputSettingsHandler(
IOptions<Settings> Settings,
IObsService ObsService
) : IRequestHandler<GetInputSettingsQuery>
{
	public async Task Handle
		(GetInputSettingsQuery query, CancellationToken cancel)
	{
		var request = query.CreateRequest();
		await ObsService.SendRequest(request);
	}
}

/*════════════════════【 QUERY 】════════════════════*/
public record GetInputSettingsQuery : IRequest
{
	public ObsRequest<GetInputSettingsRequest> CreateRequest()
	{
		var request = new RequestWrapper<GetInputSettingsRequest>
		{
			RequestType = ObsRequests.GetInputSettings,
			RequestId = new Guid(),
			RequestData = new GetInputSettingsRequest
			{
				InputUuid = ""
			}
		};

		return new ObsRequest<GetInputSettingsRequest> { Data = request };
	}
}