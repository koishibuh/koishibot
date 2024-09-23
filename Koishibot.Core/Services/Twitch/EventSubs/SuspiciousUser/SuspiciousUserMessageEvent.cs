using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsuspicious_usermessage">Twitch Documentation</see><br/>
/// When a chat message has been sent from a suspicious user.<br/>
/// Required Scopes: moderator:read:suspicious_users<br/>
/// </summary>
public class SuspiciousUserMessageEvent
{
    ///<summary>
    ///The ID of the channel where the treatment for a suspicious user was updated.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterId { get; set; }

    ///<summary>
    ///The display name of the channel where the treatment for a suspicious user was updated.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterName { get; set; }

    ///<summary>
    ///The login of the channel where the treatment for a suspicious user was updated.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterLogin { get; set; }

    ///<summary>
    ///The user ID of the user that sent the message.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The user name of the user that sent the message.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///The user login of the user that sent the message.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The status set for the suspicious user. Can be the following: “none”, “active_monitoring”, or “restricted”
    ///</summary>
    [JsonPropertyName("low_trust_status")]
    public LowTrustStatus LowTrustStatus { get; set; }

    ///<summary>
    ///A list of channel IDs where the suspicious user is also banned.
    ///</summary>
    [JsonPropertyName("shared_ban_channel_ids")]
    public List<string> SharedBanChannelIds { get; set; }

    ///<summary>
    ///User types (if any) that apply to the suspicious user.
    ///</summary>
    [JsonPropertyName("types")]
    public List<string> Types { get; set; }

    ///<summary>
    ///A ban evasion likelihood value (if any) that as been applied to the user automatically by Twitch, can be “unknown”, “possible”, or “likely”.
    ///</summary>
    [JsonPropertyName("ban_evasion_evaluation")]
    public string BanEvasionLikelyhood { get; set; }

    ///<summary>
    ///The structured chat message.
    ///</summary>
    [JsonPropertyName("message")]
    public Message Message { get; set; }
}