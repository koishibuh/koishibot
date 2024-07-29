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
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}