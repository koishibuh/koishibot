using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatnotification">Twitch Documentation</see><br/>
/// When an event that appears in chat occurs, such as someone subscribing to the channel or a subscription is gifted.
/// Required Scopes: user:read:chat, AppAccessToken: user:bot (Chatter), channel:bot (Broadcaster or Mod)
/// </summary>
public class ChatNotificationEvent
{
    /// <summary>
    ///	The broadcaster’s user ID.
    /// </summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterId { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user display name.
    /// </summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUsername { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user login. (Lowercase)
    /// </summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterLogin { get; set; } = string.Empty;

    /// <summary>
    /// The user ID of the user that sent the message.
    /// </summary>
    [JsonPropertyName("chatter_user_id")]
    public string ChatterUserId { get; set; } = string.Empty;

    /// <summary>
    /// The user name of the user that sent the message.
    /// </summary>
    [JsonPropertyName("chatter_user_name")]
    public string ChatterUsername { get; set; } = string.Empty;

    /// <summary>
    /// The user login of the user that sent the message.
    /// </summary>
    [JsonPropertyName("chatter_user_login")]
    public string ChatterLogin { get; set; } = string.Empty;

    /// <summary>
    /// Whether or not the chatter is anonymous.
    /// </summary>
    [JsonPropertyName("chatter_is_anonymous")]
    public bool ChatterIsAnonymous { get; set; }

    /// <summary>
    /// The color of the user’s name in the chat room.
    /// </summary>
    [JsonPropertyName("color")]
    public string Color { get; set; }

    /// <summary>
    /// List of chat badges.
    /// </summary>
    [JsonPropertyName("badges")]
    public List<Badge> Badges { get; set; } = [];

    /// <summary>
    /// The message Twitch shows in the chat room for this notice.
    /// </summary>
    [JsonPropertyName("system_message")]
    public string SystemMessage { get; set; } = string.Empty;

    /// <summary>
    /// A UUID that identifies the message.
    /// </summary>
    [JsonPropertyName("message_id")]
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    /// The structured chat message.
    /// </summary>
    [JsonPropertyName("message")]
    public Message Message { get; set; }

    /// <summary>
    /// The type of notice.
    /// </summary>
    [JsonPropertyName("notice_type")]
    public NoticeType NoticeType { get; set; }

    /// <summary>
    /// Information about the sub event. Null if notice_type is not sub.
    /// </summary>
    [JsonPropertyName("sub")]
    public Subscription? Sub { get; set; }

    /// <summary>
    /// Information about the resub event. Null if notice_type is not resub.
    /// </summary>
    [JsonPropertyName("resub")]
    public Resubscription? Resub { get; set; }

    /// <summary>
    /// Information about the gift sub event. Null if notice_type is not sub_gift.
    /// </summary>
    [JsonPropertyName("sub_gift")]
    public SubGift? SubGift { get; set; }

    /// <summary>
    /// Information about the community gift sub event. Null if notice_type is not community_sub_gift.
    /// </summary>
    [JsonPropertyName("community_sub_gift")]
    public CommunitySubGift? CommunitySubGift { get; set; }

    /// <summary>
    /// Information about the Prime gift paid upgrade event. Null if notice_type is not prime_paid_upgrade.
    /// </summary>
    [JsonPropertyName("gift_paid_upgrade")]
    public GiftPaidUpgrade? GiftPaidUpgrade { get; set; }

    /// <summary>
    /// Information about the Prime gift paid upgrade event. Null if notice_type is not prime_paid_upgrade.
    /// </summary>
    [JsonPropertyName("prime_paid_upgrade")]
    public PrimePaidUpgrade? PrimePaidUpgrade { get; set; }

    /// <summary>
    /// Information about the raid event. Null if notice_type is not raid.
    /// </summary>
    [JsonPropertyName("raid")]
    public Raid? Raid { get; set; }

    /// <summary>
    /// Returns an empty payload if notice_type is unraid, otherwise returns null.
    /// </summary>
    [JsonPropertyName("unraid")]
    public Unraid? Unraid { get; set; }

    /// <summary>
    /// Information about the pay it forward event. Null if notice_type is not pay_it_forward.
    /// </summary>
    [JsonPropertyName("pay_it_forward")]
    public PayItForward? PayItForward { get; set; }

    /// <summary>
    /// Information about the announcement event. Null if notice_type is not announcement.
    /// </summary>
    [JsonPropertyName("announcement")]
    public Announcement Announcement { get; set; }

    /// <summary>
    /// Information about the charity donation event. Null if notice_type is not charity_donation.
    /// </summary>
    [JsonPropertyName("charity_donation")]
    public CharityDonation CharityDonation { get; set; }

    /// <summary>
    /// Information about the bits badge tier event. Null if notice_type is not bits_badge_tier.
    /// </summary>
    [JsonPropertyName("bits_badge_tier")]
    public BitsBadgeTier BitsBadgeTier { get; set; }
}