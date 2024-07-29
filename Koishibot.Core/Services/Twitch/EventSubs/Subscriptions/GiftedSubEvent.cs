using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Subscriptions;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelsubscriptiongift">Twitch Documentation</see><br/>
/// When a user gives one or more gifted subscriptions in a channel.<br/>
/// Required Scopes: channel:read:subscriptions
/// </summary>
public class GiftedSubEvent
{
    /// <summary>
    /// The user ID of the user who sent the subscription gift. Set to null if it was an anonymous subscription gift.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? GifterUserId { get; set; }

    /// <summary>
    /// The user login of the user who sent the gift. Set to null if it was an anonymous subscription gift. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string? GifterUserLogin { get; set; }

    /// <summary>
    /// The user display name of the user who sent the gift. Set to null if it was an anonymous subscription gift.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string? GifterUserName { get; set; }

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
    /// The number of subscriptions in the subscription gift.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    /// The tier of subscriptions in the subscription gift.
    /// </summary>
    [JsonPropertyName("tier")]
    public SubTier Tier { get; set; }

    /// <summary>
    /// The total number of subscriptions gifted by this user in the channel overall.<br/>
    /// This value is null for anonymous gifts or if the gifter has opted out of sharing this information.
    /// </summary>
    [JsonPropertyName("cumulative_total")]
    public int? CumulativeTotal { get; set; }

    /// <summary>
    /// Whether the subscription gift was anonymous.
    /// </summary>
    [JsonPropertyName("is_anonymous")]
    public bool IsAnonymous { get; set; }
}
