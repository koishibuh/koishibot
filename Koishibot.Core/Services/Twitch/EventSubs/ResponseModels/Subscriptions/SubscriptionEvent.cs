using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscribe">Twitch Documentation</see><br/>
/// When a specified channel receives a subscriber. This does not include resubscribes.<br/>
/// Required Scopes: channel:read:subscriptions
/// </summary>
public class SubscriptionEvent
{
    /// <summary>
    /// The user ID for the user who subscribed to the specified channel.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string SubscriberUserId { get; set; } = string.Empty;

    /// <summary>
    /// The user login for the user who subscribed to the specified channel. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string SubscriberUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The user display name for the user who subscribed to the specified channel.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string SubscriberUserName { get; set; } = string.Empty;

    /// <summary>
    ///	The broadcaster’s user ID.
    /// </summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user login. (Lowercase)
    /// </summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user display name.
    /// </summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; } = string.Empty;

    /// <summary>
    /// The tier of the subscription.
    /// </summary>
    [JsonPropertyName("tier")]
    public SubTier Tier { get; set; }

    /// <summary>
    /// Whether the subscription is a gift.
    /// </summary>
    [JsonPropertyName("is_gift")]
    public bool IsGift { get; set; }
}
