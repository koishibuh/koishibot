using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

public class Message
{
    ///<summary>
    ///The contents of the message caught by automod.
    ///</summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    ///<summary>
    ///Metadata surrounding the potential inappropriate fragments of the message.
    ///</summary>
    [JsonPropertyName("fragments")]
    public List<Fragments> Fragments { get; set; }
}
