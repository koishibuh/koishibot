using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Shoutout;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutreceive">Twitch Documentation</see><br/>
/// When the specified broadcaster receives a Shoutout - sent only if Twitch posts the Shoutout to the broadcaster’s activity feed.<br/>
/// Required Scopes: moderator:read:shoutouts OR moderator:manage:shoutouts
/// </summary>
public class ShoutoutReceivedEvent
{
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
    ///	The user ID of the broadcaster that received the Shoutout.
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_id")]
    public string ShoutedByBroadcasterId { get; set; } = string.Empty;

    /// <summary>
    /// The user login of the broadcaster that received the Shoutout.. (Lowercase)
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_login")]
    public string ShoutedByBroadcasterLogin { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the broadcaster that received the Shoutout..
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_name")]
    public string ShoutedByBroadcasterName { get; set; } = string.Empty;

    /// <summary>
    /// The number of users that were watching the broadcaster who sent the shoutout's stream at the time of the Shoutout.
    /// </summary>
    [JsonPropertyName("viewer_count")]
    public int ViewerCount { get; set; }

    /// <summary>
    /// Thetimestamp of when the moderator sent the Shoutout<br/>
    /// (RFC3339 format converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }
}

