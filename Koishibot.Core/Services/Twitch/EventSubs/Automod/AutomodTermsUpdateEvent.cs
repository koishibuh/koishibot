using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodtermsupdate">Twitch Documentation</see><br/>
/// When a broadcaster’s automod terms are updated. Changes to private terms are not sent.<br/>
/// Required Scopes: moderator:manage:automod<br/>
/// </summary>
public class AutomodTermsUpdateEvent
{
    ///<summary>
    ///The ID of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string? BroadcasterId { get; set; }

    ///<summary>
    ///The login of the broadcaster specified in the request. (Lowercase)
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string? BroadcasterLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string? BroadcasterName { get; set; }

    ///<summary>
    ///The ID of the moderator who changed the channel settings.
    ///</summary>
    [JsonPropertyName("moderator_user_id")]
    public string? ModeratorId { get; set; }

    ///<summary>
    ///The moderator’s login. (Lowercase)
    ///</summary>
    [JsonPropertyName("moderator_user_login")]
    public string? ModeratorLogin { get; set; }

    ///<summary>
    ///The moderator’s user name.
    ///</summary>
    [JsonPropertyName("moderator_user_name")]
    public string? ModeratorName { get; set; }

    ///<summary>
    ///The status change applied to the terms.
    ///</summary>
    [JsonPropertyName("action")]
    public AutomodAction? Action { get; set; }

    ///<summary>
    ///Indicates whether this term was added due to an Automod message approve/deny action.
    ///</summary>
    [JsonPropertyName("from_automod")]
    public bool FromAutomod { get; set; }

    ///<summary>
    ///The list of terms that had a status change.
    ///</summary>
    [JsonPropertyName("terms")]
    public List<string>? Terms { get; set; }
}