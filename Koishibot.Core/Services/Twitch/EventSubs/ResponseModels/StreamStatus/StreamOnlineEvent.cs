using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.StreamStatus;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamonline">Twitch Documentation</see><br/>
/// When the specified broadcaster starts a stream.<br/>
/// Required Scopes: No authorization required.
/// </summary>
public class StreamOnlineEvent
{
    /// <summary>
    /// The id of the stream.
    /// </summary>
    [JsonPropertyName("id")]
    public string StreamId { get; set; } = string.Empty;

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
    /// The stream type.
    /// </summary>
    [JsonPropertyName("type")]
    public StreamType Type { get; set; }

    /// <summary>
    /// The timestamp at which the stream went online at.<br/>
    /// (Converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }

}