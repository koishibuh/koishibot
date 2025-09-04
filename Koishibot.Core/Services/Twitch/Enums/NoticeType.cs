using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;

[JsonConverter(typeof(NoticeTypeEnumConverter))]
public enum NoticeType
{
	Sub = 1,
	Resub,
	SubGift,
	CommunitySubGift,
	GiftSubPaidUpgrade,
	PrimeSubPaidUpgrade,
	Raid,
	CancelRaid,
	PayItForwardSub,
	Announcement,
	BitsBadgeTierUpgrade,
	CharityDonation,
	SharedChatSub,
	SharedChatResub,
	SharedChatSubGift,
	SharedChatCommunitySubGift,
	SharedChatGiftSubPaidUpgrade,
	SharedChatPrimeSubPaidUpgrade,
	SharedChatRaid,
	SharedChatPayItForward,
	SharedChatAnnouncement
}

// == ⚫ == //

public class NoticeTypeEnumConverter : JsonConverter<NoticeType>
{
	public override NoticeType Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"sub" => NoticeType.Sub,
			"resub" => NoticeType.Resub,
			"sub_gift" => NoticeType.SubGift,
			"community_sub_gift" => NoticeType.CommunitySubGift,
			"gift_paid_upgrade" => NoticeType.GiftSubPaidUpgrade,
			"prime_paid_upgrade" => NoticeType.PrimeSubPaidUpgrade,
			"raid" => NoticeType.Raid,
			"unraid" => NoticeType.CancelRaid,
			"pay_it_forward" => NoticeType.PayItForwardSub,
			"announcement" => NoticeType.Announcement,
			"bits_badge_tier" => NoticeType.BitsBadgeTierUpgrade,
			"charity_donation" => NoticeType.CharityDonation,
			"shared_chat_sub" => NoticeType.SharedChatSub,
			"shared_chat_resub" => NoticeType.SharedChatResub,
			"shared_chat_sub_gift" => NoticeType.SharedChatSubGift,
			"shared_chat_community_sub_gift" => NoticeType.SharedChatCommunitySubGift,
			"shared_chat_gift_paid_upgrade" => NoticeType.SharedChatGiftSubPaidUpgrade,
			"shared_chat_prime_paid_upgrade" => NoticeType.SharedChatPrimeSubPaidUpgrade,
			"shared_chat_raid" => NoticeType.SharedChatRaid,
			"shared_chat_pay_it_forward" => NoticeType.SharedChatPayItForward,
			"shared_chat_announcement" => NoticeType.SharedChatAnnouncement,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, NoticeType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			NoticeType.Sub => "sub",
			NoticeType.Resub => "resub",
			NoticeType.SubGift => "sub_gift",
			NoticeType.CommunitySubGift => "community_sub_gift",
			NoticeType.GiftSubPaidUpgrade => "gift_paid_upgrade",
			NoticeType.PrimeSubPaidUpgrade => "prime_paid_upgrade",
			NoticeType.Raid => "raid",
			NoticeType.CancelRaid => "unraid",
			NoticeType.PayItForwardSub => "pay_it_forward",
			NoticeType.Announcement => "announcement",
			NoticeType.BitsBadgeTierUpgrade => "bits_badge_tier",
			NoticeType.CharityDonation => "charity_donation",
			NoticeType.SharedChatSub  => "shared_chat_sub",
			NoticeType.SharedChatResub => "shared_chat_resub",
			NoticeType.SharedChatSubGift => "shared_chat_sub_gift" ,
			NoticeType.SharedChatCommunitySubGift => "shared_chat_community_sub_gift",
			NoticeType.SharedChatGiftSubPaidUpgrade => "shared_chat_gift_paid_upgrade",
			NoticeType.SharedChatPrimeSubPaidUpgrade=> "shared_chat_prime_paid_upgrade",
			NoticeType.SharedChatRaid => "shared_chat_raid",
			NoticeType.SharedChatPayItForward => "shared_chat_pay_it_forward",
			NoticeType.SharedChatAnnouncement => "shared_chat_announcement",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}