//using Koishibot.Core.Features.AdBreak.Extensions;
//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.Obs.Interfaces;
//using Koishibot.Core.Features.Raids.Enums;
//using Koishibot.Core.Features.Raids.Interfaces;
//using Koishibot.Core.Features.RaidSuggestions;
//using Koishibot.Core.Features.RaidSuggestions.Extensions;
//using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;

//namespace Koishibot.Core.Features.Raids;

//// == ⚫ POST == //

//public class StartRaidController : ApiControllerBase
//{
//	[SwaggerOperation(Tags = ["Outgoing Raid"])]
//	[HttpPost("/api/raid/twitch")]
//	public async Task<ActionResult> StartRaid([FromBody] StartRaidCommand command)
//	{
//		await Mediator.Send(command);
//		return Ok();
//	}
//}

//// == ⚫ COMMAND == //

//public record StartRaidCommand(
//		bool FirstRaid
//		) : IRequest;

//// == ⚫ HANDLER == //

//public record StartRaidHandler(
//		IAppCache Cache, IObsService ObsService,
//		IRaidApi RaidApi,
//		IChatMessageService BotIrc, ISignalrService Signalr
//		) : IRequestHandler<StartRaidCommand>
//{
//	public async Task Handle
//			(StartRaidCommand command, CancellationToken cancel)
//	{
//		var raidTarget = Cache.GetRaidTarget();
//		var streamer = raidTarget.Streamer;
//		var suggestedByName = raidTarget.SuggestedByUser.Name;

//		// TODO: Open stream in client

//		// TODO: Get the proper name for this

//		//await ObsService.ChangeScene("⭐ End");

//		var result = command.FirstRaid ? Code.RaidLink : Code.RaidAgain;
//		await BotIrc.RaidTarget(result, suggestedByName, streamer.Name);

//		await BotIrc.PostRaidCall();

//		await RaidApi.StartRaid(streamer.TwitchId);

//		var timer = new CurrentTimer();
//		timer.SetEndingStream();
//		Cache.AddCurrentTimer(timer);

//		var timerVm = timer.ConvertToVm();
//		await Signalr.UpdateTimerOverlay(timerVm);

//		// TODO: This eventually needs to use the info we have for next week of streams
//		var nextStreamDate = await RaidApi.GetNextScheduledStreamDate();

//		var title = $"We raided @{raidTarget.Streamer.Name}! " +
//				$"See you next stream on {nextStreamDate} at 1PM UK / 8AM EST";

//		await RaidApi.UpdateStreamTitle(title);
//	}
//}

//// == ⚫ TWITCH API == //

//public partial record RaidApi : IRaidApi
//{
//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#start-a-raid">Start A Raid</see></ br>
//	/// There is a 10 limit request within a 10 minute window.
//	/// </summary>
//	/// <returns></returns>
//	public async Task StartRaid(string raidingUserID)
//	{
//		await TokenProcessor.EnsureValidToken();

//		await TwitchApi.Helix.Raids.StartRaidAsync(StreamerId, raidingUserID);
//	}

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#modify-channel-information">Modify Channel Info Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task UpdateStreamTitle(string title)
//	{
//		await TokenProcessor.EnsureValidToken();

//		var update = new ModifyChannelInformationRequest { Title = title };

//		await TwitchApi.Helix.Channels.ModifyChannelInformationAsync(StreamerId, update);
//	}

//	/// <summary>
//	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-stream-schedule">Get Stream Schedule Documentation</see>
//	/// </summary>
//	/// <returns></returns>
//	public async Task<DateTime?> GetNextScheduledStreamDate()
//	{
//		await TokenProcessor.EnsureValidToken();

//		var result = await TwitchApi.Helix.Schedule.GetChannelStreamScheduleAsync(StreamerId);
//		return result is null || result.Schedule.Segments.Length == 0
//			? null
//			: result.GetStartTime();
//	}
//}