using Koishibot.Core.Services.StreamElements;

namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
public class ConnectStreamElementsController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["StreamElements"])]
	[HttpPost("/api/stream-elements")]
	public async Task<ActionResult> ConnectStreamElements()
	{
		await Mediator.Send(new ConnectStreamElementsCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record ConnectStreamElementsHandler(
IStreamElementsService StreamElementsService
) : IRequestHandler<ConnectStreamElementsCommand>
{
	public async Task Handle
		(ConnectStreamElementsCommand command, CancellationToken cancel)
	{
		await StreamElementsService.CreateWebSocket();
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public class ConnectStreamElementsCommand : IRequest;