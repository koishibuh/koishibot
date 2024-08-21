using Google.Apis.Calendar.v3.Data;
using Koishibot.Core.Exceptions;
using Settings = Koishibot.Core.Common.Settings;

namespace Koishibot.Core.Features.Obs.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/obs")]
public class StartObsWebsocketController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["OBS"])]
	[HttpPost("connection")]
	public async Task<ActionResult> StartObsWebsocket()
	{
		await Mediator.Send(new StartObsWebsocketCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record StartObsWebsocketHandler(
	IOptions<Settings> Settings,
	IObsService ObsService
	) : IRequestHandler<StartObsWebsocketCommand>
{
	public async Task Handle
		(StartObsWebsocketCommand c, CancellationToken cancel)
	{
		if (string.IsNullOrEmpty(Settings.Value.ObsSettings.WebsocketUrl))
			throw new CustomException("Obs Websocket URL is empty");

		if (string.IsNullOrEmpty(Settings.Value.ObsSettings.Port))
			throw new CustomException("Obs Port is empty");

		await ObsService.CreateWebSocket();
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record StartObsWebsocketCommand : IRequest;