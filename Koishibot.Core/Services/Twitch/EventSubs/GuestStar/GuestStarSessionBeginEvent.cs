using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.GuestStar;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionbegin">Twitch Documentation</see><br/>
/// When the host begins a new Guest Star session.<br/>
/// Required Scopes: channel:read:guest_star, channel:manage:guest_star, moderator:read:guest_star or moderator:manage:guest_star<br/>
/// </summary>
public class GuestStarSessionBeginEvent
{
	///<summary>
	///The broadcaster user ID.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string? BroadcasterId { get; set; }

	///<summary>
	///The broadcaster display name.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string? BroadcasterName { get; set; }

	///<summary>
	///The broadcaster login.
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string? BroadcasterLogin { get; set; }

	///<summary>
	///ID representing the unique session that was started.
	///</summary>
	[JsonPropertyName("session_id")]
	public string? SessionId { get; set; }

	///<summary>
	/// The timestamp indicating the time the session began.
	/// (RFC3339 converted to DateTimeOffset
	/// </summary>
	[JsonPropertyName("started_at")]
	[JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset StartedAt { get; set; }
}