using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#global-cooldown">Twitch Documentation</see>
/// </summary>
public class GlobalCooldown
{
    ///<summary>
    ///Is the setting enabled.
    ///</summary>
    [JsonPropertyName("is_enabled")]
    public bool IsEnabled { get; set; }

    ///<summary>
    ///The cooldown in seconds.
    ///</summary>
    [JsonPropertyName("seconds")]
    public int Seconds { get; set; }
}

