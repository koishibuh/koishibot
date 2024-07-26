//using Koishibot.Core.Features.AttendanceLog.Extensions;
//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.Obs.Interfaces;
//using Koishibot.Core.Features.RaidSuggestions.Extensions;
//using Koishibot.Core.Features.StreamInformation.Extensions;
//using Koishibot.Core.Features.StreamInformation.Interfaces;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Persistence.Cache.Enums;
//using TwitchLib.Api.Core.Enums;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Stream;
//namespace Koishibot.Core.Features.StreamInformation;

//// == ⚫ EVENT SUB == //

//public class StreamOffline(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//	) : IStreamOffline
//{
//	public async Task SetupMethod()
//	{
//		EventSubClient.StreamOffline += OnStreamOffline;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster("stream.offline", "1", Settings);
//	}

//	private async Task OnStreamOffline(object sender, StreamOfflineArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new StreamOfflineCommand());
//	}
//}

//// == ⚫ COMMAND == //

//public record StreamOfflineCommand() : IRequest;

//// == ⚫ HANDLER == //

///// <summary>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamoffline">Stream Offline EventSub Documentation</see></para>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record StreamOfflineHandler(
//	IOptions<Settings> Settings,
//	IObsService ObsService, IAppCache Cache, IStreamInfoApi TwitchApi,
//	KoishibotDbContext Database, IChatMessageService BotIrc
//	) : IRequestHandler<StreamOfflineCommand>
//{
//	public string StreamerId => Settings.Value.StreamerTokens.UserId;

//	public async Task Handle(StreamOfflineCommand command, CancellationToken cancel)
//	{
//		await Cache.UpdateServiceStatus(ServiceName.StreamOnline, false);

//		await ObsService.StopWebsocket();

//		var todaysStream = Cache.GetCurrentTwitchStream();

//		var result = await TwitchApi.GetRecentVod(StreamerId);
//		if (result is not null && result.VideoId == todaysStream.StreamId)
//		{
//			todaysStream.ConvertStringToTimeSpan(result.Duration);
//			await Database.AddStream(todaysStream);
//		}
//		else
//		{
//			todaysStream.CalculateStreamDuration();
//			await Database.AddStream(todaysStream);
//		}

//		// Todo: Clear Stream Session from Cache?
//		// Todo: Disable any channel points

//		Cache
//			.ClearAttendanceCache()
//			.ClearRaidSuggestions();

//		// Clear timer?

//		await BotIrc.BotSend("Stream is over, thanks for hanging out!");
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record StreamInfoApi : IStreamInfoApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-videos">Get Videos Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<RecentVod?> GetRecentVod(string userId)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Videos.GetVideosAsync
//			(userId: userId, first: 1, type: VideoType.Archive);

//		return result.FindRecentVod();
//	}
//}