using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class CommunitySubGift
{
	/// <summary>
	/// The ID of the associated community gift.
	/// </summary>
	[JsonPropertyName("id")]
	public string? Id { get; set; }

	/// <summary>
	/// Number of subscriptions being gifted.
	/// </summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }

	/// <summary>
	/// "The type of subscription plan being used.
	/// </summary>
	[JsonPropertyName("sub_tier")]
	public SubTier SubTier { get; set; }

	/// <summary>
	/// Optional. The amount of gifts the gifter has given in this channel. Null if anonymous.
	/// </summary>
	[JsonPropertyName("cumulative_total")]
	public int? CumulativeTotal { get; set; }
}
