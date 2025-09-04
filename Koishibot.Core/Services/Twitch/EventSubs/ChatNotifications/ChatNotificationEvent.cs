using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;
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
	public string BroadcasterName { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user login. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The user ID of the user that sent the message.
	/// </summary>
	[JsonPropertyName("chatter_user_id")]
	public string ChatterId { get; set; } = string.Empty;

	/// <summary>
	/// The user name of the user that sent the message.
	/// </summary>
	[JsonPropertyName("chatter_user_name")]
	public string ChatterName { get; set; } = string.Empty;

	/// <summary>
	/// The user login of the user that sent the message.
	/// TODO: Check if this still exists lol
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
	public string Color { get; set; } = "#F26C9E";

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
	public Message? Message { get; set; }

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
	public GifterInfo? GiftPaidUpgrade { get; set; }

	/// <summary>
	/// Information about the Prime gift paid upgrade event. Null if notice_type is not prime_paid_upgrade.
	/// </summary>
	[JsonPropertyName("prime_paid_upgrade")]
	public PrimePaidUpgrade? PrimePaidUpgrade { get; set; }
	
	/// <summary>
	/// Information about the pay it forward event. Null if notice_type is not pay_it_forward.
	/// </summary>
	[JsonPropertyName("pay_it_forward")]
	public GifterInfo? PayItForward { get; set; }

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
	/// Information about the announcement event. Null if notice_type is not announcement.
	/// </summary>
	[JsonPropertyName("announcement")]
	public Announcement? Announcement { get; set; }
	
	/// <summary>
	/// Information about the bits badge tier event. Null if notice_type is not bits_badge_tier.
	/// </summary>
	[JsonPropertyName("bits_badge_tier")]
	public BitsBadgeTier? BitsBadgeTier { get; set; }

	/// <summary>
	/// Information about the charity donation event. Null if notice_type is not charity_donation.
	/// </summary>
	[JsonPropertyName("charity_donation")]
	public CharityDonation? CharityDonation { get; set; }
	
	///<summary>
	///Optional. The broadcaster user ID of the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_broadcaster_user_id")]
	public string? SourceBroadcasterUserId { get; set; }
	
	///<summary>
	///Optional. The user name of the broadcaster of the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_broadcaster_user_name")]
	public string? SourceBroadcasterUsername { get; set; }
	
	///<summary>
	///Optional. The login of the broadcaster of the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_broadcaster_user_login")]
	public string? SourceBroadcasterUserLogin { get; set; }

	
	///<summary>
	///Optional. The UUID that identifies the source message from the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_message_id")]
	public string? SourceMessageId { get; set; }
	
	///<summary>
	///Optional. The list of chat badges for the chatter in the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_badges")]
	public List<Badge>? SourceBadges { get; set; }
	
	///<summary>
	///Optional. Information about the shared_chat_sub event. Is null if notice_type is not shared_chat_sub.<br/>
	///This field has the same information as the sub field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	///</summary>
	[JsonPropertyName("shared_chat_sub")]
	public Subscription? SharedChatSub { get; set; }

	/// <summary>
	/// Optional. Information about the shared_chat_resub event. Is null if notice_type is not shared_chat_resub.<br/>
	/// This field has the same information as the resub field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	/// </summary>
	[JsonPropertyName("shared_chat_resub")]
	public Resubscription? SharedChatResub { get; set; }

	///<summary>
	///Optional. Information about the shared_chat_sub_gift event. Is null if notice_type is not shared_chat_sub_gift.<br/>
	///This field has the same information as the chat_sub_gift field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	///</summary>
	[JsonPropertyName("shared_chat_sub_gift")]
	public SubGift? SharedChatSubGift { get; set; }

	///<summary>
	///Optional. Information about the shared_chat_community_sub_gift event. Is null if notice_type is not shared_chat_community_sub_gift.<br/>
	///This field has the same information as the community_sub_gift field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	///</summary>
	[JsonPropertyName("shared_chat_community_sub_gift")]
	public CommunitySubGift? SharedChatCommunitySubGift { get; set; }

	/// <summary>
	///Optional. Information about the shared_chat_gift_paid_upgrade event. Is null if notice_type is not shared_chat_gift_paid_upgrade.<br/>
	///This field has the same information as the gift_paid_upgrade field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	/// </summary>
	[JsonPropertyName("shared_chat_gift_paid_upgrade")]
	public GifterInfo? SharedChatGiftPaidUpgrade { get; set; }

	///<summary>
	///Optional. Information about the shared_chat_chat_prime_paid_upgrade event. Is null if notice_type is not shared_chat_prime_paid_upgrade.<br/>
	///This field has the same information as the prime_paid_upgrade field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	///</summary>
	[JsonPropertyName("shared_chat_prime_paid_upgrade")]
	public PrimePaidUpgrade? SharedChatPrimePaidUpgrade { get; set; }
	
	///<summary>
	///Optional. Information about the shared_chat_pay_it_forward event. Is null if notice_type is not shared_chat_pay_it_forward.<br/>
	///This field has the same information as the pay_it_forward field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	///</summary>
	[JsonPropertyName("shared_chat_pay_it_forward")]
	public GifterInfo? SharedChatPayItForward { get; set; }

	///<summary>
	///Optional. Information about the shared_chat_raid event. Is null if notice_type is not shared_chat_raid.<br/>
	///This field has the same information as the raid field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	///</summary>
	[JsonPropertyName("shared_chat_raid")]
	public Raid? SharedChatRaid { get; set; }
	
	///<summary>
	///Optional. Information about the shared_chat_announcement event. Is null if notice_type is not shared_chat_announcement.<br/>
	/// This field has the same information as the announcement field but for a notice that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
	///</summary>
	[JsonPropertyName("shared_chat_announcement")]
	public Announcement? SharedChatAnnouncement { get; set; }

}