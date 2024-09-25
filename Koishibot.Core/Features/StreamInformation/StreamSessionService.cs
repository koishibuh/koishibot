using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.StreamInformation;

public record StreamSessionService(
ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database,
IAppCache Cache,
IOptions<Settings> Settings
) : IStreamSessionService
{
	public async Task CreateOrReloadStreamSession()
	{
		var currentVod = await GetCurrentVodTwitch();
		var lastLiveStreamDb = await Database.GetLastStream();

		// New database
		if (lastLiveStreamDb is null)
		{
			var streamSessionId = await CreateNewSession();
			var liveStreamId = await CreateNewLiveStream(currentVod, streamSessionId);
			await AddCurrentSessionToCache(liveStreamId, streamSessionId);
			return;
		}

		// If crash occured but stream still valid with disconnect protection (90 sec)
		if (lastLiveStreamDb.TwitchId == currentVod.VideoId)
		{
			var streamSession = await Database.GetRecentStreamSession();
			if (streamSession is null) throw new CustomException("StreamSession is null");

			await AddCurrentSessionToCache(lastLiveStreamDb.Id, streamSession.Id);

			await UpdateAttendanceServiceStatus(streamSession);
			return;
		}

		// If crash occured and bot missed stream end event to record duration
		if (lastLiveStreamDb.EndedAt is null)
		{
			var lastLiveStreamTwitch = new GetVideosRequestParameters { VideoIds = [currentVod.VideoId] };

			var lastVodResponse = await TwitchApiRequest.GetVideos(lastLiveStreamTwitch);
			if (lastVodResponse.Data.Count == 0)
				throw new Exception("Unable to get last livestream VOD from Twitch");

			lastLiveStreamDb.EndedAt = lastLiveStreamDb.StartedAt + lastVodResponse.Data[0].Duration;
			await Database.UpdateEntry(lastLiveStreamDb);
		}

		// Check if 20 minutes has elapsed since last stream
		if (lastLiveStreamDb.GracePeriodElapsed(20, currentVod.CreatedAt))
		{
			var streamSessionId = await CreateNewSession();
			var liveStreamId = await CreateNewLiveStream(currentVod, streamSessionId);
			await AddCurrentSessionToCache(liveStreamId, streamSessionId);
		}
		else
		{
			// stream crashed, new livestream created & 20 minutes has not elapsed
			var streamSession = await Database.GetRecentStreamSession();
			if (streamSession is null) throw new CustomException("StreamSession is null");

			var liveStreamId = await CreateNewLiveStream(currentVod, streamSession.Id);

			await AddCurrentSessionToCache(liveStreamId, streamSession.Id);
			await UpdateAttendanceServiceStatus(streamSession);
		}
	}

	/*═════════◣ ◢═════════*/
	private async Task<VideoData> GetCurrentVodTwitch()
	{
		var videoParameters = new GetVideosRequestParameters
		{
		BroadcasterId = Settings.Value.StreamerTokens.UserId,
		ItemsPerPage = "1",
		VideoType = VideoType.Archive
		};

		var response = await TwitchApiRequest.GetVideos(videoParameters);
		if (response.Data.Count == 0)
			throw new Exception("Unable to get last VOD from Twitch");

		return response.Data[0];
	}

	private async Task<int> GetYearlyQuarterId()
	{
		var yearlyQuarter = await Database.GetYearlyQuarter();
		if (yearlyQuarter is null || yearlyQuarter.EndOfQuarter())
		{
			yearlyQuarter = new YearlyQuarter().Initialize();

			await Database.AddYearlyQuarter(yearlyQuarter);
			await Database.ResetAttendanceStreaks();
		}

		return yearlyQuarter.Id;
	}

	private async Task<int> CreateNewSession()
	{
		var yearlyQuarterId = await GetYearlyQuarterId();
		var attendanceStatus = Cache.GetStatusByServiceName(ServiceName.Attendance);

		var streamSession = new StreamSession
		{
		Duration = TimeSpan.Zero,
		AttendanceMandatory = attendanceStatus,
		YearlyQuarterId = yearlyQuarterId
		};

		return await Database.UpdateEntry(streamSession);
	}

	private async Task<int> CreateNewLiveStream(VideoData video, int streamSessionId)
	{
		var liveStream = new LiveStream
		{
		StartedAt = video.CreatedAt,
		StreamSessionId = streamSessionId,
		TwitchId = video.VideoId
		};

		return await Database.UpdateEntry(liveStream);
	}

	private async Task AddCurrentSessionToCache(int liveStreamId, int streamSessionId)
	{
		var lastMandatoryStreamId = await Database.GetLastMandatorySessionId();

		var cacheSession = new CurrentSession
		{
		LiveStreamId = liveStreamId,
		StreamSessionId = streamSessionId,
		LastMandatorySessionId = lastMandatoryStreamId
		};

		Cache.Add(CacheName.CurrentSession, cacheSession);
	}

	private async Task UpdateAttendanceServiceStatus(StreamSession streamSession)
	{
		var status = streamSession.AttendanceMandatory
		? Status.Online
		: Status.Offline;

		await Cache.UpdateServiceStatus(ServiceName.Attendance, status);
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IStreamSessionService
{
	Task CreateOrReloadStreamSession();
}