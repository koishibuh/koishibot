using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class Message
{
    /// <summary>
    /// The chat message in plain text.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    /// <summary>
    /// Ordered list of chat message fragments.
    /// </summary>
    [JsonPropertyName("fragments")]
    public List<Fragments> Fragments { get; set; }

    /// <summary>
    /// Optional. Metadata pertaining to the cheermote.
    /// </summary>
    [JsonPropertyName("cheermote")]
    public Cheermote Cheermote { get; set; }

    /// <summary>
    /// Optional. Metadata pertaining to the emote.
    /// </summary>
    [JsonPropertyName("emote")]
    public Emote Emote { get; set; }

    /// <summary>
    /// Optional. Metadata pertaining to the mention.
    /// </summary>
    [JsonPropertyName("mention")]
    public Mention Mention { get; set; }
}