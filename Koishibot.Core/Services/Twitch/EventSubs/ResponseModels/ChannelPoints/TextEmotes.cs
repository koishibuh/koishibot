using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#message">Twitch Documentation</see><br/>
/// </summary>
public class TextEmotes
{
    /// <summary>
    /// The text of the resubscription chat message.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    /// <summary>
    /// An array that includes the emote ID and start and end positions for where the emote appears in the text.
    /// </summary>
    [JsonPropertyName("emotes")]
    public List<Emote> Emotes { get; set; }
}