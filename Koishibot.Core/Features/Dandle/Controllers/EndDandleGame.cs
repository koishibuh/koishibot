using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
namespace Koishibot.Core.Features.Dandle.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/dandle")]
public class EndDandleGameController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dandle"])]
	[HttpDelete]
	public async Task<ActionResult> EndDandleGame()
	{
		await Mediator.Send(new EndDandleGameCommand());
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// Ends the current game of Dandle
/// </summary>
public record EndDandleGameHandler(
IChatReplyService ChatReplyService,
IDandleTimer DandleTimer,
ISignalrService Signalr,
IAppCache Cache
) : IRequestHandler<EndDandleGameCommand>
{
	public async Task Handle(EndDandleGameCommand c, CancellationToken cancel)
	{
		DandleTimer.CancelTimer();
		Cache.ResetDandle();
		Cache.DisableDandle();

		await Signalr.ClearDandleBoard();
		await Signalr.DisableOverlay(OverlayName.Dandle);
		await ChatReplyService.CreateResponse(Command.GameOver);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record EndDandleGameCommand : IRequest;