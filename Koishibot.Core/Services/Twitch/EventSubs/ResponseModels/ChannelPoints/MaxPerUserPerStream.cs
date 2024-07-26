using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#max-per-user-per-stream">Twitch Documentation</see>
/// </summary>
public class MaxPerUserPerStream
{
    ///<summary>
    ///Is the setting enabled.
    ///</summary>
    [JsonPropertyName("is_enabled")]
    public bool IsEnabled { get; set; }

    ///<summary>
    ///The max per user per stream limit.
    ///</summary>
    [JsonPropertyName("value")]
    public int Value { get; set; }

}
