using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Raid;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelraid">Twitch Documentation</see><br/>
/// When a broadcaster raids another broadcaster’s channel.<br/>
/// Required Scopes: No authorization required.
/// </summary>
public class RaidEvent
{
    /// <summary>
    /// The broadcaster ID that created the raid.
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_id")]
    public string FromBroadcasterUserId { get; set; }

    /// <summary>
    /// The broadcaster login that created the raid. (Lowercase)
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_login")]
    public string FromBroadcasterUserLogin { get; set; }

    /// <summary>
    /// The broadcaster display name that created the raid.
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_name")]
    public string FromBroadcasterUserName { get; set; }

    /// <summary>
    /// The broadcaster ID that received the raid.
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_id")]
    public string ToBroadcasterUserId { get; set; }

    /// <summary>
    /// The broadcaster login that received the raid. (Lowercase)
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_login")]
    public string ToBroadcasterUserLogin { get; set; }

    /// <summary>
    /// The broadcaster display name that received the raid.
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_name")]
    public string ToBroadcasterUserName { get; set; }

    /// <summary>
    /// The number of viewers in the raid.
    /// </summary>
    [JsonPropertyName("viewers")]
    public int Viewers { get; set; }
}
