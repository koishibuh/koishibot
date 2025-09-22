using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.StreamInformation;

public record StreamSessionService(
ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database,
IAppCache Cache,
IOptions<Settings> Settings,
ILogger<StreamSessionService> Log
) : IStreamSessionService
{
	/// <summary>
	/// Queries Twitch for LiveStream to get StreamId & StartedAt
	/// </summary>
	public async Task Reconnect()
	{
		var stream = await GetCurrentStreamTwitch();
		await CreateOrReloadStreamSession(stream.StreamId, stream.StartedAt);
	}
	
	public async Task CreateOrReloadStreamSession(string streamId, DateTimeOffset startedAt)
	{
		StreamSession? streamSession;

		var lastLiveStreamDb = await Database.GetLastStream();
		if (lastLiveStreamDb is null) // First recorded stream, new database
		{
			streamSession = await CreateNewSession();
			var recentVideoFromTwitch = await GetRecentVideoFromTwitch();

			var liveStream = new LiveStream
			{
				TwitchId = streamId,
				StreamSessionId = streamSession.Id,
				VideoId = GetVideoId(recentVideoFromTwitch, streamId),
				StartedAt = startedAt,
				EndedAt = null
			};

			await Database.UpdateEntry(liveStream);
			await AddCurrentSessionToCache(liveStream.Id, streamSession.Id, streamSession.Summary);
			return;
		}

		// If crash occured but stream still valid with disconnect protection (90s)
		if (lastLiveStreamDb.IsCurrentStream(streamId))
		{
			Log.LogError("StreamId already logged in database");
			streamSession = await Database.GetRecentStreamSession() ?? await CreateNewSession();

			await AddCurrentSessionToCache(lastLiveStreamDb.Id, streamSession.Id, streamSession.Summary);
			await UpdateAttendanceServiceStatus(streamSession);
			return;
		}

		// If crash occured and bot missed stream end event to record duration
		if (lastLiveStreamDb.EndedAt is null)
		{
			var lastLiveStreamTwitch = await GetVideoByIdFromTwitch(lastLiveStreamDb.TwitchId);
			if (lastLiveStreamTwitch is null)
			{
				streamSession = await CreateNewSession();
				var currentVod = await GetRecentVideoFromTwitch();

				var liveStream = new LiveStream
				{
					TwitchId = streamId,
					StreamSessionId = streamSession.Id,
					VideoId = GetVideoId(currentVod, streamId),
					StartedAt = startedAt,
					EndedAt = null
				};

				await Database.UpdateEntry(liveStream);
				await AddCurrentSessionToCache(lastLiveStreamDb.Id, streamSession.Id, streamSession.Summary);
				await UpdateAttendanceServiceStatus(streamSession);
			}
			else
			{
				lastLiveStreamDb.EndedAt = lastLiveStreamDb.StartedAt + lastLiveStreamTwitch.Duration;
				await Database.UpdateEntry(lastLiveStreamDb);

				await DetermineIfElapsed(lastLiveStreamDb, startedAt, streamId);
			}
		}
		else
		{
			await DetermineIfElapsed(lastLiveStreamDb, startedAt, streamId);
		}
	}

	/*═════════◣ ◢═════════*/
	private async Task DetermineIfElapsed(LiveStream lastLiveStreamDb, DateTimeOffset startedAt, string streamId)
	{
		StreamSession? streamSession;

		if (lastLiveStreamDb.GracePeriodElapsed(20, startedAt))
		{
			streamSession = await CreateNewSession();
		}
		else
		{
			streamSession = await Database.GetRecentStreamSession();
			if (streamSession is null)
			{
				Log.LogError("Stream session was null");
				streamSession = await CreateNewSession();
			}
		}

		var currentVod = await GetRecentVideoFromTwitch();

		var liveStream = new LiveStream
		{
			TwitchId = streamId,
			StreamSessionId = streamSession.Id,
			VideoId = GetVideoId(currentVod, streamId),
			StartedAt = startedAt,
			EndedAt = null
		};

		await Database.UpdateEntry(liveStream);
		await AddCurrentSessionToCache(lastLiveStreamDb.Id, streamSession.Id, streamSession.Summary);
		await UpdateAttendanceServiceStatus(streamSession);
	}

	private string? GetVideoId(VideoData? video, string streamId) =>
		(video is null || video.StreamId != streamId) ? null : video.VideoId;

	private async Task<LivestreamData> GetCurrentStreamTwitch()
	{
		var parameters = new GetLiveStreamsRequestParameters { UserLogins = [Settings.Value.StreamerTokens.UserLogin] };
		const int maxRetries = 3;
		var delay = 1000;

		for (var attempt = 1; attempt <= maxRetries; attempt++)
		{
			var streamInfo = await TwitchApiRequest.GetLiveStreams(parameters);

			if (streamInfo.Data is not null && streamInfo.Data.Count == 1)
			{
				return streamInfo.Data[0];
			}

			if (attempt < maxRetries)
			{
				await Task.Delay(delay);
				delay *= 2;
			}
			else
			{
				Log.LogError("Unable to get stream info, stream offline.");
			}
		}

		throw new CustomException("Unable to get stream info");
	}

	private async Task<VideoData?> GetRecentVideoFromTwitch()
	{
		var videoParameters = new GetVideosRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			ItemsPerPage = "1",
			VideoType = VideoType.Archive
		};

		var response = await TwitchApiRequest.GetVideos(videoParameters);

		// Could be zero if videos are unpublished or expired
		return response.Data.Count == 0
			? null
			: response.Data[0];
	}

	private async Task<VideoData?> GetVideoByIdFromTwitch(string streamId)
	{
		var videoParameters = new GetVideosRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			VideoType = VideoType.Archive
		};

		var response = await TwitchApiRequest.GetVideos(videoParameters);

		// Could be zero if videos are unpublished or expired
		return response.Data.Count == 0
			? null
			: response.Data.FirstOrDefault(x => x.StreamId == streamId);
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

	private async Task<StreamSession> CreateNewSession()
	{
		var yearlyQuarterId = await GetYearlyQuarterId();
		var attendanceStatus = Cache.GetStatusByServiceName(ServiceName.Attendance);

		var streamSession = new StreamSession
		{
			Duration = TimeSpan.Zero,
			AttendanceMandatory = attendanceStatus,
			YearlyQuarterId = yearlyQuarterId,
			Summary = ""
		};

		return await Database.UpdateEntryReturn(streamSession);
	}

	private async Task AddCurrentSessionToCache(int liveStreamId, int streamSessionId, string summary)
	{
		var lastMandatoryStreamId = await Database.GetLastMandatorySessionId(streamSessionId);

		var cacheSession = new CurrentSession
		{
			LiveStreamId = liveStreamId,
			StreamSessionId = streamSessionId,
			LastMandatorySessionId = lastMandatoryStreamId,
			Summary = summary
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
	Task Reconnect();
	Task CreateOrReloadStreamSession(string streamId, DateTimeOffset startedAt);
}