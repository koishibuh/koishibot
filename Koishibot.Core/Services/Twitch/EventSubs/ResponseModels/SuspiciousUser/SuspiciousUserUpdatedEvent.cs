using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsuspicious_userupdate">Twitch Documentation</see><br/>
/// When a suspicious user has been updated.<br/>
/// Required Scopes: moderator:read:suspicious_users<br/>
/// </summary>
public class SuspiciousUserUpdatedEvent
{
    ///<summary>
    ///The ID of the channel where the treatment for a suspicious user was updated.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The display name of the channel where the treatment for a suspicious user was updated.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The Login of the channel where the treatment for a suspicious user was updated.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The ID of the moderator that updated the treatment for a suspicious user.
    ///</summary>
    [JsonPropertyName("moderator_user_id")]
    public string ModeratorUserId { get; set; }

    ///<summary>
    ///The display name of the moderator that updated the treatment for a suspicious user.
    ///</summary>
    [JsonPropertyName("moderator_user_name")]
    public string ModeratorUserName { get; set; }

    ///<summary>
    ///The login of the moderator that updated the treatment for a suspicious user.
    ///</summary>
    [JsonPropertyName("moderator_user_login")]
    public string ModeratorUserLogin { get; set; }

    ///<summary>
    ///The ID of the suspicious user whose treatment was updated.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The display name of the suspicious user whose treatment was updated.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///The login of the suspicious user whose treatment was updated.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The status set for the suspicious user. Can be the following: “none”, “active_monitoring”, or “restricted”.
    ///</summary>
    [JsonPropertyName("low_trust_status")]
    public LowTrustStatus LowTrustStatus { get; set; }
}