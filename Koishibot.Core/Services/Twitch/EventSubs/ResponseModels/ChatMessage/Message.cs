using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

public class Message
{
    ///<summary>
    ///The chat message in plain text.
    ///</summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    ///<summary>
    ///Ordered list of chat message fragments.
    ///</summary>
    [JsonPropertyName("fragments")]
    public List<Fragments> Fragments { get; set; }
}