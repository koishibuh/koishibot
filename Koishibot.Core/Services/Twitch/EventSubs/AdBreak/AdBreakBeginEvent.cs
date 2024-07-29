using Koishibot.Core.Services.Twitch.Converters;
namespace Koishibot.Core.Services.Twitch.EventSubs.AdBreak;


/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelad_breakbegin">Twitch Documentation</see><br/>
/// When a user runs a midroll commercial break, either manually or automatically via ads manager.<br/>
/// Required Scopes: channel:read:ads
/// </summary>
public class AdBreakBeginEvent
{
    /// <summary>
    /// Length in seconds of the mid-roll ad break requested.
    /// </summary>
    [JsonPropertyName("duration_seconds")]
    [JsonConverter(typeof(TimeSpanSecondsConverter))]
    public TimeSpan DurationInSeconds { get; set; }

    /// <summary>
    /// The timestamp of when the ad break began<br/>
    /// (RFC3339 format converted to DateTimeOffset)<br/>
    /// There is potential delay between this event, when the streamer requested the ad break, and when the viewers will see ads.
    /// </summary>
    [JsonPropertyName("started_at")]
    [JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Indicates if the ad was automatically scheduled via Ads Manager.
    /// </summary>
    [JsonPropertyName("is_automatic")]
    public bool IsAutomatic { get; set; }

    /// <summary>
    /// The broadcaster’s user ID for the channel the ad was run on.
    /// </summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterId { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user login for the channel the ad was run on. (Lowercase)
    /// </summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterLogin { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user display name for the channel the ad was run on.
    /// </summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterName { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the user that requested the ad. For automatic ads, this will be the ID of the broadcaster.
    /// </summary>
    [JsonPropertyName("requester_user_id")]
    public string RequesterId { get; set; } = string.Empty;

    /// <summary>
    /// The login of the user that requested the ad. (Lowercase)
    /// </summary>
    [JsonPropertyName("requester_user_login")]
    public string RequesterLogin { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the user that requested the ad.
    /// </summary>
    [JsonPropertyName("requester_user_name")]
    public string RequesterName { get; set; } = string.Empty;
}