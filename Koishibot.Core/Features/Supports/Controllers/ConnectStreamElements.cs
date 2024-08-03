using Koishibot.Core.Services.StreamElements;
namespace Koishibot.Core.Features.Supports.Controllers;


// == ⚫ POST == //

public class ConnectStreamElementsController : ApiControllerBase
{
	private readonly IOptions<Settings> _settings;

	public ConnectStreamElementsController(IOptions<Settings> Settings)
	{
		_settings = Settings;
	}

	[SwaggerOperation(Tags = ["StreamElements"])]
	[HttpPost("/api/stream-elements")]
	public async Task<ActionResult> ProcessKofi()
	{
		await Mediator.Send(new ConnectStreamElementsCommand());
		return Ok();
	}
}

// == ⚫ HANDLER == //

public record ConnectStreamElementsHandler(
	IStreamElementsService StreamElementsService
		) : IRequestHandler<ConnectStreamElementsCommand>
{
	public async Task Handle
			(ConnectStreamElementsCommand command, CancellationToken cancel)
	{
		await StreamElementsService.CreateWebSocket(cancel);

	}
}

public class ConnectStreamElementsCommand : IRequest;