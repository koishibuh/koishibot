using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using Koishibot.Core.Services.Twitch.Enums;
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace Koishibot.Core.Services.Twitch;

public static class TwitchApiHelper
{
	public static List<EventSubSubscriptionType> DebugSubscribeToEvents()
	{
		return [
			// EventSubSubscriptionType.ChannelBitsUsed,
			// EventSubSubscriptionType.ChannelUpdate,
			// EventSubSubscriptionType.ChannelChatMessage,
			// EventSubSubscriptionType.StreamOnline,
			// EventSubSubscriptionType.StreamOffline,
			// EventSubSubscriptionType.ChannelPollBegin,
			// EventSubSubscriptionType.ChannelPollProgress,
			// EventSubSubscriptionType.ChannelPollEnd,
			// EventSubSubscriptionType.ChannelFollow,
			// EventSubSubscriptionType.ChannelSubscribe,
			// EventSubSubscriptionType.ChannelBan,
			// EventSubSubscriptionType.ChannelSubscriptionEnd,
			// EventSubSubscriptionType.ChannelSubscriptionGift,
			// EventSubSubscriptionType.ChannelSubscriptionMessage,
			// EventSubSubscriptionType.ChannelRaidSent,
			// EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption,
			// EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd
			EventSubSubscriptionType.ChannelChatNotification
		];
	}


	public static List<EventSubSubscriptionType> SubscribeToEvents()
	{
		return
		[
			// //EventSubSubscriptionType.AutomodMessageHold,
			// //EventSubSubscriptionType.AutomodMessageUpdate,
			// //EventSubSubscriptionType.AutomodSettingsUpdate,
			// //EventSubSubscriptionType.AutomodTermsUpdate,
			EventSubSubscriptionType.ChannelUpdate,
			EventSubSubscriptionType.ChannelFollow,
			EventSubSubscriptionType.ChannelAdBreakBegin,
			// //EventSubSubscriptionType.ChannelChatClear,
			// //EventSubSubscriptionType.ChannelChatClearUserMessages,
			EventSubSubscriptionType.ChannelChatMessage,
			// EventSubSubscriptionType.ChannelChatMessageDelete,
			EventSubSubscriptionType.ChannelChatNotification,
			// //EventSubSubscriptionType.ChannelChatSettingsUpdate,
			// //EventSubSubscriptionType.ChannelChatUserMessageHold,
			// //EventSubSubscriptionType.ChannelChatUserMessageUpdate,
			// EventSubSubscriptionType.ChannelSubscribe, // Now using ChannelChatNotification
			// EventSubSubscriptionType.ChannelSubscriptionEnd, 
			// EventSubSubscriptionType.ChannelSubscriptionGift, // Now using ChannelChatNotification
			// EventSubSubscriptionType.ChannelSubscriptionMessage, // Now using ChannelChatNotification
			// EventSubSubscriptionType.ChannelCheer, // Now using ChannelBitsUsed
			EventSubSubscriptionType.ChannelBitsUsed,
			// EventSubSubscriptionType.ChannelRaidSent,
			// EventSubSubscriptionType.ChannelRaidReceived,
			// EventSubSubscriptionType.ChannelBan,
			// EventSubSubscriptionType.ChannelUnban,
			// //EventSubSubscriptionType.ChannelUnbanRequestCreate,
			// //EventSubSubscriptionType.ChannelUnbanRequestResolve,
			// EventSubSubscriptionType.ChannelModerate,
			// EventSubSubscriptionType.ChannelModeratorAdd,
			// EventSubSubscriptionType.ChannelModeratorRemove,
			// //EventSubSubscriptionType.ChannelGuestStarSessionBegin,
			// //EventSubSubscriptionType.ChannelGuestStarSessionEnd,
			// //EventSubSubscriptionType.ChannelGuestStarGuestUpdate,
			// //EventSubSubscriptionType.ChannelGuestStarSettingsUpdate,
			// EventSubSubscriptionType.ChannelPointsAutomaticRewardRedemption,
			// EventSubSubscriptionType.ChannelPointsCustomRewardAdd,
			// EventSubSubscriptionType.ChannelPointsCustomRewardUpdate,
			// EventSubSubscriptionType.ChannelPointsCustomRewardRemove,
			EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionAdd,
			// EventSubSubscriptionType.ChannelPointsCustomRewardRedemptionUpdate,
			EventSubSubscriptionType.ChannelPollBegin,
			EventSubSubscriptionType.ChannelPollProgress,
			EventSubSubscriptionType.ChannelPollEnd,
			// EventSubSubscriptionType.ChannelPredictionBegin,
			// EventSubSubscriptionType.ChannelPredictionProgress,
			// EventSubSubscriptionType.ChannelPredictionLock,
			// EventSubSubscriptionType.ChannelPredictionEnd,
			// EventSubSubscriptionType.ChannelSuspiciousUserMessage,
			// EventSubSubscriptionType.ChannelSuspiciousUserUpdate,
			// EventSubSubscriptionType.ChannelVipAdd,
			// EventSubSubscriptionType.ChannelVipRemove,
			// EventSubSubscriptionType.UserWarningAcknowledgement,
			// EventSubSubscriptionType.ChannelWarningSend,
			// //EventSubSubscriptionType.CharityDonation,
			// //EventSubSubscriptionType.CharityCampaignStart,
			// //EventSubSubscriptionType.CharityCampaignProgress,
			// //EventSubSubscriptionType.CharityCampaignStop,
			// //EventSubSubscriptionType.ConduitShardDisabled,
			// //EventSubSubscriptionType.DropEntitlementGrant,
			// //EventSubSubscriptionType.ExtensionBitsTransactionCreate,
			// EventSubSubscriptionType.ChannelGoalBegin,
			// EventSubSubscriptionType.ChannelGoalProgress,
			// EventSubSubscriptionType.ChannelGoalEnd,
			// EventSubSubscriptionType.HypeTrainBegin,
			// EventSubSubscriptionType.HypeTrainProgress,
			// EventSubSubscriptionType.HypeTrainEnd,
			// EventSubSubscriptionType.ShieldModeBegin,
			// EventSubSubscriptionType.ShieldModeEnd,
			// EventSubSubscriptionType.ShoutoutCreate,
			// EventSubSubscriptionType.ShoutoutReceive,
			EventSubSubscriptionType.StreamOnline,
			EventSubSubscriptionType.StreamOffline
			// //EventSubSubscriptionType.UserAuthorizationGrant,
			// //EventSubSubscriptionType.UserAuthorizationRevoke,
			// //EventSubSubscriptionType.UserUpdate,
			// //EventSubSubscriptionType.WhisperReceived
		];
	}


