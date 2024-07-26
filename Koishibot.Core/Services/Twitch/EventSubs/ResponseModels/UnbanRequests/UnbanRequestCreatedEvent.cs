using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.UnbanRequests;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestcreate">Twitch Documentation</see><br/>
/// When a user creates an unban request.<br/>
/// Required Scopes: moderator:read:unban_requests or moderator:manage:unban_requests<br/>
/// </summary>
public class UnbanRequestCreatedEvent
{
    ///<summary>
    ///The ID of the unban request.
    ///</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    ///<summary>
    ///The broadcaster’s user ID for the channel the unban request was created for.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The broadcaster’s login name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The broadcaster’s display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///User ID of user that is requesting to be unbanned.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The user’s login name.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user’s display name.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///Message sent in the unban request.
    ///</summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    ///<summary>
    ///The timestamp of when the unban request was created.</br>
    ///(RFC3339 converted to DateTimeOffset)
    ///</summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}