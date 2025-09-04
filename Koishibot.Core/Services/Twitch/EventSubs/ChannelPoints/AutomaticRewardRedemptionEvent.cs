using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-points-automatic-reward-redemption-add-v2-event">Twitch Documentation</see><br/>
/// When a viewer has redeemed an automatic channel points reward on the specified channel.<br/>
/// Required Scopes: channel:read:redemptions OR channel:manage:redemptions
/// </summary>
public class AutomaticRewardRedemptionEvent
{
	/// <summary>
	///	The ID of the channel where the reward was redeemed.
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	/// <summary>
	/// The login of the channel where the reward was redeemed. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The display name of the channel where the reward was redeemed.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	/// <summary>
	/// The ID of the redeeming user.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string RedeemedById { get; set; } = string.Empty;

	/// <summary>
	/// The user login of the removed moderator. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string RedeemedByLogin { get; set; } = string.Empty;

	/// <summary>
	/// The display name of the removed moderator.
	/// </summary>
	[JsonPropertyName("user_name")]
	public string RedeemedByUsername { get; set; } = string.Empty;

	/// <summary>
	/// The ID of the Redemption.
	/// </summary>
	[JsonPropertyName("id")]
	public string RedemptionId { get; set; } = string.Empty;

	/// <summary>
	/// An object that contains the reward information.
	/// </summary>
	[JsonPropertyName("reward")]
	public AutoRewardInfo? Reward { get; set; }

	/// <summary>
	/// An object that contains the user message and emote information needed to recreate the message.
	/// </summary>
	[JsonPropertyName("message")]
	public Message? Message { get; set; }
	
	/// <summary>
	/// The timestamp of when the reward was redeemed.<br/>
	/// (RFC3339 format converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("redeemed_at")]
	public DateTimeOffset RedeemedAt { get; set; }
}