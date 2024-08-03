using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Dandle.Controllers;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.Dandle;

// == ⚫ SERVICE == //

public record DandleService(
	IAppCache Cache, KoishibotDbContext Database,
	IChatReplyService ChatReplyService,
	ISignalrService Signalr, ILogger<StartDandleGameHandler> Log
	) : IDandleService
{
	public async Task StartGame()
	{
		if (Cache.DandleIsEnabled()) { return; }

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
	}

	// == ⚫  == //

	public async Task EndGame()
	{
		Cache.ResetDandle();
		Cache.DisableDandle();

		await Signalr.ClearDandleBoard();

		await Signalr.DisableDandleOverlay();
		await ChatReplyService.App(Command.GameOver);
	}
}

// == ⚫ INTERFACE == //

public interface IDandleService
{
	public Task StartGame();
	public Task EndGame();
}