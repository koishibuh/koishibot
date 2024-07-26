using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#max-per-stream">Twitch Documentation</see>
/// </summary>
public class MaxPerStream
{
    ///<summary>
    ///Is the setting enabled.
    ///</summary>
    [JsonPropertyName("is_enabled")]
    public bool IsEnabled { get; set; }

    ///<summary>
    ///The max per stream limit.
    ///</summary>
    [JsonPropertyName("value")]
    public int Value { get; set; }
}
