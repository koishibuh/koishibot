using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;
public class BitsBadgeTier
{
    /// <summary>
    /// The tier of the Bits badge the user just earned. For example, 100, 1000, or 10000.
    /// </summary>
    [JsonPropertyName("charity_name")]
    public int Tier { get; set; }
}
