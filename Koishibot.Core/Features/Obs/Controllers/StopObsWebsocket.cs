using Koishibot.Core.Services.OBS;

namespace Koishibot.Core.Features.Obs.Controllers;

// == ⚫ DELETE == //

public class StopObsWebsocketController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpDelete("/api/obs/connection")]
	public async Task<ActionResult> StopObsWebsocket()
	{
		await Mediator.Send(new StopObsWebsocketCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record StopObsWebsocketCommand() : IRequest;

// == ⚫ HANDLER == //

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