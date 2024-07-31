using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Features.StreamInformation.Interfaces;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.StreamInformation;

public record StreamSessionService(
	ILogger<StreamSessionService> Log,
	ITwitchApiRequest TwitchApiRequest, IOptions<Settings> Settings,
	KoishibotDbContext Database,
	IAppCache Cache
	) : IStreamSessionService
{
	public string UserId => Settings.Value.StreamerTokens.UserId;

	public async Task CreateOrReloadStreamSession()
	{
		var parameters = new GetLiveStreamsRequestParameters
		{
			UserLogins = ["elysiagriffin"],
		};

		var streamInfo = await TwitchApiRequest.GetLiveStreams(parameters);
		if (streamInfo.Data.Count == 0)
		{
			// Todo: Display something on the client that it was unable to get the stream info
			Log.LogError("Stream info not found at startup");
			return;
		}

		var stream = streamInfo.Data[0];

		var liveStreamInfo = new LiveStreamInfo(
			stream.BroadcasterId,
			stream.VideoId,
			stream.CategoryId,
			stream.CategoryName,
			stream.StreamTitle,
			stream.ViewerCount,
			stream.StartedAt,
			stream.ThumbnailUrl);

		var sessionRepo = await Database.GetSessionByTwitchId(stream.VideoId);
		if (sessionRepo is null)
		{
			await RecordNewSession(liveStreamInfo);
		}
		else
		{
			await ReloadCurrentSession(sessionRepo);
		}
	}

	// == ⚫ == //

	public async Task RecordNewSession(LiveStreamInfo streamInfo)
	{
		var yearlyQuarter = await Database.GetYearlyQuarter();
		if (yearlyQuarter is null || yearlyQuarter.EndOfQuarter() is true)
		{
			yearlyQuarter = new YearlyQuarter().Initialize();

			await Database.AddYearlyQuarter(yearlyQuarter);
			await Database.ResetAttendanceStreaks();
		}

		var attendanceStatus = Cache.GetStatusByServiceName(ServiceName.Attendance);

		var twitchStream = streamInfo.CreateTwitchStream(attendanceStatus, yearlyQuarter);
		await Database.AddStream(twitchStream);

		var lastStreamDate = await Database.GetLastMandatoryStreamDate();
		var streamSessions = new StreamSessions(twitchStream, lastStreamDate);

		Cache.AddStreamSessions(streamSessions);
	}

	public async Task ReloadCurrentSession(TwitchStream stream)
	{
		await Cache.UpdateServiceStatus(ServiceName.Attendance, stream.AttendanceMandatory);

		var lastStreamDate = await Database.GetLastMandatoryStreamDate();
		var streamSessions = new StreamSessions(stream, lastStreamDate);

		Cache.AddStreamSessions(streamSessions);
	}
}