using Koishibot.Core.Services.Kofi.Enums;
using Koishibot.Core.Services.Twitch.Converters;
namespace Koishibot.Core.Services.Kofi;


public class KofiEvent
{
	[JsonPropertyName("verification_token")]
	public string VerificationToken { get; set; } = string.Empty;

	[JsonPropertyName("message_id")]
	public string MessageId { get; set; } = string.Empty;

	[JsonPropertyName("timestamp")]
	public DateTimeOffset Timestamp { get; set; }

	[JsonPropertyName("type")]
	public KofiType Type { get; set; }

	[JsonPropertyName("is_public")]
	public bool IsPublic { get; set; }

	[JsonPropertyName("from_name")]
	public string FromName { get; set; } = string.Empty;

	[JsonPropertyName("message")]
	public string? Message { get; set; }

	[JsonPropertyName("amount")]
	public string Amount { get; set; } = string.Empty;

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;

	[JsonPropertyName("email")]
	public string Email { get; set; } = string.Empty;

	[JsonPropertyName("currency")]
	public string Currency { get; set; } = string.Empty;

	[JsonPropertyName("is_subscription_payment")]
	public bool IsSubscriptionPayment { get; set; }

	[JsonPropertyName("is_first_subscription_payment")]
	public bool IsFirstSubscriptionPayment { get; set; }

	[JsonPropertyName("kofi_transaction_id")]
	public string KofiTransactionId { get; set; } = string.Empty;

	[JsonPropertyName("shop_items")]
	public List<ShopItem>? ShopItems { get; set; }

	[JsonPropertyName("tier_name")]
	public string? TierName { get; set; }

	[JsonPropertyName("shipping")]
	public ShippingInfo? Shipping { get; set; }
}

public class ShopItem
{
	[JsonPropertyName("direct_link_code")]
	public string DirectLinkCode { get; set; } = string.Empty;

	[JsonPropertyName("variation_name")]
	public string VariationName { get; set; } = string.Empty;

	[JsonPropertyName("quantity")]
	public int Quantity { get; set; }

}

public class ShippingInfo
{
	[JsonPropertyName("full_name")]
	public string FullName { get; set; } = string.Empty;

	[JsonPropertyName("street_address")]
	public string StreetAddress { get; set; } = string.Empty;

	[JsonPropertyName("city")]
	public string City { get; set; } = string.Empty;

	[JsonPropertyName("state_or_province")]
	public string StateOrProvince { get; set; } = string.Empty;

	[JsonPropertyName("postal_code")]
	public string PostalCode { get; set; } = string.Empty;

	[JsonPropertyName("country")]
	public string Country { get; set; } = string.Empty;

	[JsonPropertyName("country_code")]
	public string CountryCode { get; set; } = string.Empty;

	[JsonPropertyName("telephone")]
	public string Telephone { get; set; } = string.Empty;
}