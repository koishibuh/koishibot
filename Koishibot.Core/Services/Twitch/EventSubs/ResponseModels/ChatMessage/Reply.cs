using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

public class Reply
{
    ///<summary>
    ///An ID that uniquely identifies the parent message that this message is replying to.
    ///</summary>
    [JsonPropertyName("parent_message_id")]
    public string ParentMessageId { get; set; }

    ///<summary>
    ///The message body of the parent message.
    ///</summary>
    [JsonPropertyName("parent_message_body")]
    public string ParentMessageBody { get; set; }

    ///<summary>
    ///User ID of the sender of the parent message.
    ///</summary>
    [JsonPropertyName("parent_user_id")]
    public string ParentUserId { get; set; }

    ///<summary>
    ///User name of the sender of the parent message.
    ///</summary>
    [JsonPropertyName("parent_user_name")]
    public string ParentUserName { get; set; }

    ///<summary>
    ///User login of the sender of the parent message.
    ///</summary>
    [JsonPropertyName("parent_user_login")]
    public string ParentUserLogin { get; set; }

    ///<summary>
    ///An ID that identifies the parent message of the reply thread.
    ///</summary>
    [JsonPropertyName("thread_message_id")]
    public string ThreadMessageId { get; set; }

    ///<summary>
    ///User ID of the sender of the thread’s parent message.
    ///</summary>
    [JsonPropertyName("thread_user_id")]
    public string ThreadUserId { get; set; }

    ///<summary>
    ///User name of the sender of the thread’s parent message.
    ///</summary>
    [JsonPropertyName("thread_user_name")]
    public string ThreadUserName { get; set; }

    ///<summary>
    ///User login of the sender of the thread’s parent message.
    ///</summary>
    [JsonPropertyName("thread_user_login")]
    public string ThreadUserLogin { get; set; }
}
