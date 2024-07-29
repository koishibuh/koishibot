using Koishibot.Core.Features.ChatMessages.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;
using Koishibot.Core.Services.TwitchApi.Models;
using System.Text.RegularExpressions;


namespace Koishibot.Core.Features.Common;

public static class TwitchApiExtensions
{

	// == ⚫ Chat

	public static ChatMessageDto ConvertToDto(this ChatMessageReceivedEvent e, TwitchUser user)
	{
		string? command = null;
		string message = "";

		var trimmed = e.Message.Text;

		if (trimmed.StartsWith("!"))
		{
			var match = Regex.Match(e.Message.Text, @"^\s*!+(?<command>\w+)(?:\s+(?<message>.+))?$");
			if (match.Success)
			{
				command = match.Groups["command"].Value.ToLower();
				message = match.Groups["message"].Value;
			}
		}
		else
		{
			message = trimmed;
		}

		return new ChatMessageDto
		{
			User = user,
			//new TwitchUserDto(e.ChatterId, e.ChatterLogin, e.ChatterName),
			Badges = e.Badges,
			Color = e.Color,
			Message = message,
			Command = command
		};			
	}

	public static ChatMessageVm ConvertToVm(this ChatMessageReceivedEvent e) 
	{
		return new ChatMessageVm(e.ChatterId, e.ChatterName, new List<KeyValuePair<string, string>>(),
			e.Color, e.Message.Text);		 
	}



	// == ⚫ Channel Api
	//public static LiveStreamInfo ConvertToDto(this GetStreamsResponse response)
	//{
	//	var stream = response.Streams[0];

	//	return new LiveStreamInfo(
	//		stream.UserId,
	//		stream.Id,
	//		stream.GameId,
	//		stream.GameName,
	//		stream.Title,
	//		stream.ViewerCount,
	//		stream.StartedAt,
	//		stream.ThumbnailUrl
	//		);
	//}

	// == ⚫ Chat Api
	//public static bool IsCheckRestricted(this GetChatSettingsResponse response)
	//{
	//	return response.Data[0].FollowerMode == true
	//		|| response.Data[0].SubscriberMode == true;
	//}

	// == ⚫ Poll Api
	//public static bool IsPollStatusArchived(this EndPollResponse response)
	//{
	//	return response.Data[0].Status is "ARCHIVED";
	//}

	//public static CreatePollRequest CreatePollRequest(this PendingPoll poll, string streamerId)
	//{
	//	var choices = poll.Choices
	//		.Select(title => new Choice { Title = title })
	//		.ToArray();

	//	return new CreatePollRequest
	//	{
	//		BroadcasterId = streamerId,
	//		Title = poll.Title,
	//		Choices = choices,
	//		DurationSeconds = poll.Duration
	//	};
	//}

	// == ⚫ Raid Api
	//public static List<FollowingLiveStreamInfo> ConvertToDto(this GetFollowedStreamsResponse response)
	//{
	//	return response.Data.Select(r =>
	//		new FollowingLiveStreamInfo(
	//			new TwitchUserDto(r.UserId, r.UserLogin, r.UserName),
	//			new LiveStreamInfo(
	//				r.UserId,
	//				r.Id, r.GameId, r.GameName,
	//				r.Title, r.ViewerCount, r.StartedAt,
	//				r.ThumbnailUrl)))
	//			.ToList();
	//}

	// == ⚫ Users Api
	//public static UserInfo ConvertToDto(this GetUsersResponse reponse)
	//{
	//	var user = reponse.Users[0];

	//	return new UserInfo(
	//		user.Id,
	//		user.Login,
	//		user.DisplayName,
	//		user.BroadcasterType,
	//		user.Description,
	//		user.ProfileImageUrl
	//	);
	//}

	// == ⚫ Videos Api
	public static string? FindChannelTrailerUrl(this GetVideosResponse response)
	{
		return response.Data.Count == 0
		? null
		: response.Data
			.Where(t => t.Title.Contains("Channel Trailer"))
			.Select(t => t.VideoId)
			.FirstOrDefault();
	}

	public static string? FindRecentVodUrl(this GetVideosResponse response)
	{
		return response.Data.Count == 0
			? null
			: response.Data[0].VideoId;
	}

	public static string? GetTopClip(this GetClipsResponse response)
	{

		return response.Data.Count == 0
			? null
			: response.Data[0].ClipUrl;
	}

	//public static RecentVod? FindRecentVod(this GetVideosResponse response)
	//{
	//	if (response.Videos.Length == 0) { return null; };
	//	var v = response.Videos[0];
	//	return new RecentVod(v.UserId, v.Id, v.CreatedAt, v.Duration, v.PublishedAt, v.Url, v.Title,
	//					v.Type, v.Description, v.ThumbnailUrl, v.Viewable, v.ViewCount);
	//}

	// == ⚫ Schedule Api

	//public static DateTime? GetStartTime(this GetChannelStreamScheduleResponse response)
	//{
	//	return response.Schedule.Segments[0].StartTime;
	//}

	// == ⚫ Channel Points

	//public static ChannelPointReward ConvertToEntity(this CustomReward e)
	//{
	//	return new ChannelPointReward
	//	{
	//		TwitchId = e.Id,
	//		Title = e.Title,
	//		Description = e.Prompt,
	//		Cost = e.Cost,
	//		BackgroundColor = e.BackgroundColor,
	//		IsEnabled = e.IsEnabled,
	//		IsUserInputRequired = e.IsUserInputRequired,
	//		IsMaxPerStreamEnabled = e.MaxPerStreamSetting.IsEnabled,
	//		MaxPerStream = e.MaxPerStreamSetting.MaxPerStream,
	//		IsMaxPerUserPerStreamEnabled = e.MaxPerUserPerStreamSetting.IsEnabled,
	//		MaxPerUserPerStream = e.MaxPerUserPerStreamSetting.MaxPerUserPerStream,
	//		IsGlobalCooldownEnabled = e.GlobalCooldownSetting.IsEnabled,
	//		GlobalCooldownSeconds = e.GlobalCooldownSetting.GlobalCooldownSeconds,
	//		IsPaused = e.IsPaused,
	//		ShouldRedemptionsSkipRequestQueue = e.ShouldRedemptionsSkipQueue,
	//		ImageUrl = e.Image is not null ? e.Image.Url4x : e.DefaultImage.Url4x
	//	};
	//}
}