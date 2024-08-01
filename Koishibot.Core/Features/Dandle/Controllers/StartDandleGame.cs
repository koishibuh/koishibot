using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.Dandle.Controllers;

// == ⚫ POST  == //

public class StartDandleGameController : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Dandle"])]
	[HttpPost("/api/dandle")]
	public async Task<ActionResult> StartDandleGame()
	{
		var result = await Mediator.Send(new StartDandleGameCommand());
		return Ok(result);
	}
}

// == ⚫ HANDLER  == //

/// <summary>
/// 
/// </summary>
public record StartDandleGameHandler(
	IAppCache Cache, KoishibotDbContext Database,
	IChatReplyService ChatReplyService,
	ISignalrService Signalr, ILogger<StartDandleGameHandler> Log
	) : IRequestHandler<StartDandleGameCommand, string>
{
	public async Task<string> Handle
		(StartDandleGameCommand c, CancellationToken cancel)
	{
		if (Cache.DandleIsEnabled()) { return "Dandle not enabled"; }

		var dandleDictionary = await Database.GetDandleWords();

		var dandleInfo = new DandleGame()
			.SetNewGame()
			.LoadDictionary(dandleDictionary)
			.SelectRandomWord();

		Log.LogInformation($"Selected Dandle word is '{dandleInfo.TargetWord.Word}'");

		Cache.UpdateDandle(dandleInfo);
		Cache.EnableDandle();

		await Signalr.EnableDandleOverlay();
		await ChatReplyService.App(Command.NewGame);
		return dandleInfo.TargetWord.Word;
	}
}

// == ⚫ COMMAND  == //

public record StartDandleGameCommand() : IRequest<string>;
