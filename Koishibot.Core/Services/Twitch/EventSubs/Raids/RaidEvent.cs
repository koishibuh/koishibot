namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Raids;

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
    public string FromBroadcasterId { get; set; }

    /// <summary>
    /// The broadcaster login that created the raid. (Lowercase)
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_login")]
    public string FromBroadcasterLogin { get; set; }

    /// <summary>
    /// The broadcaster display name that created the raid.
    /// </summary>
    [JsonPropertyName("from_broadcaster_user_name")]
    public string FromBroadcasterName { get; set; }

    /// <summary>
    /// The broadcaster ID that received the raid.
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_id")]
    public string ToBroadcasterId { get; set; }

    /// <summary>
    /// The broadcaster login that received the raid. (Lowercase)
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_login")]
    public string ToBroadcasterLogin { get; set; }

    /// <summary>
    /// The broadcaster display name that received the raid.
    /// </summary>
    [JsonPropertyName("to_broadcaster_user_name")]
    public string ToBroadcasterName { get; set; }

    /// <summary>
    /// The number of viewers in the raid.
    /// </summary>
    [JsonPropertyName("viewers")]
    public int ViewerCount { get; set; }
}
