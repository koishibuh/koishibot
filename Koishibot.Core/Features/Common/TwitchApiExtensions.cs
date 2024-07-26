//using Koishibot.Core.Features.ChannelPoints.Models;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.Polls.Models;
//using Koishibot.Core.Features.RaidSuggestions.Models;
//using Koishibot.Core.Features.StreamInformation.Models;
//using Koishibot.Core.Features.TwitchUsers.Models;
//using TwitchLib.Api.Helix.Models.ChannelPoints;
//using TwitchLib.Api.Helix.Models.Chat.ChatSettings;
//using TwitchLib.Api.Helix.Models.Clips.GetClips;
//using TwitchLib.Api.Helix.Models.Polls.CreatePoll;
//using TwitchLib.Api.Helix.Models.Polls.EndPoll;
//using TwitchLib.Api.Helix.Models.Schedule.GetChannelStreamSchedule;
//using TwitchLib.Api.Helix.Models.Streams.GetFollowedStreams;
//using TwitchLib.Api.Helix.Models.Streams.GetStreams;
//using TwitchLib.Api.Helix.Models.Users.GetUsers;
//using TwitchLib.Api.Helix.Models.Videos.GetVideos;

//namespace Koishibot.Core.Features.Common;

//public static class TwitchApiExtensions
//{
//	// == ⚫ Channel Api
//	public static LiveStreamInfo ConvertToDto(this GetStreamsResponse response)
//	{
//		var stream = response.Streams[0];

//		return new LiveStreamInfo(
//			stream.UserId,
//			stream.Id,
//			stream.GameId,
//			stream.GameName,
//			stream.Title,
//			stream.ViewerCount,
//			stream.StartedAt,
//			stream.ThumbnailUrl
//			);
//	}

//	// == ⚫ Chat Api
//	public static bool IsCheckRestricted(this GetChatSettingsResponse response)
//	{
//		return response.Data[0].FollowerMode == true
//			|| response.Data[0].SubscriberMode == true;
//	}

//	// == ⚫ Poll Api
//	public static bool IsPollStatusArchived(this EndPollResponse response)
//	{
//		return response.Data[0].Status is "ARCHIVED";
//	}

//	public static CreatePollRequest CreatePollRequest(this PendingPoll poll, string streamerId)
//	{
//		var choices = poll.Choices
//			.Select(title => new Choice { Title = title })
//			.ToArray();

//		return new CreatePollRequest
//		{
//			BroadcasterId = streamerId,
//			Title = poll.Title,
//			Choices = choices,
//			DurationSeconds = poll.Duration
//		};
//	}

//	// == ⚫ Raid Api
//	public static List<FollowingLiveStreamInfo> ConvertToDto(this GetFollowedStreamsResponse response)
//	{
//		return response.Data.Select(r =>
//			new FollowingLiveStreamInfo(
//				new TwitchUserDto(r.UserId, r.UserLogin, r.UserName),
//				new LiveStreamInfo(
//					r.UserId,
//					r.Id, r.GameId, r.GameName,
//					r.Title, r.ViewerCount, r.StartedAt,
//					r.ThumbnailUrl)))
//				.ToList();
//	}

//	// == ⚫ Users Api
//	public static UserInfo ConvertToDto(this GetUsersResponse reponse)
//	{
//		var user = reponse.Users[0];

//		return new UserInfo(
//			user.Id,
//			user.Login,
//			user.DisplayName,
//			user.BroadcasterType,
//			user.Description,
//			user.ProfileImageUrl
//		);
//	}

//	// == ⚫ Videos Api
//	public static string? FindChannelTrailerUrl(this GetVideosResponse response)
//	{
//		return response.Videos.Length == 0
//		? null
//		: response.Videos
//			.Where(t => t.Title.Contains("Channel Trailer"))
//			.Select(t => t.Id)
//			.FirstOrDefault();
//	}

//	public static string? FindRecentVodUrl(this GetVideosResponse response)
//	{
//		return response.Videos.Length == 0
//			? null
//			: response.Videos[0].Id;
//	}

//	public static string? GetTopClip(this GetClipsResponse response)
//	{
//		return response.Clips.Length == 0
//			? null
//			: response.Clips[0].Url;
//	}

//	public static RecentVod? FindRecentVod(this GetVideosResponse response)
//	{
//		if (response.Videos.Length == 0) { return null; };
//		var v = response.Videos[0];
//		return new RecentVod(v.UserId, v.Id, v.CreatedAt, v.Duration, v.PublishedAt, v.Url, v.Title,
//						v.Type, v.Description, v.ThumbnailUrl, v.Viewable, v.ViewCount);
//	}

//	// == ⚫ Schedule Api

//	public static DateTime? GetStartTime(this GetChannelStreamScheduleResponse response)
//	{
//		return response.Schedule.Segments[0].StartTime;
//	}

//	// == ⚫ Channel Points

//	public static ChannelPointReward ConvertToEntity(this CustomReward e)
//	{
//		return new ChannelPointReward
//		{
//			TwitchId = e.Id,
//			Title = e.Title,
//			Description = e.Prompt,
//			Cost = e.Cost,
//			BackgroundColor = e.BackgroundColor,
//			IsEnabled = e.IsEnabled,
//			IsUserInputRequired = e.IsUserInputRequired,
//			IsMaxPerStreamEnabled = e.MaxPerStreamSetting.IsEnabled,
//			MaxPerStream = e.MaxPerStreamSetting.MaxPerStream,
//			IsMaxPerUserPerStreamEnabled = e.MaxPerUserPerStreamSetting.IsEnabled,
//			MaxPerUserPerStream = e.MaxPerUserPerStreamSetting.MaxPerUserPerStream,
//			IsGlobalCooldownEnabled = e.GlobalCooldownSetting.IsEnabled,
//			GlobalCooldownSeconds = e.GlobalCooldownSetting.GlobalCooldownSeconds,
//			IsPaused = e.IsPaused,
//			ShouldRedemptionsSkipRequestQueue = e.ShouldRedemptionsSkipQueue,
//			ImageUrl = e.Image is not null ? e.Image.Url4x : e.DefaultImage.Url4x
//		};
//	}
//}