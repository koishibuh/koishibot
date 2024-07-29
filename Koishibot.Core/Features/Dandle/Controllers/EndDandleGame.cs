using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
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
	ITwitchIrcService BotIrc,
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
		await BotIrc.PostDandleGameEnded();
	}
}

// == ⚫ COMMAND  == //

public record EndDandleGameCommand() : IRequest;
