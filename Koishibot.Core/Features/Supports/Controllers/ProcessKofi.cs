using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Kofi;
using System.Text.Json;
namespace Koishibot.Core.Features.Supports.Controllers;

/*══════════════════【 CONTROLLER 】══════════════════*/
[Route("api/kofi")]
public class ProcessKofiController(IOptions<Settings> settings) : ApiControllerBase
{
	[SwaggerOperation(Tags = ["Kofi"])]
	[HttpPost]
	public async Task<ActionResult> ProcessKofi([FromForm] string data)
	{
		var kofiData = JsonSerializer.Deserialize<KofiEvent>(data)
			?? throw new Exception("KofiData is null");

		if (kofiData.VerificationToken != settings.Value.KofiVerificationToken)
		{
			return new BadRequestObjectResult(new { error = "Invalid verification token" });
		}

		await Mediator.Send(new ProcessKofiCommand { Data = kofiData });
		return Ok();
	}
}

/*═══════════════════【 HANDLER 】═══════════════════*/
public record ProcessKofiHandler(
KoishibotDbContext Database,
ISignalrService Signalr
) : IRequestHandler<ProcessKofiCommand>
{
	public async Task Handle
		(ProcessKofiCommand command, CancellationToken cancel)
	{
		var userLogin = command.GetUserLogin();
		var user = await Database.GetUserByLogin(userLogin);

		var kofi = command.CreateModel(user);
		await Database.UpdateEntry(kofi);

		var kofiVm = command.CreateVm();
		await Signalr.SendStreamEvent(kofiVm);

		// Update overlay bar
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public class ProcessKofiCommand : IRequest
{
	public KofiEvent Data { get; set; }

	public string GetUserLogin() => Data.FromName.ToLower();

	public Kofi CreateModel(TwitchUser? user) => new Kofi
	{
		KofiTransactionId = Data.KofiTransactionId,
		Timestamp = Data.Timestamp,
		TransactionUrl = Data.Url,
		KofiType = Data.Type,
		UserId = user?.Id,
		Username = Data.FromName,
		Message = Data.Message ?? string.Empty,
		Currency = Data.Currency,
		Amount = Data.Amount
	};


	public StreamEventVm CreateVm() => new StreamEventVm
	{
		EventType = StreamEventType.Kofi,
		Timestamp = Data.Timestamp.ToString("yyyy-MM-dd HH:mm"),
		Message = $"{Data.FromName} tipped {Data.Amount} via Kofi"
	};
}