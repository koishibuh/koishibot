using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

public class Fragments
{
    ///<summary>
    ///Message text in a fragment.
    ///</summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    ///<summary>
    ///Optional. Metadata pertaining to the emote.
    ///</summary>
    [JsonPropertyName("emote")]
    public Emote? Emote { get; set; }

    ///<summary>
    ///Optional. Metadata pertaining to the cheermote.
    ///</summary>
    [JsonPropertyName("cheermote")]
    public Cheermote? Cheermote { get; set; }
}