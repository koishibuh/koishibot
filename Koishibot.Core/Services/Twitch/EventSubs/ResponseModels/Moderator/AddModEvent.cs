using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderator;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-moderator-add-event">Twitch Documentation</see><br/>
/// When a user is given moderator privileges on a specified channel.<br/>
/// Required Scopes: moderation:read
/// </summary>
public class AddModEvent
{
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
    /// The user ID of the new moderator.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string NewModUserId { get; set; } = string.Empty;

    /// <summary>
    /// The user login of the new moderator. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string NewModUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the new moderator.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string NewModUserName { get; set; } = string.Empty;
}
