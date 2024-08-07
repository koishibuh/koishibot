﻿using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Obs.Interfaces;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.StreamInformation.Extensions;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.StreamInformation;

// == ⚫ COMMAND == //

public record StreamOfflineCommand() : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamoffline">Stream Offline EventSub Documentation</see></para>
/// </summary>
public record StreamOfflineHandler(
	IOptions<Settings> Settings,
	IObsService ObsService, IAppCache Cache, ITwitchApiRequest TwitchApiRequest,
	KoishibotDbContext Database, ITwitchIrcService BotIrc
	) : IRequestHandler<StreamOfflineCommand>
{
	public string StreamerId => Settings.Value.StreamerTokens.UserId;

	public async Task Handle(StreamOfflineCommand command, CancellationToken cancel)
	{
		await Cache.UpdateServiceStatus(ServiceName.StreamOnline, false);

		await ObsService.StopWebsocket();

		var todaysStream = Cache.GetCurrentTwitchStream();

		var parameters = new GetVideosRequestParameters
		{
			BroadcasterId = StreamerId,
			ItemsPerPage = "1",
			VideoType = VideoType.Archive
		};

		var response = await TwitchApiRequest.GetVideos(parameters);
		// Check if this can be null or empty

		if (response.Data.Count > 0 && response.Data[0].VideoId == todaysStream.StreamId)
		{
			todaysStream.Duration = response.Data[0].Duration;
		}
		else
		{
			todaysStream.CalculateStreamDuration();
		}

			await Database.AddStream(todaysStream);

		// Todo: Clear Stream Session from Cache?
		// Todo: Disable any channel points

		Cache
			.ClearAttendanceCache()
			.ClearRaidSuggestions();

		// Clear timer?

		await BotIrc.BotSend("Stream is over, thanks for hanging out!");
	}
}