using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodmessageupdate">Twitch Documentation</see><br/>
/// When a message in the automod queue has its status changed.<br/>
/// Required Scopes: moderator:manage:automod<br/>
/// </summary>
public class AutoModMessageUpdate
{
    ///<summary>
    ///The ID of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The login of the broadcaster specified in the request. (Lowercase)
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The message sender’s user ID.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string ViewerUserId { get; set; }

    ///<summary>
    ///The message sender’s login name. (Lowercase)
    ///</summary>
    [JsonPropertyName("user_login")]
    public string ViewerUserLogin { get; set; }

    ///<summary>
    ///The message sender’s display name.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string ViewerUserName { get; set; }

    ///<summary>
    ///The ID of the moderator.
    ///</summary>
    [JsonPropertyName("moderator_user_id")]
    public string ModeratorUserId { get; set; }

    ///<summary>
    ///TThe moderator’s user name.
    ///</summary>
    [JsonPropertyName("moderator_user_name")]
    public string ModeratorUserName { get; set; }

    ///<summary>
    ///The login of the moderator.
    ///</summary>
    [JsonPropertyName("moderator_user_login")]
    public string ModeratorUserLogin { get; set; }

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

    ///<summary>
    ///The category of the message.
    ///</summary>
    [JsonPropertyName("category")]
    public string Category { get; set; }

    ///<summary>
    ///The level of severity. Measured between 1 to 4.
    ///</summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }

    ///<summary>
    ///The message’s status.
    ///</summary>
    [JsonPropertyName("status")]
    public AutomodMessageStatus Status { get; set; }

    ///<summary>
    ///The timestamp of when automod saved the message.<br/>
    ///(Converted to DateTimeOffset)
    ///</summary>
    [JsonPropertyName("held_at")]
    public DateTimeOffset HeldAt { get; set; }
}