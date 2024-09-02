using Koishibot.Core.Services.OBS;

namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
public class StopObsWebsocketController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpDelete("connection")]
	public async Task<ActionResult> StopObsWebsocket()
	{
		await Mediator.Send(new StopObsWebsocketCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record StopObsWebsocketHandler(
IObsService ObsService
) : IRequestHandler<StopObsWebsocketCommand>
{
	public async Task Handle
	(StopObsWebsocketCommand c, CancellationToken cancel)
	{
		await ObsService.Disconnect();
		await Task.CompletedTask;
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record StopObsWebsocketCommand : IRequest;