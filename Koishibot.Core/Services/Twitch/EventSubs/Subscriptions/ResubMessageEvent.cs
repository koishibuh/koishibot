using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptionmessage">Twitch Documentation</see><br/>
/// When a user sends a resubscription chat message in a specific channel.<br/>
/// Required Scopes: channel:read:subscriptions
/// </summary>
public class ResubMessageEvent
{
	/// <summary>
	/// The user ID of the user who sent a resubscription chat message.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string ResubscriberId { get; set; } = string.Empty;

	/// <summary>
	/// The user login of the user who sent a resubscription chat message. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string ResubscriberLogin { get; set; } = string.Empty;

	/// <summary>
	/// The user display name of the user who a resubscription chat message.
	/// </summary>
	[JsonPropertyName("user_name")]
	public string ResubscriberName { get; set; } = string.Empty;

	/// <summary>
	///	The broadcaster’s user ID.
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user login. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user display name.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	/// <summary>
	/// The tier of the user’s subscription.
	/// </summary>
	[JsonPropertyName("tier")]
	public SubTier Tier { get; set; }

	/// <summary>
	/// An object that contains the resubscription message and emote information needed to recreate the message.
	/// </summary>
	[JsonPropertyName("message")]
	public TextEmotes? Message { get; set; }

	/// <summary>
	/// The total number of months the user has been subscribed to the channel.
	/// </summary>
	[JsonPropertyName("cumulative_months")]
	public int CumulativeMonths { get; set; }

	/// <summary>
	/// The number of consecutive months the user’s current subscription has been active. <br/>
	/// This value is null if the user has opted out of sharing this information.
	/// </summary>
	[JsonPropertyName("streak_months")]
	public int? StreakMonths { get; set; }

	/// <summary>
	/// How many months this subscription has been renewed for in advance.
	/// </summary>
	[JsonPropertyName("duration_months")]
	public int DurationMonths { get; set; }
}