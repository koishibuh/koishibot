namespace Koishibot.Core.Features.Dandle.Controllers;

// == ⚫ DELETE  == //

public class EndDandleGameController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dandle"])]
	[HttpDelete("/api/dandle")]
	public async Task<ActionResult> EndDandleGame()
	{
		await Mediator.Send(new EndDandleGameCommand());
		return Ok();
	}
}

// == ⚫ HANDLER  == //

/// <summary>
/// Ends the current game of Dandle
/// </summary>
public record EndDandleGameHandler(
	IDandleService DandleService
	) : IRequestHandler<EndDandleGameCommand>
{
	public async Task Handle
		(EndDandleGameCommand c, CancellationToken cancel)
	{
		await DandleService.EndGame();
	}
}

// == ⚫ COMMAND  == //

public record EndDandleGameCommand() : IRequest;