using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.BanUser;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-ban-event">Twitch Documentation</see><br/>
/// When a viewer is timed out or banned from the specified channel.<br/>
/// Required Scopes: channel:moderate
/// </summary>
public class DisciplinedUserEvent
{
    /// <summary>
    /// The user ID for the user who was banned on the specified channel.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string DisciplinedUserId { get; set; }

    /// <summary>
    /// The user login for the user who was banned on the specified channel. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string DisciplinedUserLogin { get; set; }

    /// <summary>
    /// The user display name for the user who was banned on the specified channel.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string DisciplinedUserName { get; set; }

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
    /// The user ID of the issuer of the ban/timeout.
    /// </summary>
    [JsonPropertyName("moderator_user_id")]
    public string ModeratorUserId { get; set; }

    /// <summary>
    /// The user login of the issuer of the ban/timeout. (Lowercase)
    /// </summary>
    [JsonPropertyName("moderator_user_login")]
    public string ModeratorUserLogin { get; set; }

    /// <summary>
    /// The user name of the issuer of the ban/timeout.
    /// </summary>
    [JsonPropertyName("moderator_user_name")]
    public string ModeratorUserName { get; set; }

    /// <summary>
    /// The reason behind the ban.
    /// </summary>
    [JsonPropertyName("reason")]
    public string Reason { get; set; }

    /// <summary>
    /// The timestamp of when the user was banned or timed out<br/>
    /// (RFC3339 format converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("banned_at")]
    public DateTimeOffset BannedAt { get; set; }

    /// <summary>
    /// The timestamp of when the timeout ends or null if the user was banned instead of timed out.
    /// (RFC3339 format converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("ends_at")]
    public DateTimeOffset? EndsAt { get; set; }

    /// <summary>
    /// Indicates whether the ban is permanent (true) or a timeout (false).<br/>
    /// If true, ends_at will be null.
    /// </summary>
    [JsonPropertyName("is_permanent")]
    public bool IsPermanent { get; set; }

}