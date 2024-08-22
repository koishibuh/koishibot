namespace Koishibot.Core.Services.StreamElements.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventType
{
	Subscriber,
	CommunityGiftPurchase,
	Tip,
	Follow,
	Cheer,
	Raid,
	Merch,
	CharityCampaignDonation,
	Redemption //Items
	// Emotes doesn't trigger?
}