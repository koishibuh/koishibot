using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-points-voting">Twitch Documentation</see><br/>
/// </summary>
public class ChannelPointsVoting
{
    /// <summary>
    /// Indicates if Channel Points can be used for voting.
    /// </summary>
    [JsonPropertyName("is_enabled")]
    public bool IsEnabled { get; set; }


    /// <summary>
    /// Number of Channel Points required to vote once with Channel Points.
    /// </summary>
    [JsonPropertyName("amount_per_vote")]
    public int AmountPerVote { get; set; }
}