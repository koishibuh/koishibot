//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Services.TwitchEventSub.Extensions;
//using TwitchLib.Api.Core.Enums;

//namespace Koishibot.Core.Features.Shoutouts;

//public record ShoutoutApi(
//	ITwitchAPI TwitchApi, IOptions<Settings> Settings,
//	IRefreshAccessTokenService TokenProcessor,
//	ILogger<ShoutoutApi> Log) : IShoutoutApi
//{
//	public string StreamerId => Settings.Value.StreamerTokens.UserId;


//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-information">Get Channel Info Documentation</see> <br/>
//	/// BroadcasterId, BroadcasterLogin, BroadcasterName<br/>
//	/// BroadcasterLanguage, GameId, GameName, GameTitle<br/>
//	/// Delay, Tags, ContentClassificationLabels, IsBrandedContent<br/><br/>
//	/// Works for online or offline stream
//	/// </summary>
//	/// <returns></returns>
//	public async Task<StreamInfo> GetStreamInfo(string streamerId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Channels.GetChannelInformationAsync(streamerId);
//		return result is null || result.Data.Length == 0
//			? throw new Exception("Unable to get channel info from Api")
//			: result.ConvertToDto();
//	}

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-a-shoutout">Send Shoutout Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task SendShoutout(string userId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		await TwitchApi.Helix.Chat.SendShoutoutAsync(StreamerId, userId, StreamerId);
//	}

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
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-videos">Get Videos Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<RecentVod?> GetRecentVod(string userId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Videos.GetVideosAsync
//			(userId: userId, first: 1, type: VideoType.Archive);

//		return result.FindRecentVod();
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