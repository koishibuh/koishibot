using Koishibot.Core.Features.Obs.Interfaces;
namespace Koishibot.Core.Features.Obs.Controllers;

// == ⚫ POST == //

public class StartObsWebsocketController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpPost("/api/obs/")]
	public async Task<ActionResult> StartObsWebsocket()
	{
		await Mediator.Send(new StartObsWebsocketCommand());
		return Ok();
	}
}

// == ⚫ COMMAND == //

public record StartObsWebsocketCommand() : IRequest;

// == ⚫ HANDLER == //

public record StartObsWebsocketHandler(
	IObsService ObsService
	) : IRequestHandler<StartObsWebsocketCommand>
{
	public async Task Handle
			(StartObsWebsocketCommand c, CancellationToken cancel)
	{
		await ObsService.StartWebsocket();
	}
}