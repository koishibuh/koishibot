using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#emotes">Twitch Documentation</see><br/>
/// </summary>
public class Emote
{
    /// <summary>
    /// The emote ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The index of where the Emote starts in the text.
    /// </summary>
    [JsonPropertyName("begin")]
    public int Begin { get; set; }

    /// <summary>
    /// The index of where the Emote ends in the text.
    /// </summary>
    [JsonPropertyName("end")]
    public int End { get; set; }
}