using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Kofi;
using Koishibot.Core.Services.StreamElements.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
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
	[SwaggerOperation(Tags = ["Kofi"])]
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

		var user = await database.GetUserByLogin(kofiData.FromName.ToLower());

		var kofi = new Kofi
		{
			KofiTransactionId = kofiData.KofiTransactionId,
			Timestamp = kofiData.Timestamp,
			TransactionUrl = kofiData.Url,
			KofiType = kofiData.Type.ToString(),
			UserId = user?.Id,
			Username = kofiData.FromName,
			Message = kofiData.Message ?? string.Empty,
			Currency = kofiData.Currency,
			AmountInPence = Toolbox.AmountStringToPence(kofiData.Amount)
		};
		await database.UpdateEntry(kofi);

		var kofiVm = kofi.CreateVm(kofiData.Amount);
		await signalr.SendStreamEvent(kofiVm);

		// Update overlay bar
		return Ok();
	}
}
	/*═══════════════════【】═══════════════════*/