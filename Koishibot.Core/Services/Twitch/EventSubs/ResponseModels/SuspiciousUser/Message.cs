using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser;
public class Message
{
    ///<summary>
    ///The UUID that identifies the message.
    ///</summary>
    [JsonPropertyName("message_id")]
    public string MessageId { get; set; }

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
