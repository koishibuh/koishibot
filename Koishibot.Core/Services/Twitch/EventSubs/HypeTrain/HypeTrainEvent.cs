using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainbegin">Twitch Documentation</see><br/>
/// When a Hype Train begins on the specified channel.<br/>
/// Required Scopes: channel:read:hype_train<br/><br/>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainend">Twitch Documentation</see><br/>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#hype-train-end-event">Reference</see><br/>
/// When a Hype Train ends on the specified channel. <br/>
/// Required Scopes: channel:read:hype_train
/// </summary>
public class HypeTrainEvent
{
	/// <summary>
	/// The Hype Train ID.
	/// </summary>
	[JsonPropertyName("id")]
	public string? Id { get; set; }

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
	public string BroadcasterUsername { get; set; } = string.Empty;

	/// <summary>
	/// The starting level of the Hype Train.
	/// </summary>
	[JsonPropertyName("level")]
	public int Level { get; set; }

	/// <summary>
	/// The number of points contributed to the Hype Train at the current level.
	/// </summary>
	[JsonPropertyName("progress")]
	public int Progress { get; set; }

	/// <summary>
	/// Total points contributed to the Hype Train.
	/// </summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }

	/// <summary>
	/// The number of points required to reach the next level.
	/// </summary>
	[JsonPropertyName("goal")]
	public int Goal { get; set; }

	/// <summary>
	/// The contributors with the most points contributed.
	/// </summary>
	[JsonPropertyName("top_contributions")]
	public List<Contributor>? TopContributions { get; set; }

	/// <summary>
	/// The most recent contribution.
	/// </summary>
	[JsonPropertyName("last_contribution")]
	public Contributor? LastContribution { get; set; }

	/// <summary>
	/// The time when the Hype Train started<br/>
	/// (Converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("started_at")]
	[JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset StartedAt { get; set; }

	/// <summary>
	/// The time when the Hype Train expires.<br/>
	/// (Converted to DateTimeOffset)<br/>
	/// The expiration is extended when the Hype Train reaches a new level.
	/// </summary>
	[JsonPropertyName("expires_at")]
	[JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset ExpiresAt { get; set; }

	/// <summary>
	/// The time when the Hype Train expires<br/>
	/// (Converted to DateTimeOffset) (Hypetrain Ended)<br/>
	/// The expiration is extended when the Hype Train reaches a new level.
	/// </summary>
	[JsonPropertyName("ended_at")]
	[JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset? EndedAt { get; set; }

	/// <summary>
	/// The time when the Hype Train cooldown ends so that the next Hype Train can start.<br/>
	/// (Converted to DateTimeOffset) (Hypetrain Ended)
	/// </summary>
	[JsonPropertyName("cooldown_ends_at")]
	[JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset? CooldownEndsAt { get; set; }
}



/// <summary>
/// <see href=" https://dev.twitch.tv/docs/eventsub/eventsub-reference/#top-contributions">Twitch Documentation</see><br/>
/// The top contributor for a contribution type. <br/>
/// For example, the top contributor using BITS (by aggregate) or the top contributor using subscriptions (by count).
/// </summary>
/// 
public class Contributor
{
	/// <summary>
	/// The ID of the user that made the contribution.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string? UserId { get; set; }

	/// <summary>
	/// The user's login name. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string? UserLogin { get; set; }

	/// <summary>
	/// The user's display name.
	/// </summary>
	[JsonPropertyName("user_name")]
	public string? UserName { get; set; }

	/// <summary>
	/// "The contribution method used.
	/// </summary>
	[JsonPropertyName("type")]
	public ContributionEventSubType Type { get; set; }
	/// <summary>
	/// The total amount contributed. If type is bits, total represents the amount of Bits used. If type is subscription, total is 500, 1000, or 2500 to represent tier 1, 2, or 3 subscriptions, respectively.
	/// </summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }
}