using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.StreamInformation;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamoffline">Stream Offline EventSub Documentation</see></para>
/// </summary>
public record StreamOfflineHandler(
	IOptions<Settings> Settings,
	IAppCache Cache, 
	ITwitchApiRequest TwitchApiRequest,
	KoishibotDbContext Database, ITwitchIrcService BotIrc
	) : IRequestHandler<StreamOfflineCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle(StreamOfflineCommand command, CancellationToken cancel)
	{
		// Find stream session, set end

		await Cache.UpdateServiceStatus(ServiceName.StreamOnline, Status.Offline);

		//await ObsService.StopWebsocket();

		var lastStream = await Database.GetLastStream();

		lastStream.EndedAt = DateTimeOffset.UtcNow;
		await Database.UpdateEntry(lastStream);

		// var streamSessionId = Cache.GetCurrentStreamSessionId();
		//
		var streamSession = await Database.StreamSessions
			.OrderByDescending(x => x.Id)
			.FirstOrDefaultAsync();

		var liveStreams = await Database.LiveStreams
			.Where(x => x.StreamSessionId == streamSession.Id)
			.OrderBy(x => x.StartedAt)
			.ToListAsync(cancellationToken: cancel);

		streamSession.Duration = (liveStreams.Last().EndedAt ?? DateTimeOffset.UtcNow) - liveStreams.First().StartedAt;
		await Database.UpdateEntry(streamSession);

		// Todo: Clear Stream Session from Cache?
		// Todo: Disable any channel points

		Cache
			.ClearAttendanceCache()
			.ClearRaidSuggestions();

		// Clear timer?

		await BotIrc.BotSend("Stream is over, thanks for hanging out!");
	}
}

// == ⚫ COMMAND == //

public record StreamOfflineCommand() : IRequest
{
	public GetVideosRequestParameters CreateParameters(string streamerId)
	{
		return new GetVideosRequestParameters
		{
			BroadcasterId = streamerId,
			ItemsPerPage = "1",
			VideoType = VideoType.Archive
		};
	}
}