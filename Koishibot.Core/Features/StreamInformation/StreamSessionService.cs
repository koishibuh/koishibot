using Koishibot.Core.Exceptions;
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
ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database,
IAppCache Cache
) : IStreamSessionService
{
	public async Task CreateOrReloadStreamSession()
	{
		var parameters = new GetLiveStreamsRequestParameters { UserLogins = ["elysiagriffin"] };

		const int maxRetries = 3;
		var delay = 1000;

		for (var attempt = 1; attempt <= maxRetries; attempt++)
		{
			var streamInfo = await TwitchApiRequest.GetLiveStreams(parameters);

			if (streamInfo.Data is not null)
			{
				var stream = streamInfo.Data[0];
				var liveStreamInfo = stream.ConvertToDto();

				var sessionRepo = await Database.GetSessionByTwitchId(liveStreamInfo.StreamId);
				if (sessionRepo is null)
				{
					await RecordNewSession(liveStreamInfo);
				}
				else
				{
					await ReloadCurrentSession(sessionRepo);
				}

				return;
			}

			if (attempt < maxRetries)
			{
				await Task.Delay(delay);
				delay *= 2;
			}
			else
			{
				var liveStreamInfo = new LiveStreamInfo("", "", "", "", "", 0, DateTimeOffset.UtcNow, "");
				await RecordNewSession(liveStreamInfo);
				throw new CustomException("Was not able to get stream info");
			}
		}
	}

	/*═══════════◣ NEW SESSION ◢═══════════*/
	public async Task RecordNewSession(LiveStreamInfo streamInfo)
	{
		var yearlyQuarter = await Database.GetYearlyQuarter();
		if (yearlyQuarter is null || yearlyQuarter.EndOfQuarter())
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

	/*═════════◣ CURRENT SESSION ◢═════════*/
	public async Task ReloadCurrentSession(TwitchStream stream)
	{
		var status = stream.AttendanceMandatory
		? ServiceStatusString.Online
		: ServiceStatusString.Offline;

		await Cache.UpdateServiceStatus(ServiceName.Attendance, status);

		var lastStreamDate = await Database.GetLastMandatoryStreamDate();
		var streamSessions = new StreamSessions(stream, lastStreamDate);

		Cache.AddStreamSessions(streamSessions);
	}
}