using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.OBS;
namespace Koishibot.Core.Features.Obs.Controllers;


/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
public class UpdateBrowserSourceController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpPost("source")]
	public async Task<ActionResult> UpdateBrowserSource()
	{
		await Mediator.Send(new UpdateBrowserSourceCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record UpdateBrowserSourceHandler(
IOptions<Settings> Settings,
IObsService ObsService,
IAppCache Cache
) : IRequestHandler<UpdateBrowserSourceCommand>
{
	public async Task Handle
		(UpdateBrowserSourceCommand c, CancellationToken cancel)
	{
		var result = Cache.GetStatusByServiceName(ServiceName.ObsWebsocket);
		if (result is false)
		{
			throw new CustomException("ObsWebsocket not connected");
		}

		// TODO: WIP
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record UpdateBrowserSourceCommand : IRequest;