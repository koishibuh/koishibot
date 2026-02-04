using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(SubscriptionTypeEnumConverter))]
public enum EventSubSubscriptionType
{
	AutomodMessageHold = 1,
	AutomodMessageUpdate,
	AutomodSettingsUpdate,
	AutomodTermsUpdate,
	ChannelUpdate,
	ChannelFollow,
	ChannelAdBreakBegin,
	ChannelChatClear,
	ChannelChatClearUserMessages,
	ChannelChatMessage,
	ChannelChatMessageDelete,
	ChannelChatNotification,
	ChannelChatSettingsUpdate,
	ChannelChatUserMessageHold,
	ChannelChatUserMessageUpdate,
	ChannelSharedChatBegin,
	ChannelSharedChatUpdate,
	ChannelSharedChatEnd,
	ChannelSubscribe,
	ChannelSubscriptionEnd,
	ChannelSubscriptionGift,
	ChannelSubscriptionMessage,
	ChannelCheer,
	ChannelBitsUsed,
	ChannelRaid,
	ChannelRaidSent, //custom
	ChannelRaidReceived, //custom
	ChannelBan,
	ChannelUnban,
	ChannelUnbanRequestCreate,
	ChannelUnbanRequestResolve,
	ChannelModerate,
	ChannelModeratorAdd,
	ChannelModeratorRemove,
	ChannelGuestStarSessionBegin, // beta
	ChannelGuestStarSessionEnd, // beta
	ChannelGuestStarGuestUpdate, // beta
	ChannelGuestStarSettingsUpdate, // beta
	ChannelPointsAutomaticRewardRedemption,
	ChannelPointsCustomRewardAdd,
	ChannelPointsCustomRewardUpdate,
	ChannelPointsCustomRewardRemove,
	ChannelPointsCustomRewardRedemptionAdd,
	ChannelPointsCustomRewardRedemptionUpdate,
	ChannelPollBegin,
	ChannelPollProgress,
	ChannelPollEnd,
	ChannelPredictionBegin,
	ChannelPredictionProgress,
	ChannelPredictionLock,
	ChannelPredictionEnd,
	ChannelSuspiciousUserMessage,
	ChannelSuspiciousUserUpdate,
	ChannelVipAdd,
	ChannelVipRemove,
	UserWarningAcknowledgement,
	ChannelWarningSend,
	CharityDonation,
	CharityCampaignStart,
	CharityCampaignProgress,
	CharityCampaignStop,
	//ConduitShardDisabled,
	//DropEntitlementGrant, // Webhooks Only
	//ExtensionBitsTransactionCreate, // Webhooks Only
	ChannelGoalBegin,
	ChannelGoalProgress,
	ChannelGoalEnd,
	HypeTrainBegin,
	HypeTrainProgress,
	HypeTrainEnd,
	ShieldModeBegin,
	ShieldModeEnd,
	ShoutoutCreate,
	ShoutoutReceive,
	StreamOnline,
	StreamOffline,
	//UserAuthorizationGrant, // Webhooks Only
	//UserAuthorizationRevoke, // Webhooks Only
	UserUpdate,
	WhisperReceived,
	NotSupported
}

// == ⚫ == //

public class SubscriptionTypeEnumConverter : JsonConverter<EventSubSubscriptionType>
{
	public override EventSubSubscriptionType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		if (string.IsNullOrEmpty(value))
		{
			return EventSubSubscriptionType.NotSupported;
		}

