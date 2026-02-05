using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Kofi;
using Koishibot.Core.Services.Kofi.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/kofi")]
public class ProcessKofiController(IOptions<Settings> settings,
KoishibotDbContext database,
ISignalrService signalr
) : ApiControllerBase
{
	[AllowAnonymous]
	[HttpPost]
	public async Task<ActionResult> ProcessKofi([FromForm] string data)
	{
		var kofiData = JsonSerializer.Deserialize<KofiEvent>(data)
			?? throw new Exception("KofiData is null");

		// todo: notify UI that verification token is missing
		if (settings.Value.KofiVerificationToken is "")
			return new BadRequestObjectResult(new { error = "Unable to verify token" });

		if (kofiData.VerificationToken != settings.Value.KofiVerificationToken)
			return new BadRequestObjectResult(new { error = "Invalid verification token" });

		var user = await database.GetUserByLogin(kofiData.FromName);

		var kofi = new Kofi().CreateKofi(kofiData, user?.Id);
		await database.UpdateEntry(kofi);

		var tipJarGoal = await database.GetActiveTipJarGoal();
		if (tipJarGoal is not null)
		{
			tipJarGoal.CurrentAmount = tipJarGoal.CurrentAmount + kofi.AmountInPence;
			await database.UpdateEntry(tipJarGoal);
		}
		
		var message = kofiData.Type switch
		{
			KofiType.Donation => $"{kofiData.FromName} tipped {kofiData.Amount} via Kofi",
			KofiType.ShopOrder => $"{kofiData.FromName} purchased an item on Kofi for {kofiData.Amount}",
			KofiType.Commission => $"{kofiData.FromName} purchased a commission for {kofiData.Amount}",
			_ => $"{kofiData.FromName} Kofi subbed for {kofiData.Amount}"
		};

		var kofiVm = new StreamEventVm()
			.CreateKofiEvent(message, kofi.AmountInPence);
		await signalr.SendStreamEvent(kofiVm);
		
		return Ok();
	}
}
	/*═══════════════════【】═══════════════════*/