using System.Text;
using System.Text.Json;

namespace Koishibot.Core.Services.TwitchApi;
public static class TwitchApiHelper
{
	public static EndPoint GetEndpoint(ApiRequest Api)
	{
		return Api switch
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
			ApiRequest.UpdateRewardRedemptionStatus => new EndPoint(HttpMethod.Patch, "channel_points/custom_rewards/redemptions"),
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
			//ApiRequest.GetVideos => new EndPoint(HttpMethod.Get, "videos"),
			//ApiRequest.DeleteVideos => new EndPoint(HttpMethod.Delete, "videos"),
			_ => throw new Exception("Not found")
		};
	}

	public static Dictionary<string, string> GetQueryParameters(ApiRequest Api, IOptions<Settings> Settings) 
	{
		var queryParams = new Dictionary<string, string>();
		
		switch (Api)
		{
			case ApiRequest.SnoozeNextAd:
			case ApiRequest.GetAdSchedule:
			case ApiRequest.EditChannelInfo:
				queryParams.Add("broadcaster_id", Settings.Value.StreamerTokens.UserId);
				break;
				case ApiRequest.SendChatAnnouncement:
				queryParams.Add("broadcaster_id", Settings.Value.StreamerTokens.UserId);
				queryParams.Add("moderator_id", Settings.Value.StreamerTokens.UserId);
				break;
		}

		return queryParams;
	}


	//public static string ToQueryParams(this object obj)
	//{
	//	var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(
	//			JsonSerializer.Serialize(obj),
	//			new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
	//	);

	//	var paramStrings = dict
	//			.Where(x => x.Value != null)
	//			.Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(Convert.ToString(x.Value))}");

	//	return paramStrings.Any() ? $"?{string.Join("&", paramStrings)}" : string.Empty;
	//}

	//public static string ToQueryParams(this object obj)
	//{
	//	var paramStrings = new List<string>();

	//	foreach (var p in obj.GetType().GetProperties())
	//	{
	//		var jsonPropertyNameAttribute = (JsonPropertyNameAttribute)p.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true).FirstOrDefault();
	//		var propertyName = jsonPropertyNameAttribute?.Name ?? p.Name;

	//		var value = Convert.ToString(p.GetValue(obj));

	//		if (value != null)
	//			paramStrings.Add($"{propertyName}={value}");
	//	}

	//	if (!paramStrings.Any())
	//		return string.Empty;

	//	return $"?{string.Join("&", paramStrings)}";
	//}

	public static StringContent ConvertToStringContent<T>(T requestBody)
	{
		var json = JsonSerializer.Serialize(requestBody);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		return content;
	}



	public static string ObjectQueryFormatter(this object value)
	{
		StringBuilder query = new StringBuilder();
		JsonDocument json_value = JsonDocument.Parse(JsonSerializer.Serialize(value));

		if (value != null)
		{
			Query_Object_Parameters_Extractor(query, json_value.RootElement, null);
		}

		json_value.Dispose();
		return query.ToString();
	}

	private static void Query_Array_Parameters_Extractor(StringBuilder query, JsonElement json_value, string name)
	{
		StringBuilder name_builder = new StringBuilder();

		foreach (JsonElement element in json_value.EnumerateArray())
		{
			name_builder.Append(name);
			name_builder.Append("[");
			name_builder.Append(query.Length);
			name_builder.Append("]");

			string name_result = name_builder.ToString();
			name_builder.Clear();

			if (element.ValueKind == JsonValueKind.Object)
			{
				Query_Object_Parameters_Extractor(query, element, name_result);
			}
			else if (element.ValueKind == JsonValueKind.Array)
			{
				Query_Array_Parameters_Extractor(query, element, name_result);
			}
			else
			{
				query.Append(System.Web.HttpUtility.UrlEncode(name_result));
				query.Append("=");
				query.Append(System.Web.HttpUtility.UrlEncode(element.ToString()));
			}

			if (query.Length > 0)
			{
				query.Append("&");
			}
		}
	}

	private static void Query_Object_Parameters_Extractor(StringBuilder query, JsonElement json_value, string? name)
	{
		StringBuilder name_builder = new StringBuilder();

		foreach (JsonProperty property in json_value.EnumerateObject())
		{
			if (name != null)
			{
				name_builder.Append(name);
				name_builder.Append(".");
				name_builder.Append(property.Name);
			}
			else
			{
				name_builder.Append(property.Name);
			}

			string name_result = name_builder.ToString();
			name_builder.Clear();

			if (property.Value.ValueKind == JsonValueKind.Object)
			{
				Query_Object_Parameters_Extractor(query, property.Value, name_result);
			}
			else if (property.Value.ValueKind == JsonValueKind.Array)
			{
				Query_Array_Parameters_Extractor(query, property.Value, name_result);
			}
			else
			{
				query.Append(System.Web.HttpUtility.UrlEncode(name_result));
				query.Append("=");
				query.Append(System.Web.HttpUtility.UrlEncode(property.Value.ToString()));
			}

			if (query.Length > 0)
			{
				query.Append("&");
			}
		}
	}

}


public record EndPoint(HttpMethod HttpMethod, string Url);
