using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptionend">Twitch Documentation</see><br/>
/// When a subscription to the specified channel expires.
/// Required Scopes: channel:read:subscriptions
/// </summary>
public class SubEndedEvent
{
    /// <summary>
    /// The user ID for the user whose subscription ended.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string SubscriberUserId { get; set; } = string.Empty;

    /// <summary>
    /// The user login for the user whose subscription ended. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string SubscriberUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The user display name for the user whose subscription ended.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string SubscriberUserName { get; set; } = string.Empty;

    /// <summary>
    ///	The broadcaster’s user ID.
    /// </summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterId { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user login. (Lowercase)
    /// </summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterLogin { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user display name.
    /// </summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterName { get; set; } = string.Empty;

    /// <summary>
    /// The tier of the subscription that ended.
    /// </summary>
    [JsonPropertyName("tier")]
    public SubTier Tier { get; set; }

    /// <summary>
    /// Whether the subscription was a gift.
    /// </summary>
    [JsonPropertyName("is_gift")]
    public bool IsGift { get; set; }
}
