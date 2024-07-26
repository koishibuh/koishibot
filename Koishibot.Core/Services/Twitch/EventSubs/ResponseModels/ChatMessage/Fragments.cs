using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

public class Fragments
{
    ///<summary>
    ///The type of message fragment.
    ///</summary>
    [JsonPropertyName("type")]
    public FragmentType Type { get; set; }

    ///<summary>
    ///Message text in fragment.
    ///</summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    ///<summary>
    ///Optional. Metadata pertaining to the cheermote.
    ///</summary>
    [JsonPropertyName("cheermote")]
    public Cheermote? Cheermote { get; set; }

    ///<summary>
    ///Optional. Metadata pertaining to the emote.
    ///</summary>
    [JsonPropertyName("emote")]
    public Emote? Emote { get; set; }

    ///<summary>
    ///Optional. Metadata pertaining to the mention.
    ///</summary>
    [JsonPropertyName("mention")]
    public Mention? Mention { get; set; }
}
