using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.UnbanRequests;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelunban_requestresolve">Twitch Documentation</see><br/>
/// When an unban request has been resolved.<br/>
/// Required Scopes: moderator:read:unban_requests or moderator:manage:unban_requests<br/>
/// </summary>
public class UnbanRequestResolvedEvent
{
    ///<summary>
    ///The ID of the unban request.
    ///</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    ///<summary>
    ///The broadcaster’s user ID for the channel the unban request was updated for.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string? BroadcasterId { get; set; }

    ///<summary>
    ///The broadcaster’s login name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string? BroadcasterLogin { get; set; }

    ///<summary>
    ///The broadcaster’s display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string? BroadcasterName { get; set; }

    ///<summary>
    ///Optional. User ID of moderator who approved/denied the request.
    ///</summary>
    [JsonPropertyName("moderator_id")]
    public string? ModeratorUserId { get; set; }

    ///<summary>
    ///Optional. The moderator’s login name
    ///</summary>
    [JsonPropertyName("moderator_login")]
    public string? ModeratorUserLogin { get; set; }

    ///<summary>
    ///Optional. The moderator’s display name
    ///</summary>
    [JsonPropertyName("moderator_name")]
    public string? ModeratorUserName { get; set; }

    ///<summary>
    ///User ID of user that requested to be unbanned.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string? ViewerUserId { get; set; }

    ///<summary>
    ///The user’s login name.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string? ViewerUserLogin { get; set; }

    ///<summary>
    ///The user’s display name.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string? ViewerUserName { get; set; }

    ///<summary>
    ///Optional. Resolution text supplied by the mod/broadcaster upon approval/denial of the request.
    ///</summary>
    [JsonPropertyName("resolution_text")]
    public string? ResolutionText { get; set; }

    ///<summary>
    ///Dictates whether the unban request was approved or denied.
    ///</summary>
    [JsonPropertyName("status")]
    public UnbanStatus Status { get; set; }
}