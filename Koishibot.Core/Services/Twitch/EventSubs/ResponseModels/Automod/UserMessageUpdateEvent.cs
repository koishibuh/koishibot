using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatuser_message_update">Twitch Documentation</see><br/>
/// When a user's message automod status is updated.<br/>
/// Required Scopes: user:read:chat scope<br/>
/// </summary>
public class UserMessageUpdateEvent
{
    ///<summary>
    ///The ID of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The login of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The User ID of the message sender.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The message sender’s login.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The message sender’s user name.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///The message’s status. Possible values are:approved
    //denied
    //invalid
    ///</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    ///<summary>
    ///The ID of the message that was flagged by automod.
    ///</summary>
    [JsonPropertyName("message_id")]
    public string MessageId { get; set; }

    ///<summary>
    ///The body of the message.
    ///</summary>
    [JsonPropertyName("message")]
    public List<Message> Message { get; set; }
}