using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;
public class DeletedMessage
{
    ///<summary>
    ///The ID of the user whose message is being deleted.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The login of the user.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user name of the user.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///The ID of the message being deleted.
    ///</summary>
    [JsonPropertyName("message_id")]
    public string MessageId { get; set; }

    ///<summary>
    ///The message body of the message being deleted.
    ///</summary>
    [JsonPropertyName("message_body")]
    public string MessageBody { get; set; }
}