		return value switch
		{
			"automod.message.hold" => EventSubSubscriptionType.AutomodMessageHold,
			"automod.message.update" => EventSubSubscriptionType.AutomodMessageUpdate,
			"automod.settings.update" => EventSubSubscriptionType.AutomodSettingsUpdate,
			"automod.terms.update" => EventSubSubscriptionType.AutomodTermsUpdate,
			"channel.update" => EventSubSubscriptionType.ChannelUpdate,
			"channel.follow" => EventSubSubscriptionType.ChannelFollow,
			"channel.ad_break.begin" => EventSubSubscriptionType.ChannelAdBreakBegin,
			"channel.chat.clear" => EventSubSubscriptionType.ChannelChatClear,
			"channel.chat.clear_user_messages" => EventSubSubscriptionType.ChannelChatClearUserMessages,
			"channel.chat.message" => EventSubSubscriptionType.ChannelChatMessage,
			"channel.chat.message_delete" => EventSubSubscriptionType.ChannelChatMessageDelete,
			"channel.chat.notification" => EventSubSubscriptionType.ChannelChatNotification,
			"channel.chat_settings.update" => EventSubSubscriptionType.ChannelChatSettingsUpdate,
			"channel.chat.user_message_hold" => EventSubSubscriptionType.ChannelChatUserMessageHold,
			"channel.chat.user_message_update" => EventSubSubscriptionType.ChannelChatUserMessageUpdate,
			"channel.shared_chat.begin" => EventSubSubscriptionType.ChannelSharedChatBegin,
			"channel.shared_chat.update" => EventSubSubscriptionType.ChannelSharedChatUpdate,
			"channel.shared_chat.end" => EventSubSubscriptionType.ChannelSharedChatEnd,
			"channel.subscribe" => EventSubSubscriptionType.ChannelSubscribe,
			"channel.subscription.end" => EventSubSubscriptionType.ChannelSubscriptionEnd,
			"channel.subscription.gift" => EventSubSubscriptionType.ChannelSubscriptionGift,
			"channel.subscription.message" => EventSubSubscriptionType.ChannelSubscriptionMessage,
			"channel.cheer" => EventSubSubscriptionType.ChannelCheer, // old
			"channel.bits.use" => EventSubSubscriptionType.ChannelBitsUsed,
			"channel.raid" => EventSubSubscriptionType.ChannelRaid,
			"channel.ban" => EventSubSubscriptionType.ChannelBan,
			"channel.unban" => EventSubSubscriptionType.ChannelUnban,
			"channel.unban_request.create" => EventSubSubscriptionType.ChannelUnbanRequestCreate,
			"channel.unban_request.resolve" => EventSubSubscriptionType.ChannelUnbanRequestResolve,
			"channel.moderate" => EventSubSubscriptionType.ChannelModerate,
			"channel.moderator.add" => EventSubSubscriptionType.ChannelModeratorAdd,
			"channel.moderator.remove" => EventSubSubscriptionType.ChannelModeratorRemove,
			"channel.guest_star_session.begin" => EventSubSubscriptionType.ChannelGuestStarSessionBegin,
			"channel.guest_star_session.end" => EventSubSubscriptionType.ChannelGuestStarSessionEnd,
			"channel.guest_star_guest.update" => EventSubSubscriptionType.ChannelGuestStarSettingsUpdate,
			"channel.guest_star_settings.update" => EventSubSubscriptionType.ChannelGuestStarSettingsUpdate,
			"channel.channel_points_automatic_reward_redemption.add" => EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption,
			"channel.channel_points_custom_reward.add" => EventSubSubscriptionType.ChannelPointsCustomRewardAdd,
			"channel.channel_points_custom_reward.update" => EventSubSubscriptionType.ChannelPointsCustomRewardUpdate,
			"channel.channel_points_custom_reward.remove" => EventSubSubscriptionType.ChannelPointsCustomRewardRemove,
			"channel.channel_points_custom_reward_redemption.add" => EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd,
			"channel.channel_points_custom_reward_redemption.update" => EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate,
			"channel.poll.begin" => EventSubSubscriptionType.ChannelPollBegin,
			"channel.poll.progress" => EventSubSubscriptionType.ChannelPollProgress,
			"channel.poll.end" => EventSubSubscriptionType.ChannelPollEnd,
			"channel.prediction.begin" => EventSubSubscriptionType.ChannelPredictionBegin,
			"channel.prediction.progress" => EventSubSubscriptionType.ChannelPredictionProgress,
			"channel.prediction.lock" => EventSubSubscriptionType.ChannelPredictionLock,
			"channel.prediction.end" => EventSubSubscriptionType.ChannelPredictionEnd,
			"channel.suspicious_user.message" => EventSubSubscriptionType.ChannelSuspiciousUserMessage,
			"channel.suspicious_user.update" => EventSubSubscriptionType.ChannelSuspiciousUserUpdate,
			"channel.vip.add" => EventSubSubscriptionType.ChannelVipAdd,
			"channel.vip.remove" => EventSubSubscriptionType.ChannelVipRemove,
			"channel.warning.acknowledge" => EventSubSubscriptionType.UserWarningAcknowledgement,
			"channel.warning.send" => EventSubSubscriptionType.ChannelWarningSend,
			"channel.charity_campaign.donate" => EventSubSubscriptionType.CharityDonation,
			"channel.charity_campaign.start" => EventSubSubscriptionType.CharityCampaignStart,
			"channel.charity_campaign.progress" => EventSubSubscriptionType.CharityCampaignProgress,
			"channel.charity_campaign.stop" => EventSubSubscriptionType.CharityCampaignStop,
			//"conduit.shard.disabled" => SubscriptionType.ConduitShardDisabled,
			//"drop.entitlement.grant" => SubscriptionType.DropEntitlementGrant,
			//"extension.bits_transaction.create" => SubscriptionType.ExtensionBitsTransactionCreate,
			"channel.goal.begin" => EventSubSubscriptionType.ChannelGoalBegin,
			"channel.goal.progress" => EventSubSubscriptionType.ChannelGoalProgress,
			"channel.goal.end" => EventSubSubscriptionType.ChannelGoalEnd,
			"channel.hype_train.begin" => EventSubSubscriptionType.HypeTrainBegin,
			"channel.hype_train.progress" => EventSubSubscriptionType.HypeTrainProgress,
			"channel.hype_train.end" => EventSubSubscriptionType.HypeTrainEnd,
			"channel.shield_mode.begin" => EventSubSubscriptionType.ShieldModeBegin,
			"channel.shield_mode.end" => EventSubSubscriptionType.ShieldModeEnd,
			"channel.shoutout.create" => EventSubSubscriptionType.ShoutoutCreate,
			"channel.shoutout.receive" => EventSubSubscriptionType.ShoutoutReceive,
			"stream.online" => EventSubSubscriptionType.StreamOnline,
			"stream.offline" => EventSubSubscriptionType.StreamOffline,
			//"user.authorization.grant" => SubscriptionType.UserAuthorizationGrant,
			//"user.authorization.revoke" => SubscriptionType.UserAuthorizationRevoke,
			"user.update" => EventSubSubscriptionType.UserUpdate,
			"user.whisper.message" => EventSubSubscriptionType.WhisperReceived,
			_ => EventSubSubscriptionType.NotSupported
		};
	}

	public override void Write
			(Utf8JsonWriter writer, EventSubSubscriptionType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			EventSubSubscriptionType.AutomodMessageHold => "automod.message.hold",
			EventSubSubscriptionType.AutomodMessageUpdate => "automod.message.update",
			EventSubSubscriptionType.AutomodSettingsUpdate => "automod.settings.update",
			EventSubSubscriptionType.AutomodTermsUpdate => "automod.terms.update",
			EventSubSubscriptionType.ChannelUpdate => "channel.update",
			EventSubSubscriptionType.ChannelFollow => "channel.follow",
			EventSubSubscriptionType.ChannelAdBreakBegin => "channel.ad_break.begin",
			EventSubSubscriptionType.ChannelChatClear => "channel.chat.clear",
			EventSubSubscriptionType.ChannelChatClearUserMessages => "channel.chat.clear_user_messages",
			EventSubSubscriptionType.ChannelChatMessage => "channel.chat.message",
			EventSubSubscriptionType.ChannelChatMessageDelete => "channel.chat.message_delete",
			EventSubSubscriptionType.ChannelChatNotification => "channel.chat.notification",
			EventSubSubscriptionType.ChannelChatSettingsUpdate => "channel.chat_settings.update",
			EventSubSubscriptionType.ChannelChatUserMessageHold => "channel.chat.user_message_hold",
			EventSubSubscriptionType.ChannelChatUserMessageUpdate => "channel.chat.user_message_update",
			EventSubSubscriptionType.ChannelSharedChatBegin => "channel.shared_chat.begin",
			EventSubSubscriptionType.ChannelSharedChatUpdate => "channel.shared_chat.update",
			EventSubSubscriptionType.ChannelSharedChatEnd => "channel.shared_chat.end",
			EventSubSubscriptionType.ChannelSubscribe => "channel.subscribe",
			EventSubSubscriptionType.ChannelSubscriptionEnd => "channel.subscription.end",
			EventSubSubscriptionType.ChannelSubscriptionGift => "channel.subscription.gift",
			EventSubSubscriptionType.ChannelSubscriptionMessage => "channel.subscription.message",
			EventSubSubscriptionType.ChannelCheer => "channel.cheer",
			EventSubSubscriptionType.ChannelBitsUsed => "channel.bits.use",
			EventSubSubscriptionType.ChannelRaid => "channel.raid",
			EventSubSubscriptionType.ChannelRaidSent => "channel.raid", // custom
			EventSubSubscriptionType.ChannelRaidReceived => "channel.raid", // custom
			EventSubSubscriptionType.ChannelBan => "channel.ban",
			EventSubSubscriptionType.ChannelUnban => "channel.unban",
			EventSubSubscriptionType.ChannelUnbanRequestCreate => "channel.unban_request.create",
			EventSubSubscriptionType.ChannelUnbanRequestResolve => "channel.unban_request.resolve",
			EventSubSubscriptionType.ChannelModerate => "channel.moderate",
			EventSubSubscriptionType.ChannelModeratorAdd => "channel.moderator.add",
			EventSubSubscriptionType.ChannelModeratorRemove => "channel.moderator.remove",
			EventSubSubscriptionType.ChannelGuestStarSessionBegin => "channel.guest_star_session.begin",
			EventSubSubscriptionType.ChannelGuestStarSessionEnd => "channel.guest_star_session.end",
			EventSubSubscriptionType.ChannelGuestStarGuestUpdate => "channel.guest_star_guest.update",
			EventSubSubscriptionType.ChannelGuestStarSettingsUpdate => "channel.guest_star_settings.update",
			EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption => "channel.channel_points_automatic_reward_redemption.add",
			EventSubSubscriptionType.ChannelPointsCustomRewardAdd => "channel.channel_points_custom_reward.add",
			EventSubSubscriptionType.ChannelPointsCustomRewardUpdate => "channel.channel_points_custom_reward.update",
			EventSubSubscriptionType.ChannelPointsCustomRewardRemove => "channel.channel_points_custom_reward.remove",
			EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd => "channel.channel_points_custom_reward_redemption.add",
			EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate => "channel.channel_points_custom_reward_redemption.update",
			EventSubSubscriptionType.ChannelPollBegin => "channel.poll.begin",
			EventSubSubscriptionType.ChannelPollProgress => "channel.poll.progress",
			EventSubSubscriptionType.ChannelPollEnd => "channel.poll.end",
			EventSubSubscriptionType.ChannelPredictionBegin => "channel.prediction.begin",
			EventSubSubscriptionType.ChannelPredictionProgress => "channel.prediction.progress",
			EventSubSubscriptionType.ChannelPredictionLock => "channel.prediction.lock",
			EventSubSubscriptionType.ChannelPredictionEnd => "channel.prediction.end",
			EventSubSubscriptionType.ChannelSuspiciousUserMessage => "channel.suspicious_user.message",
			EventSubSubscriptionType.ChannelSuspiciousUserUpdate => "channel.suspicious_user.update",
			EventSubSubscriptionType.ChannelVipAdd => "channel.vip.add",
			EventSubSubscriptionType.ChannelVipRemove => "channel.vip.remove",
			EventSubSubscriptionType.UserWarningAcknowledgement => "channel.warning.acknowledge",
			EventSubSubscriptionType.ChannelWarningSend => "channel.warning.send",
			EventSubSubscriptionType.CharityDonation => "channel.charity_campaign.donate",
			EventSubSubscriptionType.CharityCampaignStart => "channel.charity_campaign.start",
			EventSubSubscriptionType.CharityCampaignProgress => "channel.charity_campaign.progress",
			EventSubSubscriptionType.CharityCampaignStop => "channel.charity_campaign.stop",
			//SubscriptionType.ConduitShardDisabled => "conduit.shard.disabled",
			//SubscriptionType.DropEntitlementGrant => "drop.entitlement.grant",
			//SubscriptionType.ExtensionBitsTransactionCreate => "extension.bits_transaction.create",
			EventSubSubscriptionType.ChannelGoalBegin => "channel.goal.begin",
			EventSubSubscriptionType.ChannelGoalProgress => "channel.goal.progress",
			EventSubSubscriptionType.ChannelGoalEnd => "channel.goal.end",
			EventSubSubscriptionType.HypeTrainBegin => "channel.hype_train.begin",
			EventSubSubscriptionType.HypeTrainProgress => "channel.hype_train.progress",
			EventSubSubscriptionType.HypeTrainEnd => "channel.hype_train.end",
			EventSubSubscriptionType.ShieldModeBegin => "channel.shield_mode.begin",
			EventSubSubscriptionType.ShieldModeEnd => "channel.shield_mode.end",
			EventSubSubscriptionType.ShoutoutCreate => "channel.shoutout.create",
			EventSubSubscriptionType.ShoutoutReceive => "channel.shoutout.receive",
			EventSubSubscriptionType.StreamOnline => "stream.online",
			EventSubSubscriptionType.StreamOffline => "stream.offline",
			//SubscriptionType.UserAuthorizationGrant => "user.authorization.grant",
			//SubscriptionType.UserAuthorizationRevoke => "user.authorization.revoke",
			EventSubSubscriptionType.UserUpdate => "user.update",
			EventSubSubscriptionType.WhisperReceived => "user.whisper.message",
			EventSubSubscriptionType.NotSupported => "",

			// custom exception here at some point
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}