﻿using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;

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
        throw new NotImplementedException();
    }
}