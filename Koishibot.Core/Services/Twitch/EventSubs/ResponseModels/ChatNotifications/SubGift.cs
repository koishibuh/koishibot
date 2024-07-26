using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class SubGift
{
    /// <summary>
    /// The number of months the subscription is for.
    /// </summary>
    [JsonPropertyName("duration_months")]
    public int DurationMonths { get; set; }

    /// <summary>
    /// Optional. The amount of gifts the gifter has given in this channel. Null if anonymous.
    /// </summary>
    [JsonPropertyName("cumulative_total")]
    public int? CumulativeTotal { get; set; }

    /// <summary>
    /// The user ID of the subscription gift recipient.
    /// </summary>
    [JsonPropertyName("recipient_user_id")]
    public string RecipientUserId { get; set; }

    /// <summary>
    /// The user name of the subscription gift recipient.
    /// </summary>
    [JsonPropertyName("recipient_user_name")]
    public string RecipientUserName { get; set; }

    /// <summary>
    /// The user login of the subscription gift recipient.
    /// </summary>
    [JsonPropertyName("recipient_user_login")]
    public string RecipientUserLogin { get; set; }

    /// <summary>
    /// The type of subscription plan being used.
    /// </summary>
    [JsonPropertyName("sub_tier")]
    public SubTier SubTier { get; set; }

    /// <summary>
    /// Optional. The ID of the associated community gift. Null if not associated with a community gift.
    /// </summary>
    [JsonPropertyName("community_gift_id")]
    public string? CommunityGiftId { get; set; }
}