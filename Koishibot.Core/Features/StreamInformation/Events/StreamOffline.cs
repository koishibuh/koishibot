using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.StreamInformation;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
///   <para>
///     <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamoffline">
///       Stream Offline EventSub
///       Documentation
///     </see>
///   </para>
/// </summary>
public record StreamOfflineHandler(
IOptions<Settings> Settings,
IAppCache Cache,
ITwitchApiRequest TwitchApiRequest,
KoishibotDbContext Database, ITwitchIrcService BotIrc,
ILogger<StreamOfflineHandler> Log
) : IRequestHandler<StreamOfflineCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle(StreamOfflineCommand command, CancellationToken cancel)
	{
		// await Cache.UpdateServiceStatus(ServiceName.StreamOnline, Status.Offline);

		//await ObsService.StopWebsocket();

		await UpdateLiveStreamEndedAt();
		await UpdateStreamSessionDuration();

		// Todo: Clear Stream Session from Cache?
		// Todo: Disable any channel points

		// Cache
		// 	.ClearAttendanceCache()
		// 	.ClearRaidSuggestions();
		//
		// // Clear timer?
		//
		// await BotIrc.BotSend("Stream is over, thanks for hanging out!");
	}

	private async Task UpdateLiveStreamEndedAt()
	{
		var lastStream = await Database.GetLastStream();
		if (lastStream is null)
			throw new CustomException("StreamOffline: LastStream is null");

		lastStream.EndedAt = DateTimeOffset.UtcNow;
		await Database.UpdateEntry(lastStream);
	}

	private async Task UpdateStreamSessionDuration()
	{
		var streamSession = await Database.GetRecentStreamSession();

		if (streamSession is null)
			throw new CustomException("StreamOffline: Stream Session was null");

		streamSession.ResetDuration();

		var liveStreamsFromDb = await Database.GetLiveStreamsBySessionId(streamSession.Id);
		if (liveStreamsFromDb is null)
			throw new CustomException($"StreamOffline: LiveStreams with SessionId {streamSession.Id} not found");

		GetVideosResponse? recentVideosResponse = null;
		if (liveStreamsFromDb.Any(x => x.EndedAt is null))
		{
			recentVideosResponse = await GetTodaysVideosFromTwitch();
		}

		foreach (var liveStreamDb in liveStreamsFromDb)
		{
			if (liveStreamDb.EndedAt.HasValue)
			{
				streamSession.Duration += liveStreamDb.CalculateDuration();
			}
			else
			{
				var stream = recentVideosResponse?.Data.FirstOrDefault(x => x.StreamId == liveStreamDb.TwitchId);
				if (stream is null) continue;
				liveStreamDb.EndedAt = liveStreamDb.CalculateEndedAtTime(stream.Duration);
				streamSession.Duration += stream.Duration;
			}
		}

		await Database.UpdateEntry(streamSession);
	}

	private async Task<GetVideosResponse> GetTodaysVideosFromTwitch()
	{
		var recentLiveStreams = new GetVideosRequestParameters
		{
			BroadcasterId = Settings.Value.StreamerTokens.UserId,
			TimeRange = VideoPeriod.Day
		};

		return await TwitchApiRequest.GetVideos(recentLiveStreams);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record StreamOfflineCommand : IRequest
{
	public GetVideosRequestParameters CreateParameters(string streamerId) =>
		new()
		{
			BroadcasterId = streamerId,
			ItemsPerPage = "1",
			VideoType = VideoType.Archive
		};
}