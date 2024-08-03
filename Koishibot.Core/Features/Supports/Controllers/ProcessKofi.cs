using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Extensions;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Kofi;
using System.Text.Json;
namespace Koishibot.Core.Features.Supports.Controllers;


// == ⚫ POST == //

public class ProcessKofiController : ApiControllerBase
{
	private readonly IOptions<Settings> _settings;

	public ProcessKofiController(IOptions<Settings> Settings)
	{
		_settings = Settings;
	}

	[SwaggerOperation(Tags = ["Kofi"])]
	[HttpPost("/api/kofi")]
	public async Task<ActionResult> ProcessKofi([FromForm] string data)
	{
		var kofiData = JsonSerializer.Deserialize<KofiEvent>(data)
			?? throw new Exception("KofiData is null");

		if (kofiData.VerificationToken != _settings.Value.KofiVerificationToken)
		{
			return new BadRequestObjectResult(new { error = "Invalid verification token" });
		}

		await Mediator.Send(new ProcessKofiCommand { Data = kofiData });
		return Ok();
	}
}

// == ⚫ HANDLER == //

public record ProcessKofiHandler(
	KoishibotDbContext Database,
	ISignalrService Signalr
		) : IRequestHandler<ProcessKofiCommand>
{
	public async Task Handle
			(ProcessKofiCommand command, CancellationToken cancel)
	{
		var userLogin = command.GetUserlogin();
		var user = await Database.GetUserByLogin(userLogin);

		var kofi = command.CreateModel(user);
		await Database.UpdateEntry(kofi);

		var kofiVm = command.CreateVm();
		await Signalr.SendStreamEvent(kofiVm);
		
		// Update overlay bar
	}
}

// == ⚫ COMMAND == //

public class ProcessKofiCommand : IRequest
{
	public KofiEvent Data { get; set; }

	public string GetUserlogin()
	{
		return Data.FromName.ToLower();
	}

	public Kofi CreateModel(TwitchUser? user)
	{
		return new Kofi
		{
			KofiTransactionId = Data.KofiTransactionId,
			Timestamp = Data.Timestamp,
			TransactionUrl = Data.Url,
			KofiType = Data.Type,
			UserId = user is not null ? user.Id : null,
			Username = Data.FromName,
			Message = Data.Message ?? string.Empty,
			Currency = Data.Currency,
			Amount = Data.Amount
		};
	}

	public StreamEventVm CreateVm()
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Kofi,
			Timestamp = Data.Timestamp.ToString("yyyy-MM-dd HH:mm"),
			Message = $"{Data.FromName} tipped {Data.Amount} via Kofi"
		};
	}
}