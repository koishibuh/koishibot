using Koishibot.Core.Services.TwitchEventSubNew.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs;
public static class EventSubHelper
{
    public static string GetVersionForType(SubscriptionType type)
    {
        switch (type)
        {
            case SubscriptionType.ChannelGuestStarSessionBegin:
            case SubscriptionType.ChannelGuestStarSessionEnd:
            case SubscriptionType.ChannelGuestStarGuestUpdate:
            case SubscriptionType.ChannelGuestStarSettingsUpdate:
                return "beta";

            case SubscriptionType.ChannelUpdate:
            case SubscriptionType.ChannelFollow:
            case SubscriptionType.ChannelModerate:
                return "2";

            default:
                return "1";
        }
    }

    public static Dictionary<string, string> GetConditionsForType(SubscriptionType type, string streamerId)
    {
        var conditions = new Dictionary<string, string>();

        switch (type)
        {
            // BroadcasterUserId
            case SubscriptionType.ChannelAdBreakBegin:
            case SubscriptionType.ChannelBan:
            case SubscriptionType.ChannelSubscribe:
            case SubscriptionType.ChannelSubscriptionEnd:
            case SubscriptionType.ChannelSubscriptionGift:
            case SubscriptionType.ChannelSubscriptionMessage:
            case SubscriptionType.ChannelCheer:
            case SubscriptionType.ChannelUpdate:
            case SubscriptionType.ChannelUnban:
            case SubscriptionType.ChannelModeratorAdd:
            case SubscriptionType.ChannelModeratorRemove:
            case SubscriptionType.ChannelPointsAutomaticRewardRedemption:
            case SubscriptionType.ChannelPointsCustomRewardAdd:
            case SubscriptionType.ChannelPointsCustomRewardUpdate: // reward_id optional
            case SubscriptionType.ChannelPointsCustomRewardRemove: // reward_id optional
            case SubscriptionType.ChannelPointsCustomRewardRedemptionAdd: // reward_id optional
            case SubscriptionType.ChannelPointsCustomRewardRedemptionUpdate: // reward_id optional
            case SubscriptionType.ChannelPollBegin:
            case SubscriptionType.ChannelPollProgress:
            case SubscriptionType.ChannelPollEnd:
            case SubscriptionType.ChannelPredictionBegin:
            case SubscriptionType.ChannelPredictionProgress:
            case SubscriptionType.ChannelPredictionLock:
            case SubscriptionType.ChannelPredictionEnd:
            case SubscriptionType.ChannelVipAdd:
            case SubscriptionType.ChannelVipRemove:
            case SubscriptionType.GoalBegin:
            case SubscriptionType.GoalProgress:
            case SubscriptionType.GoalEnd:
            case SubscriptionType.HypeTrainBegin:
            case SubscriptionType.HypeTrainProgress:
            case SubscriptionType.HypeTrainEnd:
            case SubscriptionType.StreamOnline:
            case SubscriptionType.StreamOffline:
            case SubscriptionType.CharityDonation:
            case SubscriptionType.CharityCampaignStart:
            case SubscriptionType.CharityCampaignProgress:
            case SubscriptionType.CharityCampaignStop:
                conditions.Add(ConvertEnum(ConditionType.BroadcasterUserId), streamerId);
                break;

            // FromBroadcasterUserId
            case SubscriptionType.ChannelRaidSent:
                conditions.Add(ConvertEnum(ConditionType.FromBroadcasterUserId), streamerId);
                break;

            // ToBroadcasterUserId
            case SubscriptionType.ChannelRaidReceived:
                conditions.Add(ConvertEnum(ConditionType.ToBroadcasterUserId), streamerId);
                break;

            // BroadcasterUserId & ModeratorUserId
            case SubscriptionType.AutomodMessageHold:
            case SubscriptionType.AutomodMessageUpdate:
            case SubscriptionType.AutomodSettingsUpdate:
            case SubscriptionType.AutomodTermsUpdate:
            case SubscriptionType.ChannelFollow:
            case SubscriptionType.ChannelUnbanRequestCreate:
            case SubscriptionType.ChannelUnbanRequestResolve:
            case SubscriptionType.ChannelModerate:
            case SubscriptionType.ChannelGuestStarSessionBegin:
            case SubscriptionType.ChannelGuestStarSessionEnd:
            case SubscriptionType.ChannelGuestStarGuestUpdate:
            case SubscriptionType.ChannelGuestStarSettingsUpdate:
            case SubscriptionType.ChannelSuspiciousUserMessage:
            case SubscriptionType.ChannelSuspiciousUserUpdate:
            case SubscriptionType.ChannelWarningAcknowledgement:
            case SubscriptionType.ChannelWarningSend:
            case SubscriptionType.ShieldModeBegin:
            case SubscriptionType.ShieldModeEnd:
            case SubscriptionType.ShoutoutCreate:
            case SubscriptionType.ShoutoutReceive:
                conditions.Add(ConvertEnum(ConditionType.BroadcasterUserId), streamerId);
                conditions.Add(ConvertEnum(ConditionType.ModeratorUserId), streamerId);
                break;

            // BroadcasterUserId & UserId
            case SubscriptionType.ChannelChatClear:
            case SubscriptionType.ChannelChatClearUserMessages:
            case SubscriptionType.ChannelChatMessage:
            case SubscriptionType.ChannelChatMessageDelete:
            case SubscriptionType.ChannelChatNotification:
            case SubscriptionType.ChannelChatSettingsUpdate:
            case SubscriptionType.ChannelChatUserMessageHold:
            case SubscriptionType.ChannelChatUserMessageUpdate:
                conditions.Add(ConvertEnum(ConditionType.BroadcasterUserId), streamerId);
                conditions.Add(ConvertEnum(ConditionType.UserId), streamerId);
                break;


            // UserId
            case SubscriptionType.WhisperReceived:
            case SubscriptionType.UserUpdate:
                conditions.Add(ConvertEnum(ConditionType.UserId), streamerId);
                break;


            //case SubscriptionType.ConduitShardDisabled:
            //case SubscriptionType.DropEntitlementGrant: // Webhooks Only
            //case SubscriptionType.ExtensionBitsTransaction: // Webhooks Only
            //case SubscriptionType.UserAuthorizationGrant: // Webhooks Only
            //case SubscriptionType.UserAuthorizationRevoke: // Webhooks Only
            case SubscriptionType.NotSupported:
            default:
                break;
        }

        return conditions;
    }

    public static string ConvertEnum(ConditionType type)
    {
        return type switch
        {
            ConditionType.BroadcasterUserId => "broadcaster_user_id",
            ConditionType.ModeratorUserId => "moderator_user_id",
            ConditionType.UserId => "user_id",
            ConditionType.ToBroadcasterUserId => "to_broadcaster_user_id",
            ConditionType.FromBroadcasterUserId => "from_broadcaster_user_id",
            ConditionType.RewardId => "reward_id",
            _ => "",
        };
    }
}
