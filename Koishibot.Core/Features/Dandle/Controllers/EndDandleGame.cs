using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
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
/// 
/// </summary>
public record EndDandleGameHandler(
	IAppCache Cache,
	IChatReplyService ChatReplyService,
	ISignalrService Signalr
	) : IRequestHandler<EndDandleGameCommand>
{
	public async Task Handle
		(EndDandleGameCommand c, CancellationToken cancel)
	{
		Cache.ResetDandle();
		Cache.DisableDandle();

		await Signalr.ClearDandleBoard();

		await Signalr.DisableDandleOverlay();
		await ChatReplyService.App(Command.GameOver);
	}
}

// == ⚫ COMMAND  == //

public record EndDandleGameCommand() : IRequest;
