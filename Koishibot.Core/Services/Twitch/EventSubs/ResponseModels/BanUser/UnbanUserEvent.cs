using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.BanUser;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-unban-event">Twitch Documentation</see><br/>
/// When a viewer is unbanned from the specified channel.<br/>
/// Required Scopes: channel:moderate
/// </summary>
public class UnbanUserEvent
{
    /// <summary>
    /// The user id for the user who was unbanned on the specified channel.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UnbannedUserId { get; set; }

    /// <summary>
    /// The user login for the user who was unbanned on the specified channel. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string UnbannedUserLogin { get; set; }

    /// <summary>
    /// The user display name for the user who was unbanned on the specified channel.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UnbannedUserName { get; set; }

    /// <summary>
    ///	The broadcaster’s user ID.
    /// </summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    /// <summary>
    /// The broadcaster’s user login. (Lowercase)
    /// </summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    /// <summary>
    /// The broadcaster’s user display name.
    /// </summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    /// <summary>
    /// The user ID of the issuer of the unban.
    /// </summary>
    [JsonPropertyName("moderator_user_id")]
    public string ModeratorUserId { get; set; }

    /// <summary>
    /// The user login of the issuer of the unban. (Lowercase)
    /// </summary>
    [JsonPropertyName("moderator_user_login")]
    public string ModeratorUserLogin { get; set; }

    /// <summary>
    /// The user name of the issuer of the unban.
    /// </summary>
    [JsonPropertyName("moderator_user_name")]
    public string ModeratorUserName { get; set; }
}