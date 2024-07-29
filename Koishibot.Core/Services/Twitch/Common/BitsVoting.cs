namespace Koishibot.Core.Services.Twitch.Common;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#bits-voting">Twitch Documentation</see><br/>
/// Bits voting is not supported.<br/>
/// </summary>
public class BitsVoting
{
    /// <summary>
    /// Not used; will be set to false
    /// </summary>
    [JsonPropertyName("is_enabled")]
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Not used; will be set to 0.
    /// </summary>
    [JsonPropertyName("amount_per_vote")]
    public int AmountPerVote { get; set; }
}