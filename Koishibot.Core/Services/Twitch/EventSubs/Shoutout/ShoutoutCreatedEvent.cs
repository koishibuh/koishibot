using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Shoutout;


/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshoutoutcreate">Twitch Documentation</see><br/>
/// When the specified broadcaster sends a Shoutout<br/>
/// Required Scopes: moderator:read:shoutouts OR moderator:manage:shoutouts
/// </summary>
public class ShoutoutCreatedEvent
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
    [JsonPropertyName("to_broadcaster_user_id")]
    public string ShoutedoutBroadcasterId { get; set; } = string.Empty;

    /// <summary>
    /// The user login of the broadcaster that received the Shoutout. (Lowercase)
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_login")]
    public string ShoutedoutBroadcasterLogin { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the broadcaster that received the Shoutout.
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_name")]
    public string ShoutedoutBroadcasterName { get; set; } = string.Empty;

    /// <summary>
    /// The UserId of the moderator that sent the Shoutout. If the broadcaster sent the Shoutout, this ID is the same as the ID in broadcaster_user_id.
    /// </summary>
    [JsonPropertyName("moderator_user_id")]
    public string ModeratorUserId { get; set; } = string.Empty;

    /// <summary>
    /// The Userlogin of the moderator that sent the Shoutout. (Lowercase)
    /// </summary>
    [JsonPropertyName("moderator_user_login")]
    public string ModeratorUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The Username of the moderator that sent the Shoutout.
    /// </summary>
    [JsonPropertyName("moderator_user_name")]
    public string ModeratorUserName { get; set; } = string.Empty;

    /// <summary>
    /// The number of users that were watching the broadcaster’s stream at the time of the Shoutout.
    /// </summary>
    [JsonPropertyName("viewer_count")]
    public int ViewerCount { get; set; }

    /// <summary>
    /// The timestamp of when the moderator sent the Shoutout.<br/>
    /// (RFC3339 format converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// The timestamp of when the broadcaster may send a Shoutout to a different broadcaster.<br/>
    /// (RFC3339 format converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("cooldown_ends_at")]
    public DateTimeOffset CooldownEndsAt { get; set; }

    /// <summary>
    /// The timestampof when the broadcaster may send another Shoutout to the broadcaster in to_broadcaster_user_id.<br/>
    /// (RFC3339 format converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("target_cooldown_ends_at")]
    public DateTimeOffset TargetCooldownEndsAt { get; set; }
}

