namespace Koishibot.Core.Features.Obs.Controllers;

// == ⚫ POST == //

public class StartObsWebsocketController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpPost("/api/obs/connection")]
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
	//IObsService ObsService
	IObsService ObsServiceNew
	) : IRequestHandler<StartObsWebsocketCommand>
{
	public async Task Handle
			(StartObsWebsocketCommand c, CancellationToken cancel)
	{
		await ObsServiceNew.CreateWebSocket(cancel);
	}
}