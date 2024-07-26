using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Follow;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelfollow">Twitch Documentation</see><br/>
/// When a specified channel receives a follow.<br/>
/// Required Scopes: moderator:read:followers
/// </summary>
public class FollowEvent
{
    /// <summary>
    /// The user ID for the user now following the specified channel.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string FollowerUserId { get; set; } = string.Empty;

    /// <summary>
    /// The user login for the user now following the specified channel. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string FollowerUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The user display name for the user now following the specified channel.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string FollowerUserName { get; set; } = string.Empty;

    /// <summary>
    ///	The broadcaster’s user ID.
    /// </summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user login. (Lowercase)
    /// </summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user display name.
    /// </summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp of when the follow occurred<br/>
    /// (RFC3339 converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("followed_at")]
    public DateTimeOffset FollowedAt { get; set; }
}