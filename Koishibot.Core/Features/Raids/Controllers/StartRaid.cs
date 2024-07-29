//using Koishibot.Core.Features.AdBreak.Extensions;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.Obs.Interfaces;
//using Koishibot.Core.Features.Raids.Enums;
//using Koishibot.Core.Features.Raids.Interfaces;
//using Koishibot.Core.Features.RaidSuggestions.Extensions;
//using Koishibot.Core.Services.Twitch.Irc;
//using Koishibot.Core.Services.TwitchApi.Models;

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
//	IOptions<Settings> Settings,
//		IAppCache Cache, IObsService ObsService,
//		IRaidApi RaidApi,
//		ITwitchApiRequest TwitchApiRequest,
//		ITwitchIrcService BotIrc, ISignalrService Signalr
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


//		var parameters = new StartRaidRequestParameters
//		{
//			ReceivingBroadcasterId = raidTarget.Streamer.TwitchId,
//			SendingBroadcasterId = Settings.Value.StreamerTokens.UserId
//		};

//		await TwitchApiRequest.StartRaid(parameters);

//		var timer = new CurrentTimer();
//		timer.SetEndingStream();
//		Cache.AddCurrentTimer(timer);

//		var timerVm = timer.ConvertToVm();
//		await Signalr.UpdateTimerOverlay(timerVm);

//		// TODO: This eventually needs to use the info we have for next week of streams
//		var streamParameters = new GetChannelStreamScheduleRequestParameters
//		{
//			BroadcasterId = Settings.Value.StreamerTokens.UserId,
//			First = 1
//		};
//		var nextStreamDate = await TwitchApiRequest.GetStreamSchedule(streamParameters);

//		var title = $"We raided @{raidTarget.Streamer.Name}! " +
//				$"See you next stream on {nextStreamDate} at 1PM UK / 8AM EST";


//		var channelParameters = new EditChannelInfoRequestParameters
//		{
//			BroadcasterId = Settings.Value.StreamerTokens.UserId
//		};
//		var channelBody = new EditChannelInfoRequestBody
//		{
//			Title = title
//		};

//		await TwitchApiRequest.EditChannelInfo(channelParameters, channelBody);

//	}
//}