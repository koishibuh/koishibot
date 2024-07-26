using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.GuestStar;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_sessionend">Twitch Documentation</see><br/>
/// When a running Guest Star session is ended by the host, or automatically by the system.<br/>
/// Required Scopes: channel:read:guest_star, channel:manage:guest_star, moderator:read:guest_star or moderator:manage:guest_star<br/>
/// </summary>
public class GuestStarSessionEndedEvent
{
    ///<summary>
    ///The non-host broadcaster user ID.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The non-host broadcaster display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The non-host broadcaster login.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///ID representing the unique session that was started.
    ///</summary>
    [JsonPropertyName("session_id")]
    public string SessionId { get; set; }

    ///<summary>
    /// The timestamp indicating the time the session began.<br/>
    /// (RFC3339 converted to DateTimeOffset)
    ///</summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }

    ///<summary>
    ///The timestamp indicating the time the session ended.<br/>
    ///(RFC3339 converted to DateTimeOffset)
    ///</summary>
    [JsonPropertyName("ended_at")]
    public DateTimeOffset EndedAt { get; set; }

    ///<summary>
    ///User ID of the host channel.
    ///</summary>
    [JsonPropertyName("host_user_id")]
    public string HostUserId { get; set; }

    ///<summary>
    ///The host display name.
    ///</summary>
    [JsonPropertyName("host_user_name")]
    public string HostUserName { get; set; }

    ///<summary>
    ///The host login.
    ///</summary>
    [JsonPropertyName("host_user_login")]
    public string HostUserLogin { get; set; }
}