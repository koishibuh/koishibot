namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage_delete">Twitch Documentation</see><br/>
/// When a moderator removes a specific message.<br/>
/// Required Scopes: user:read:chat<br/>
/// </summary>
public class MessageDeletedEvent
{
    ///<summary>
    ///The broadcaster user ID.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string? BroadcasterId { get; set; }

    ///<summary>
    ///The broadcaster display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string? BroadcasterName { get; set; }

    ///<summary>
    ///The broadcaster login. (Lowercase)
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string? BroadcasterLogin { get; set; }

    ///<summary>
    ///The ID of the user whose message was deleted.
    ///</summary>
    [JsonPropertyName("target_user_id")]
    public string? TargetUserId { get; set; }

    ///<summary>
    ///The user name of the user whose message was deleted.
    ///</summary>
    [JsonPropertyName("target_user_name")]
    public string? TargetUserName { get; set; }

    ///<summary>
    ///The user login of the user whose message was deleted. (Lowercase)
    ///</summary>
    [JsonPropertyName("target_user_login")]
    public string? TargetUserLogin { get; set; }

    ///<summary>
    ///A UUID that identifies the message that was removed.
    ///</summary>
    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }
}