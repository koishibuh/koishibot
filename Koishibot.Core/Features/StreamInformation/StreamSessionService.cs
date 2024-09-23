using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChatCommands.Extensions;
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
IAppCache Cache,
ILogger<StreamSessionService> Log
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

			if (streamInfo.Data is not null && streamInfo.Data.Count == 1)
			{
				var stream = streamInfo.Data[0];
				// var liveStreamInfo = stream.ConvertToDto();

				// check if Id exists
				var storedLiveStream = await Database.LiveStreams
				.FirstOrDefaultAsync(x => x.TwitchId == stream.VideoId);

				if (storedLiveStream is null)
				{
					// create new livestream
					storedLiveStream = new LiveStream
					{
					TwitchId = stream.VideoId,
					StartedAt = stream.StartedAt,
					EndedAt = null
					};

					storedLiveStream = await Database.UpdateEntryReturn(storedLiveStream);

					// has it been 20 minutes since last livestream?
					// Get previous livestream entry
					var lastLiveStream = await Database.LiveStreams
					.OrderByDescending(x => x.Id)
					.Skip(1)
					.FirstOrDefaultAsync();

					// has been 20  minutes, create stream session
					if (lastLiveStream is null ||
					    lastLiveStream.EndedAt + TimeSpan.FromMinutes(20) < DateTimeOffset.UtcNow)
					{
						await RecordNewSession(storedLiveStream);
					}
					else
					{
						await ReloadCurrentSession(storedLiveStream);
					}
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
				Log.LogError("Unable to get stream info, stream offline.");
			}
		}
	}

	/*═══════════◣ NEW SESSION ◢═══════════*/
	public async Task RecordNewSession(LiveStream stream)
	{
		var yearlyQuarter = await Database.GetYearlyQuarter();
		if (yearlyQuarter is null || yearlyQuarter.EndOfQuarter())
		{
			yearlyQuarter = new YearlyQuarter().Initialize();

			await Database.AddYearlyQuarter(yearlyQuarter);
			await Database.ResetAttendanceStreaks();
		}

		var attendanceStatus = Cache.GetStatusByServiceName(ServiceName.Attendance);

		var streamSession = new StreamSession
		{
		Duration = TimeSpan.Zero,
		AttendanceMandatory = attendanceStatus,
		YearlyQuarterId = yearlyQuarter.Id
		};

		streamSession.LiveStreams.Add(stream);
		streamSession = await Database.UpdateEntryReturn(streamSession);

		var lastMandatoryStreamId = await Database.GetLastMandatorySessionId();
		var cacheSession = new CurrentSession
		{
		LiveStreamId = stream.Id,
		StreamSessionId = streamSession.Id,
		LastMandatorySessionId = lastMandatoryStreamId
		};

		Cache.Add(CacheName.CurrentSession, cacheSession);
	}

	/*═════════◣ CURRENT SESSION ◢═════════*/
	public async Task ReloadCurrentSession(LiveStream stream)
	{
		// use old stream session
		var streamSession = await Database.StreamSessions
		.OrderByDescending(x => x.Id)
		.FirstOrDefaultAsync();

		streamSession.LiveStreams.Add(stream);
		await Database.SaveChangesAsync();
		var lastMandatoryStreamId = await Database.GetLastMandatorySessionId();

		var cacheSession = new CurrentSession
		{
		LiveStreamId = stream.Id,
		StreamSessionId = streamSession.Id,
		LastMandatorySessionId = lastMandatoryStreamId
		};

		Cache.Add(CacheName.CurrentSession, cacheSession);

		var status = streamSession.AttendanceMandatory
		? Status.Online
		: Status.Offline;

		await Cache.UpdateServiceStatus(ServiceName.Attendance, status);
	}
}