//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Raids.Interfaces;
//using Koishibot.Core.Features.TwitchUsers.Models;
//using TwitchLib.Api.Core.Enums;

//namespace Koishibot.Core.Features.Shoutouts;

//public record PromoVideoService(
//		IPromoVideoApi PromoVideoApi
//		) : IPromoVideoService
//{
//	public async Task<string?> Start(TwitchUser user)
//	{
//		var videoUrl = "https://player.twitch.tv/?muted=false&parent=twitch.tv&video=";

//		await Task.Delay(300);

//		var channelTrailer = await PromoVideoApi.GetChannelTrailer(user.TwitchId);
//		if (channelTrailer is not null) { return $"{videoUrl}{channelTrailer}"; }

//		await Task.Delay(300);

//		var recentVod = await PromoVideoApi.GetRecentVodId(user.TwitchId);
//		if (recentVod is not null) { return $"{videoUrl}{recentVod}"; }

//		await Task.Delay(300);

//		var clip = await PromoVideoApi.GetTopClip(user.TwitchId);
//		if (clip is not null) { return clip; }

//		return null;
//	}
//}

//// == ⚫ TWITCH API == //

//public record PromoVideoApi(
//	ITwitchAPI TwitchApi, IOptions<Settings> Settings,
//	IRefreshAccessTokenService TokenProcessor,
//	ILogger<ShoutoutApi> Log) : IPromoVideoApi
//{
//	public string StreamerId => Settings.Value.StreamerTokens.UserId;

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-videos">Get Videos Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<string?> GetChannelTrailer(string userId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Videos.GetVideosAsync
//			(userId: userId, first: 20, type: VideoType.Upload);

//		return result.FindChannelTrailerUrl();
//	}

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-videos">Get Videos Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<string?> GetRecentVodId(string userId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Videos.GetVideosAsync
//			(userId: userId, first: 1, type: VideoType.Archive);

//		return result.FindRecentVodUrl();
//	}

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-clips">Get Clips Documentation</see>< /br>
//	/// Get streamer's top clip from the last 30 days.
//	/// </summary>
//	/// <returns></returns>
//	public async Task<string?> GetTopClip(string userId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var dateRange = Toolbox.DateRangeOfLast30Days();

//		var result = await TwitchApi.Helix.Clips.GetClipsAsync
//			(broadcasterId: userId, startedAt: dateRange[0], endedAt: dateRange[1], first: 1);

//		return result.GetTopClip();
//	}
//}