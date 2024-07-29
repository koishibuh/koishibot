namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Warning;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelwarningsend">Twitch Documentation</see><br/>
/// When a warning is sent to a user. Broadcasters and moderators can see the warning’s details.<br/>
/// Required Scopes: moderator:read:warnings or moderator:manage:warnings<br/>
/// </summary>
public class UserWarningSentEvent
{
    ///<summary>
    ///The user ID of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string? BroadcasterId { get; set; }

    ///<summary>
    ///The login of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string? BroadcasterLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string? BroadcasterName { get; set; }

    ///<summary>
    ///The user ID of the moderator who sent the warning.
    ///</summary>
    [JsonPropertyName("moderator_user_id")]
    public string? ModeratorUserId { get; set; }

    ///<summary>
    ///The login of the moderator.
    ///</summary>
    [JsonPropertyName("moderator_user_login")]
    public string? ModeratorUserLogin { get; set; }

    ///<summary>
    ///The user name of the moderator.
    ///</summary>
    [JsonPropertyName("moderator_user_name")]
    public string? ModeratorUserName { get; set; }

    ///<summary>
    ///The ID of the user being warned.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string? WarnedUserId { get; set; }

    ///<summary>
    ///The login of the user being warned.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string? WarnedUserLogin { get; set; }

    ///<summary>
    ///The user name of the user being.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string? WarnedUserName { get; set; }

    ///<summary>
    ///Optional. The reason given for the warning.
    ///</summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    ///<summary>
    ///Optional. The chat rules cited for the warning.
    ///</summary>
    [JsonPropertyName("chat_rules_cited")]
    public List<string>? ChatRulesCited { get; set; }
}