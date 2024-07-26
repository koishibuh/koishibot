using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints.Enum;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

public class AutoRewardInfo
{
    /// <summary>
    /// The type of reward. 
    /// </summary>
    [JsonPropertyName("type")]
    public RewardType Type { get; set; }

    /// <summary>
    /// The reward cost.
    /// </summary>
    [JsonPropertyName("cost")]
    public int Cost { get; set; }

    /// <summary>
    /// Optional. Emote that was unlocked.
    /// </summary>
    [JsonPropertyName("unlocked_emote")]
    public UnlockedEmote? UnlockedEmote { get; set; }
}