	public static EndPoint GetEndpoint(ApiRequest api)
	{
		return api switch
		{
			ApiRequest.StartAd => new EndPoint(HttpMethod.Post, "channels/commercial"),
			ApiRequest.GetAdSchedule => new EndPoint(HttpMethod.Get, "channels/ads"),
			ApiRequest.SnoozeNextAd => new EndPoint(HttpMethod.Post, "channels/ads/schedule/snooze"),
			ApiRequest.GetGameAnalytics => new EndPoint(HttpMethod.Get, "analytics/games"),
			ApiRequest.GetBitsLeaderboard => new EndPoint(HttpMethod.Get, "bits/leaderboard"),
			ApiRequest.GetCheermotes => new EndPoint(HttpMethod.Get, "bits/cheermotes"),
			ApiRequest.GetChannelInfo => new EndPoint(HttpMethod.Get, "channels"),
			ApiRequest.EditChannelInfo => new EndPoint(HttpMethod.Patch, "channels"),
			ApiRequest.GetChannelEditors => new EndPoint(HttpMethod.Get, "channels/editors"),
			ApiRequest.GetFollowedChannels => new EndPoint(HttpMethod.Get, "channels/followed"),
			ApiRequest.GetChannelFollowers => new EndPoint(HttpMethod.Get, "channels/followers"),
			ApiRequest.CreateCustomReward => new EndPoint(HttpMethod.Post, "channel_points/custom_rewards"),
			ApiRequest.DeleteCustomReward => new EndPoint(HttpMethod.Delete, "channel_points/custom_rewards"),
			ApiRequest.GetCustomRewards => new EndPoint(HttpMethod.Get, "channel_points/custom_rewards"),
			ApiRequest.GetCustomRewardRedemptions => new EndPoint(HttpMethod.Get, "channel_points/custom_rewards/redemptions"),
			ApiRequest.UpdateCustomReward => new EndPoint(HttpMethod.Patch, "channel_points/custom_rewards"),
			ApiRequest.UpdateRewardRedemptionStatus => new EndPoint(HttpMethod.Patch,
			"channel_points/custom_rewards/redemptions"),
			ApiRequest.GetCharityCampaign => new EndPoint(HttpMethod.Get, "charity/campaigns"),
			ApiRequest.GetCharityCampaignDonations => new EndPoint(HttpMethod.Get, "charity/donations"),
			ApiRequest.GetChatters => new EndPoint(HttpMethod.Get, "chat/chatters"),
			ApiRequest.GetChannelEmotes => new EndPoint(HttpMethod.Get, "chat/emotes"),
			ApiRequest.GetGlobalEmotes => new EndPoint(HttpMethod.Get, "chat/emotes/global"),
			ApiRequest.GetEmoteSets => new EndPoint(HttpMethod.Get, "chat/emotes/set"),
			ApiRequest.GetChannelChatBadges => new EndPoint(HttpMethod.Get, "chat/badges"),
			ApiRequest.GetGlobalChatBadges => new EndPoint(HttpMethod.Get, "badges/global"),
			ApiRequest.GetChatSettings => new EndPoint(HttpMethod.Get, "chat/settings"),
			ApiRequest.GetUserEmotes => new EndPoint(HttpMethod.Get, "chat/emotes/user"),
			ApiRequest.UpdateChatSettings => new EndPoint(HttpMethod.Patch, "chat/settings"),
			ApiRequest.SendChatAnnouncement => new EndPoint(HttpMethod.Post, "chat/announcements"),
			ApiRequest.SendShoutout => new EndPoint(HttpMethod.Post, "chat/shoutouts"),
			ApiRequest.SendChatMessage => new EndPoint(HttpMethod.Post, "chat/messages"),
			ApiRequest.GetUserChatColor => new EndPoint(HttpMethod.Get, "chat/color"),
			ApiRequest.UpdateUserChatColor => new EndPoint(HttpMethod.Put, "chat/color"),
			ApiRequest.CreateClip => new EndPoint(HttpMethod.Post, "clips"),
			ApiRequest.GetClips => new EndPoint(HttpMethod.Get, "clips"),
			ApiRequest.GetContentClassificationLabels => new EndPoint(HttpMethod.Get, "content_classification_labels"),
			//ApiRequest.CreateEventSubSubscription => new EndPoint(HttpMethod.Post, "eventsub/subscriptions"),
			//ApiRequest.DeleteEventSubSubscription => new EndPoint(HttpMethod.Delete, "eventsub/subscriptions"),
			ApiRequest.GetEventSubSubscriptions => new EndPoint(HttpMethod.Get, "eventsub/subscriptions"),
			ApiRequest.GetTopGames => new EndPoint(HttpMethod.Get, "games/top"),
			ApiRequest.GetGamesAndCategories => new EndPoint(HttpMethod.Get, "games"),
			ApiRequest.GetChannelGoals => new EndPoint(HttpMethod.Get, "goals"),
			//ApiRequest.GetChannelGuestStarSettings => new EndPoint(HttpMethod.Get, "guest_star/channel_settings"),
			//ApiRequest.UpdateChannelGuestStarSettings => new EndPoint(HttpMethod.Put, "guest_star/channel_settings"),
			//ApiRequest.GetGuestStarSession => new EndPoint(HttpMethod.Get, "guest_star/session"),
			//ApiRequest.CreateGuestStarSession => new EndPoint(HttpMethod.Post, "guest_star/session"),
			//ApiRequest.EndGuestStarSession => new EndPoint(HttpMethod.Delete, "guest_star/session"),
			//ApiRequest.GetGuestStarInvites => new EndPoint(HttpMethod.Get, "guest_star/invites"),
			//ApiRequest.SendGuestStarInvite => new EndPoint(HttpMethod.Post, "guest_star/invites"),
			//ApiRequest.DeleteGuestStarInvite => new EndPoint(HttpMethod.Delete, "guest_star/invites"),
			//ApiRequest.AssignGuestStarSlot => new EndPoint(HttpMethod.Post, "guest_star/slot"),
			//ApiRequest.UpdateGuestStarSlot => new EndPoint(HttpMethod.Patch, "guest_star/slot"),
			//ApiRequest.DeleteGuestStarSlot => new EndPoint(HttpMethod.Delete, "guest_star/slot"),
			//ApiRequest.UpdateGuestStarSlotSettings => new EndPoint(HttpMethod.Patch, "guest_star/slot_settings"),
			ApiRequest.GetHypeTrainEvents => new EndPoint(HttpMethod.Get, "hypetrain/events"),
			ApiRequest.CheckAutomodStatus => new EndPoint(HttpMethod.Post, "moderation/enforcements/status"),
			ApiRequest.ManageHeldAutomodMessages => new EndPoint(HttpMethod.Post, "moderation/automod/message"),
			ApiRequest.GetAutomodSettings => new EndPoint(HttpMethod.Get, "moderation/automod/settings"),
			ApiRequest.UpdateAutomodSettings => new EndPoint(HttpMethod.Put, "moderation/automod/settings"),
			ApiRequest.GetBannedUsers => new EndPoint(HttpMethod.Get, "moderation/banned"),
			ApiRequest.BanUser => new EndPoint(HttpMethod.Post, "moderation/bans"),
			ApiRequest.UnbanUser => new EndPoint(HttpMethod.Delete, "moderation/bans"),
			ApiRequest.GetUnbanRequests => new EndPoint(HttpMethod.Get, "moderation/unban_requests"),
			ApiRequest.ResolveUnbanRequest => new EndPoint(HttpMethod.Patch, "moderation/unban_requests"),
			ApiRequest.GetBlockedTerms => new EndPoint(HttpMethod.Get, "moderation/blocked_terms"),
			ApiRequest.AddBlockedTerm => new EndPoint(HttpMethod.Post, "moderation/blocked_terms"),
			ApiRequest.RemoveBlockedTerm => new EndPoint(HttpMethod.Delete, "moderation/blocked_terms"),
			ApiRequest.DeleteChatMessages => new EndPoint(HttpMethod.Delete, "moderation/chat"),
			ApiRequest.GetModeratedChannels => new EndPoint(HttpMethod.Get, "moderation/channels"),
			ApiRequest.GetChannelModerators => new EndPoint(HttpMethod.Get, "moderation/moderators"),
			ApiRequest.AddChannelModerator => new EndPoint(HttpMethod.Post, "moderation/moderators"),
			ApiRequest.RemoveChannelModerator => new EndPoint(HttpMethod.Delete, "moderation/moderators"),
			ApiRequest.GetVips => new EndPoint(HttpMethod.Get, "channels/vips"),
			ApiRequest.AddVip => new EndPoint(HttpMethod.Post, "channels/vips"),
			ApiRequest.RemoveVip => new EndPoint(HttpMethod.Delete, "channels/vips"),
			ApiRequest.UpdateShieldModeStatus => new EndPoint(HttpMethod.Put, "moderation/shield_mode"),
			ApiRequest.GetShieldModeStatus => new EndPoint(HttpMethod.Get, "moderation/shield_mode"),
			ApiRequest.WarnChatUser => new EndPoint(HttpMethod.Post, "moderation/warnings"),
			ApiRequest.GetPolls => new EndPoint(HttpMethod.Get, "polls"),
			ApiRequest.CreatePoll => new EndPoint(HttpMethod.Post, "polls"),
			ApiRequest.EndPoll => new EndPoint(HttpMethod.Patch, "polls"),
			ApiRequest.GetPredictions => new EndPoint(HttpMethod.Get, "predictions"),
			ApiRequest.CreatePrediction => new EndPoint(HttpMethod.Post, "predictions"),
			ApiRequest.EndPrediction => new EndPoint(HttpMethod.Patch, "predictions"),
			ApiRequest.StartRaid => new EndPoint(HttpMethod.Post, "raids"),
			ApiRequest.CancelRaid => new EndPoint(HttpMethod.Delete, "raids"),
			ApiRequest.GetChannelStreamSchedule => new EndPoint(HttpMethod.Get, "schedule"),
			ApiRequest.GetChannelICalendar => new EndPoint(HttpMethod.Get, "schedule/icalendar"),
			ApiRequest.UpdateChannelStreamSchedule => new EndPoint(HttpMethod.Patch, "schedule/settings"),
			ApiRequest.CreateChannelStreamScheduleSegment => new EndPoint(HttpMethod.Post, "schedule/segment"),
			ApiRequest.UpdateChannelStreamScheduleSegment => new EndPoint(HttpMethod.Patch, "schedule/segment"),
			ApiRequest.DeleteChannelStreamScheduleSegment => new EndPoint(HttpMethod.Delete, "schedule/segment"),
			ApiRequest.SearchGamesAndCategories => new EndPoint(HttpMethod.Get, "search/categories"),
			ApiRequest.SearchChannels => new EndPoint(HttpMethod.Get, "search/channels"),
			ApiRequest.GetStreamKey => new EndPoint(HttpMethod.Get, "streams/key"),
			ApiRequest.GetLiveStreams => new EndPoint(HttpMethod.Get, "streams"),
			ApiRequest.GetFollowedLiveStreams => new EndPoint(HttpMethod.Get, "streams/followed"),
			ApiRequest.CreateStreamMarker => new EndPoint(HttpMethod.Post, "streams/markers"),
			ApiRequest.GetStreamMarkers => new EndPoint(HttpMethod.Get, "streams/markers"),
			ApiRequest.GetBrocasterSubscriptions => new EndPoint(HttpMethod.Get, "subscriptions"),
			ApiRequest.CheckUserSubscription => new EndPoint(HttpMethod.Get, "subscriptions/user"),
			//ApiRequest.GetChannelTeams => new EndPoint(HttpMethod.Get, "teams/channel"),
			//ApiRequest.GetTeams => new EndPoint(HttpMethod.Get, "teams"),
			ApiRequest.GetUsers => new EndPoint(HttpMethod.Get, "users"),
			ApiRequest.UpdateUser => new EndPoint(HttpMethod.Put, "users"),
			ApiRequest.GetUserBlockList => new EndPoint(HttpMethod.Get, "users/blocks"),
			ApiRequest.BlockUser => new EndPoint(HttpMethod.Put, "users/blocks"),
			ApiRequest.UnblockUser => new EndPoint(HttpMethod.Delete, "users/blocks"),
			ApiRequest.GetVideos => new EndPoint(HttpMethod.Get, "videos"),
			ApiRequest.DeleteVideos => new EndPoint(HttpMethod.Delete, "videos"),
			ApiRequest.SendWhisper => new EndPoint(HttpMethod.Post, "whispers"),
			_ => throw new Exception("Not found")
		};
	}


