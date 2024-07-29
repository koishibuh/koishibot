using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Raids.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.Shoutouts;

public record PromoVideoService(
		IOptions<Settings> Settings,
		ITwitchApiRequest TwitchApiRequest
		//IPromoVideoApi PromoVideoApi
		) : IPromoVideoService
{
	public async Task<string?> Start(TwitchUser user)
	{
		var videoUrl = "https://player.twitch.tv/?muted=false&parent=twitch.tv&video=";

		await Task.Delay(300);


		var parameters = new GetVideosRequestParameters
		{
			BroadcasterId = user.TwitchId,
			ItemsPerPage = "20",
			VideoType = VideoType.Upload
		};
		var channelTrailer = await TwitchApiRequest.GetVideos(parameters);
		var channelTrailerResult = channelTrailer.FindChannelTrailerUrl();
		if (channelTrailerResult is not null) { return $"{videoUrl}{channelTrailer}"; }

		await Task.Delay(300);

		var vodparameters = new GetVideosRequestParameters
		{
			BroadcasterId = user.TwitchId,
			ItemsPerPage = "1",
			VideoType = VideoType.Archive
		};

		var recentVod = await TwitchApiRequest.GetVideos(vodparameters);
		if (recentVod is not null) { return $"{videoUrl}{recentVod}"; }

		await Task.Delay(300);

		var dateRange = Toolbox.DateRangeOfLast30Days();
		
		
		var clipParameters = new GetClipsRequestParameters
		{
			BroadcasterId = user.TwitchId,
			StartedAt = dateRange[0],
			EndedAt = dateRange[1],
			ResultsPerPage = 1
		};

		var clip = await TwitchApiRequest.GetClips(clipParameters);
		var clipresult = clip.GetTopClip();

		if (clipresult is not null) { return clipresult; }



		return null;
	}


}