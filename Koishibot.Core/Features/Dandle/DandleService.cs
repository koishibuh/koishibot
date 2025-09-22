using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Dandle.Controllers;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;

namespace Koishibot.Core.Features.Dandle;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record DandleService(
IAppCache Cache,
IChatReplyService ChatReplyService,
IServiceScopeFactory ServiceScopeFactory,
ISignalrService Signalr
) : IDandleService
{
	public async Task StartGame()
	{
		await Signalr.ClearDandleBoard();
		
		if (Cache.DandleIsEnabled()) return;

		var dandleDictionary = await GetDandleDictionary();

		var dandleInfo = new DandleGame()
		.SetNewGame()
		.LoadDictionary(dandleDictionary)
		.SelectRandomWord();
		Cache.UpdateDandle(dandleInfo);
		Cache.EnableDandle();

		await Signalr.EnableOverlay(OverlayName.Dandle);
		await ChatReplyService.CreateResponse(Response.NewGame);
	}

	public async Task EndGame()
	{
		Cache.ResetDandle();
		Cache.DisableDandle();

		await Signalr.ClearDandleBoard();

		await Signalr.DisableOverlay(OverlayName.Dandle);
		await ChatReplyService.CreateResponse(Response.GameOver);
	}

	private async Task<List<DandleWord>> GetDandleDictionary()
	{
		using var scope = ServiceScopeFactory.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<KoishibotDbContext>();
		return await database.GetDandleWords();
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IDandleService
{
	public Task StartGame();
	public Task EndGame();
}