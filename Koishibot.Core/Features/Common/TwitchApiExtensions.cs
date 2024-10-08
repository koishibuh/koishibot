using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.Common;

public static class TwitchApiExtensions
{
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