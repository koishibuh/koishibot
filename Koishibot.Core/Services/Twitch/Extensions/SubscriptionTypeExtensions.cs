using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.Extensions;

public static class SubscriptionTypeExtensions
{
	public static string GetVersion(this EventSubSubscriptionType type)
	{
		switch (type)
		{
			case EventSubSubscriptionType.ChannelGuestStarSessionBegin:
			case EventSubSubscriptionType.ChannelGuestStarSessionEnd:
			case EventSubSubscriptionType.ChannelGuestStarGuestUpdate:
			case EventSubSubscriptionType.ChannelGuestStarSettingsUpdate:
				return "beta";

			case EventSubSubscriptionType.ChannelUpdate:
			case EventSubSubscriptionType.ChannelFollow:
			case EventSubSubscriptionType.ChannelModerate:
			case EventSubSubscriptionType.HypeTrainBegin:
			case EventSubSubscriptionType.HypeTrainProgress:
			case EventSubSubscriptionType.HypeTrainEnd:
			case EventSubSubscriptionType.AutomodMessageHold:
			case EventSubSubscriptionType.AutomodMessageUpdate:
			case EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption:
				return "2";

			default:
				return "1";
		}
	}

	public static Dictionary<string, string> GetConditions(this EventSubSubscriptionType type, string streamerId)
	{
		var conditions = new Dictionary<string, string>();

		switch (type)
		{
			// BroadcasterId
			case EventSubSubscriptionType.ChannelAdBreakBegin:
			case EventSubSubscriptionType.ChannelBan:
			case EventSubSubscriptionType.ChannelSubscribe:
			case EventSubSubscriptionType.ChannelSubscriptionEnd:
			case EventSubSubscriptionType.ChannelSubscriptionGift:
			case EventSubSubscriptionType.ChannelSubscriptionMessage:
			case EventSubSubscriptionType.ChannelCheer: // old
			case EventSubSubscriptionType.ChannelBitsUsed:
			case EventSubSubscriptionType.ChannelUpdate:
			case EventSubSubscriptionType.ChannelUnban:
			case EventSubSubscriptionType.ChannelModeratorAdd:
			case EventSubSubscriptionType.ChannelModeratorRemove:
			case EventSubSubscriptionType.ChannelSharedChatBegin:
			case EventSubSubscriptionType.ChannelSharedChatUpdate:
			case EventSubSubscriptionType.ChannelSharedChatEnd:
			case EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption:
			case EventSubSubscriptionType.ChannelPointsCustomRewardAdd:
			case EventSubSubscriptionType.ChannelPointsCustomRewardUpdate: // reward_id optional
			case EventSubSubscriptionType.ChannelPointsCustomRewardRemove: // reward_id optional
			case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd: // reward_id optional
			case EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate: // reward_id optional
			case EventSubSubscriptionType.ChannelPollBegin:
			case EventSubSubscriptionType.ChannelPollProgress:
			case EventSubSubscriptionType.ChannelPollEnd:
			case EventSubSubscriptionType.ChannelPredictionBegin:
			case EventSubSubscriptionType.ChannelPredictionProgress:
			case EventSubSubscriptionType.ChannelPredictionLock:
			case EventSubSubscriptionType.ChannelPredictionEnd:
			case EventSubSubscriptionType.ChannelVipAdd:
			case EventSubSubscriptionType.ChannelVipRemove:
			case EventSubSubscriptionType.ChannelGoalBegin:
			case EventSubSubscriptionType.ChannelGoalProgress:
			case EventSubSubscriptionType.ChannelGoalEnd:
			case EventSubSubscriptionType.HypeTrainBegin:
			case EventSubSubscriptionType.HypeTrainProgress:
			case EventSubSubscriptionType.HypeTrainEnd:
			case EventSubSubscriptionType.StreamOnline:
			case EventSubSubscriptionType.StreamOffline:
			case EventSubSubscriptionType.CharityDonation:
			case EventSubSubscriptionType.CharityCampaignStart:
			case EventSubSubscriptionType.CharityCampaignProgress:
			case EventSubSubscriptionType.CharityCampaignStop:
				conditions.Add(ConvertEnum(ConditionType.BroadcasterId), streamerId);
				break;

			// FromBroadcasterId
			case EventSubSubscriptionType.ChannelRaidSent:
				conditions.Add(ConvertEnum(ConditionType.FromBroadcasterId), streamerId);
				break;

			// ToBroadcasterId
			case EventSubSubscriptionType.ChannelRaidReceived:
				conditions.Add(ConvertEnum(ConditionType.ToBroadcasterId), streamerId);
				break;

			// BroadcasterId & ModeratorUserId
			case EventSubSubscriptionType.AutomodMessageHold:
			case EventSubSubscriptionType.AutomodMessageUpdate:
			case EventSubSubscriptionType.AutomodSettingsUpdate:
			case EventSubSubscriptionType.AutomodTermsUpdate:
			case EventSubSubscriptionType.ChannelFollow:
			case EventSubSubscriptionType.ChannelUnbanRequestCreate:
			case EventSubSubscriptionType.ChannelUnbanRequestResolve:
			case EventSubSubscriptionType.ChannelModerate:
			case EventSubSubscriptionType.ChannelGuestStarSessionBegin:
			case EventSubSubscriptionType.ChannelGuestStarSessionEnd:
			case EventSubSubscriptionType.ChannelGuestStarGuestUpdate:
			case EventSubSubscriptionType.ChannelGuestStarSettingsUpdate:
			case EventSubSubscriptionType.ChannelSuspiciousUserMessage:
			case EventSubSubscriptionType.ChannelSuspiciousUserUpdate:
			case EventSubSubscriptionType.UserWarningAcknowledgement:
			case EventSubSubscriptionType.ChannelWarningSend:
			case EventSubSubscriptionType.ShieldModeBegin:
			case EventSubSubscriptionType.ShieldModeEnd:
			case EventSubSubscriptionType.ShoutoutCreate:
			case EventSubSubscriptionType.ShoutoutReceive:
				conditions.Add(ConvertEnum(ConditionType.BroadcasterId), streamerId);
				conditions.Add(ConvertEnum(ConditionType.ModeratorUserId), streamerId);
				break;

			// BroadcasterId & UserId
			case EventSubSubscriptionType.ChannelChatClear:
			case EventSubSubscriptionType.ChannelChatClearUserMessages:
			case EventSubSubscriptionType.ChannelChatMessage:
			case EventSubSubscriptionType.ChannelChatMessageDelete:
			case EventSubSubscriptionType.ChannelChatNotification:
			case EventSubSubscriptionType.ChannelChatSettingsUpdate:
			case EventSubSubscriptionType.ChannelChatUserMessageHold:
			case EventSubSubscriptionType.ChannelChatUserMessageUpdate:
				conditions.Add(ConvertEnum(ConditionType.BroadcasterId), streamerId);
				// conditions.Add(ConvertEnum(ConditionType.BroadcasterId), ""); // To sub to other channels
				conditions.Add(ConvertEnum(ConditionType.UserId), streamerId);
				break;


			// UserId
			case EventSubSubscriptionType.WhisperReceived:
			case EventSubSubscriptionType.UserUpdate:
				conditions.Add(ConvertEnum(ConditionType.UserId), streamerId);
				break;


			//case SubscriptionType.ConduitShardDisabled:
			//case SubscriptionType.DropEntitlementGrant: // Webhooks Only
			//case SubscriptionType.ExtensionBitsTransaction: // Webhooks Only
			//case SubscriptionType.UserAuthorizationGrant: // Webhooks Only
			//case SubscriptionType.UserAuthorizationRevoke: // Webhooks Only
			case EventSubSubscriptionType.NotSupported:
			default:
				break;
		}

		return conditions;
	}

	public static string ConvertEnum(ConditionType type)
	{
		return type switch
		{
			ConditionType.BroadcasterId => "broadcaster_user_id",
			ConditionType.ModeratorUserId => "moderator_user_id",
			ConditionType.UserId => "user_id",
			ConditionType.ToBroadcasterId => "to_broadcaster_user_id",
			ConditionType.FromBroadcasterId => "from_broadcaster_user_id",
			ConditionType.RewardId => "reward_id",
			_ => "",
		};
	}
}