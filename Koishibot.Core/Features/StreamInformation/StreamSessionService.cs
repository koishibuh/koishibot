//using Koishibot.Core.Features.AttendanceLog.Extensions;
//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.StreamInformation.Extensions;
//using Koishibot.Core.Features.StreamInformation.Interfaces;
//using Koishibot.Core.Features.StreamInformation.Models;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Persistence.Cache.Enums;
//using TwitchLib.Api.Helix.Models.Streams.GetStreams;
//namespace Koishibot.Core.Features.StreamInformation;

//public record StreamSessionService(
//	ILogger<StreamSessionService> Log,
//	IStreamInfoApi TwitchApi, IOptions<Settings> Settings,
//	KoishibotDbContext Database,
//	IAppCache Cache
//	) : IStreamSessionService
//{
//	public string UserId => Settings.Value.StreamerTokens.UserId;

//	public async Task CreateOrReloadStreamSession()
//	{
//		var streamInfo = await TwitchApi.GetLiveStream(UserId);
//		if (streamInfo is null)
//		{
//			// Todo: Display something on the client that it was unable to get the stream info
//			Log.LogError("Stream info not found at startup");
//			return;
//		}

//		var sessionRepo = await Database.GetSessionByTwitchId(streamInfo.StreamId);
//		if (sessionRepo is null)
//		{
//			await RecordNewSession(streamInfo);
//		}
//		else
//		{
//			await ReloadCurrentSession(sessionRepo);
//		}
//	}


//	// == ⚫ == //

//	public async Task RecordNewSession(LiveStreamInfo streamInfo)
//	{
//		var yearlyQuarter = await Database.GetYearlyQuarter();
//		if (yearlyQuarter is null || yearlyQuarter.EndOfQuarter() is true)
//		{
//			yearlyQuarter = new YearlyQuarter().Initialize();

//			await Database.AddYearlyQuarter(yearlyQuarter);
//			await Database.ResetAttendanceStreaks();
//		}

//		var attendanceStatus = Cache.GetStatusByServiceName(ServiceName.Attendance);

//		var twitchStream = streamInfo.CreateTwitchStream(attendanceStatus, yearlyQuarter);
//		await Database.AddStream(twitchStream);

//		var lastStreamDate = await Database.GetLastMandatoryStreamDate();
//		var streamSessions = new StreamSessions(twitchStream, lastStreamDate);

//		Cache.AddStreamSessions(streamSessions);
//	}

//	public async Task ReloadCurrentSession(TwitchStream stream)
//	{
//		await Cache.UpdateServiceStatus(ServiceName.Attendance, stream.AttendanceMandatory);

//		var lastStreamDate = await Database.GetLastMandatoryStreamDate();
//		var streamSessions = new StreamSessions(stream, lastStreamDate);

//		Cache.AddStreamSessions(streamSessions);
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record StreamInfoApi : IStreamInfoApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-streams">Get Streams Documentation</see><br/>
//	/// Id, UserId, UserLogin, UserName, GameId, GameName <br/>
//	/// Type, Title, Tags, ViewerCount, StartedAt, Language<br/>
//	/// ThumbnailUrl, TagIds, IsMature<br/>
//	/// If stream is offline, streams will return null.
//	/// </summary>
//	/// <returns></returns>
//	public async Task<LiveStreamInfo?> GetLiveStream(string streamerId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var retryCount = 0;

//		var result = await TwitchApi.GetLiveStream(streamerId);
//		while (result is null && retryCount < 3)
//		{
//			var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
//			await Task.Delay(delay);
//			retryCount++;

//			result = await TwitchApi.GetLiveStream(streamerId);
//		}

//		if (result is null || result.Streams.Length == 0)
//		{
//			Log.LogError("Stream is offline");
//			return null;
//		}

//		return result.ConvertToDto();
//	}
//}

//public static class TwitchApiExtensions
//{
//	public static async Task<GetStreamsResponse?> GetLiveStream(this ITwitchAPI TwitchApi, string streamerId)
//	{
//		var streamerIds = new List<string> { streamerId };
//		return await TwitchApi.Helix.Streams.GetStreamsAsync(first: 1, userIds: streamerIds);
//	}
//}