	public static StringContent ConvertToStringContent<T>(T requestBody)
	{
		var json = JsonSerializer.Serialize(requestBody);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		return content;
	}
}

public static class QueryStringConverterStj
{
	public static string ObjectQueryFormatter(this object value)
	{
		var builder = new QueryStringBuilder();

		var json = JsonSerializer.SerializeToNode(value)!.AsObject();
		AppendObject(builder, json);

		return builder.ToString();
	}

	private static void AppendObject(QueryStringBuilder builder, JsonObject json)
	{
		foreach (var (key, value) in json)
		{
			if (value is null)
				continue;

			AppendValue(builder, key, value);
		}
	}

	private static void AppendValue(QueryStringBuilder builder, string key, JsonNode value)
	{
		if (value is JsonArray jArray)
		{
			foreach (var element in jArray)
				if (element != null)
					AppendValue(builder, key, element);
		}
		else
		{
			builder.Append(key, value.ToString());
		}
	}
}

public class QueryStringBuilder
{
	private StringBuilder Builder { get; } = new();

	public QueryStringBuilder Append(string key, string value)
	{
		if (Builder.Length > 0)
			Builder.Append('&');
		Builder
		.Append(HttpUtility.UrlEncode(key))
		.Append('=')
		.Append(HttpUtility.UrlEncode(value));
		return this;
	}

	public override string ToString()
	{
		return Builder.ToString();
	}
}



public record EndPoint(HttpMethod HttpMethod, string Url);