namespace Koishibot.Core.Features.Dandle.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
public class StartDandleGameController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dandle"])]
	[HttpPost("/api/dandle")]
	public async Task<ActionResult> StartDandleGame()
	{
		await Mediator.Send(new StartDandleGameCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// Starts a game of Dandle
/// </summary>
public record StartDandleGameHandler(
IDandleService DandleService
) : IRequestHandler<StartDandleGameCommand>
{
	public async Task Handle(StartDandleGameCommand c, CancellationToken cancel)
		=> await DandleService.StartGame();
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record StartDandleGameCommand() : IRequest;