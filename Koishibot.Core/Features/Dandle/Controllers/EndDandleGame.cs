//using Koishibot.Core.Features.Dandle.Extensions;
//namespace Koishibot.Core.Features.Dandle.Controllers;

//// == ⚫ DELETE  == //

//public class EndDandleGameController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = new[] { "Dandle" })]
//	[HttpDelete("/api/dandle")]
//	public async Task<ActionResult> EndDandleGame()
//	{
//		await Mediator.Send(new EndDandleGameCommand());
//		return Ok();
//	}
//}

//// == ⚫ COMMAND  == //

//public record EndDandleGameCommand() : IRequest;

//// == ⚫ HANDLER  == //

///// <summary>
///// 
///// </summary>
//public record EndDandleGameHandler(
//	IAppCache Cache, 
//	IChatMessageService BotIrc,
//	ISignalrService Signalr
//	) : IRequestHandler<EndDandleGameCommand>
//{
//	public async Task Handle
//		(EndDandleGameCommand c, CancellationToken cancel)
//	{
//		Cache.ResetDandle();
//		Cache.DisableDandle();

//		await Signalr.ClearDandleBoard();

//		await Signalr.DisableDandleOverlay();
//		await BotIrc.PostDandleGameEnded();
//	}
//}
