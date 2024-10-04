using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Raids.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.Shoutouts;

public record PromoVideoService(
ITwitchApiRequest TwitchApiRequest
//IPromoVideoApi PromoVideoApi
) : IPromoVideoService
{
	public async Task<string?> Start(TwitchUser user)
	{
		const string videoUrl = "https://player.twitch.tv/?muted=false&parent=twitch.tv&video=";

		await Task.Delay(300);

		var channelTrailer = await GetChannelTrailer(user.TwitchId);
		if (channelTrailer is not null)
		{
			return $"{videoUrl}{channelTrailer}";
		}

		await Task.Delay(300);

		var recentVod = await GetRecentVod(user.TwitchId);
		if (recentVod is not null)
		{
			return $"{videoUrl}{recentVod}";
		}

		await Task.Delay(300);

		var recentTopClip = await GetRecentTopClip(user.TwitchId);

		return recentTopClip
			?? null;
	}

	/*═════════════【】═════════════*/
	private async Task<string?> GetChannelTrailer(string userId)
	{
		var parameters = new GetVideosRequestParameters
		{
			BroadcasterId = userId,
			ItemsPerPage = "20",
			VideoType = VideoType.Upload
		};
		var channelTrailer = await TwitchApiRequest.GetVideos(parameters);
		return channelTrailer.Data.Count == 0
			? null
			: channelTrailer.Data
				.Where(t => t.Title.Contains("Channel Trailer"))
				.Select(t => t.VideoId)
				.FirstOrDefault();
	}

	private async Task<string?> GetRecentVod(string userId)
	{
		var parameters = new GetVideosRequestParameters
		{
			BroadcasterId = userId,
			ItemsPerPage = "1",
			VideoType = VideoType.Archive
		};

		var response = await TwitchApiRequest.GetVideos(parameters);
		return response.Data.Count == 1
			? response.Data[0].Url
			: null;
	}

	private async Task<string?> GetRecentTopClip(string userId)
	{
		var dateRange = Toolbox.DateRangeOfLast30Days();

		var clipParameters = new GetClipsRequestParameters
		{
			BroadcasterId = userId,
			StartedAt = dateRange[0],
			EndedAt = dateRange[1],
			ResultsPerPage = 1
		};

		var clip = await TwitchApiRequest.GetClips(clipParameters);
		return clip.Data.Count == 1
			? clip.Data[0].ClipUrl
			: null;
	}
}