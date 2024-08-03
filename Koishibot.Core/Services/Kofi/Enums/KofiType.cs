namespace Koishibot.Core.Services.Kofi.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum KofiType
{
	Donation,
	Subscription,
	Commission,
	ShopOrder
}