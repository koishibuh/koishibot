using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderator;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderatorremove">Twitch Documentation</see><br/>
/// When a user has moderator privileges removed on a specified channel.<br/>
/// Required Scopes: moderation:read
/// </summary>
public class RemoveModEvent
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
    public string BroadcasterUsername { get; set; } = string.Empty;

    /// <summary>
    /// The user ID of the removed moderator.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string RemovedModUserId { get; set; } = string.Empty;

    /// <summary>
    /// The user login of the removed moderator. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string RemovedModUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the removed moderator.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string RemovedModUserName { get; set; } = string.Empty;
}
