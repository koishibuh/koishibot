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
ITwitchApiRequest TwitchApiRequest,
IOptions<Settings> Settings,
KoishibotDbContext Database,
IAppCache Cache
) : IStreamSessionService
{
	public async Task CreateOrReloadStreamSession()
	{
		var parameters = new GetLiveStreamsRequestParameters { UserLogins = [ "elysiagriffin" ] };

		var streamInfo = await TwitchApiRequest.GetLiveStreams(parameters);
		if (streamInfo.Data is null || streamInfo.Data.Count == 0)
		{
			// Todo: Display something on the client that it was unable to get the stream info
			Log.LogError("Stream info not found at startup");
			return;
		}

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