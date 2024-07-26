//using Koishibot.Core.Persistence;
//namespace Koishibot.Core.Features.Dandle.Controllers;

//// == ⚫ POST  == //

//public class StartDandleGameController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = new[] { "Dandle" })]
//	[HttpPost("/api/dandle")]
//	public async Task<ActionResult> StartDandleGame()
//	{
//		var result =  await Mediator.Send(new StartDandleGameCommand());
//		return Ok(result);
//	}
//}

//// == ⚫ COMMAND  == //

//public record StartDandleGameCommand() : IRequest<string>;

//// == ⚫ HANDLER  == //

///// <summary>
///// 
///// </summary>
//public record StartDandleGameHandler(
//	IAppCache Cache, KoishibotDbContext Database,
//	//IChatMessageService BotIrc,
//	ISignalrService Signalr, ILogger<StartDandleGameHandler> Log
//	) : IRequestHandler<StartDandleGameCommand, string>
//{
//	public async Task<string> Handle
//		(StartDandleGameCommand c, CancellationToken cancel)
//	{
//		//if (Cache.DandleIsEnabled()) { return "Dandle not enabled"; }

//		//var dandleDictionary = await Database.GetDandleWords();

//		//var dandleInfo = new DandleGame()
//		//	.SetNewGame()
//		//	.LoadDictionary(dandleDictionary)
//		//	.SelectRandomWord();

//		//Log.LogInformation($"Selected Dandle word is '{dandleInfo.TargetWord.Word}'");

//		//Cache.UpdateDandle(dandleInfo);
//		//Cache.EnableDandle();

//		//await Signalr.EnableDandleOverlay();
//		//await BotIrc.PostDandleGameStarted();
//		//return dandleInfo.TargetWord.Word;
//		return "";
//	}
//}