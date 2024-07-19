using Newtonsoft.Json;

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
		var kofiData = JsonConvert.DeserializeObject<Data>(data)
			?? throw new Exception("KofiData is null");

		if (kofiData.VerificationToken != _settings.Value.KofiVerificationToken)
		{
			return new BadRequestObjectResult(new { error = "Invalid verification token"});
		}

		await Mediator.Send(new ProcessKofiCommand { Data = kofiData });
		return Ok();
	}
}

// == ⚫ COMMAND == //

public class ProcessKofiCommand : IRequest
{
	public Data Data { get; set; }
}

public class Data
{
	public string VerificationToken { get; set; } = string.Empty;
	public string MessageId { get; set; } = string.Empty;
	public string Timestamp { get; set; } = string.Empty;
	public KofiType Type { get; set; }
	public bool IsPublic { get; set; }
	public string FromName { get; set; } = string.Empty;
	public string? Message { get; set; }
	public string Amount { get; set; } = string.Empty;
	public string Url { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Currency { get; set; } = string.Empty;
	public bool IsSubscriptionPayment { get; set; }
	public bool IsFirstSubscriptionPayment { get; set; }
	public string KofiTransactionId { get; set; } = string.Empty;
	public List<ShopItem>? ShopItems { get; set; }
	public string? TierName { get; set; }
	public ShippingInfo? Shipping { get; set; }
}

public class ShopItem
{
	public string DirectLinkCode { get; set; } = string.Empty;
	public string VariationName { get; set; } = string.Empty;
	public int Quantity { get; set; }

}

public class ShippingInfo
{
	public string FullName { get; set; } = string.Empty;
	public string StreetAddress { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string StateOrProvince { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string CountryCode { get; set; } = string.Empty;
	public string Telephone { get; set; } = string.Empty;
}

public enum KofiType
{
	Donation,
	Subscription,
	Commission,
	ShopOrder
}

// == ⚫ HANDLER == //

public record ProcessKofiHandler(

		) : IRequestHandler<ProcessKofiCommand>
{
	public async Task Handle
			(ProcessKofiCommand command, CancellationToken cancel)
	{
		// save to database
		// update overlay
		var test = command.Data.Timestamp;
	}
}