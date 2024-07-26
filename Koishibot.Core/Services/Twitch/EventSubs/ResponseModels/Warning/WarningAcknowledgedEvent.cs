using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Warning;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningacknowledge">Twitch Documentation</see><br/>
/// When a warning is acknowledged by a user.<br/>
/// Broadcasters and moderators can see the warning’s details.<br/>
/// Required Scopes: moderator:read:warnings or moderator:manage:warnings
/// </summary>
public class WarningAcknowledgedEvent
{
    ///<summary>
    ///The user ID of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The login of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The ID of the user that has acknowledged their warning.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The login of the user that has acknowledged their warning.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user name of the user that has acknowledged their warning.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
}