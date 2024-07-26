using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Interfaces;
using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.AdBreak;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Events;

// == ⚫ COMMAND == //

public record AdBreakStartedCommand(AdBreakBeginEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelad_breakbegin">Channel Ad Break Begin</see>
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
/// <returns></returns>
/// <exception cref="NotImplementedException"></exception>
public record AdBreakStartedHandler(
	IOptions<Settings> Settings,
	ILogger<AdBreakStartedHandler> Log,
	IAppCache Cache, ITwitchIrcService BotIrc,
	IPomodoroTimer PomodoroService,
	//IAdsApi TwitchApi,
	ITwitchApiRequest TwitchApiRequest,
	ISignalrService Signalr
	) : IRequestHandler<AdBreakStartedCommand>
{
	public async Task Handle
		(AdBreakStartedCommand command, CancellationToken cancel)
	{
		Log.LogInformation("Ad started");

		await Signalr.SendAdStartedEvent(new AdBreakVm(command.args.DurationInSeconds, DateTime.Now));
		// TODO: Post to UI Add started?

		PomodoroService.CancelTimer();

		await BotIrc.AdsStarted();

		var parameters = new GetAdScheduleRequestParameters
			{ BroadcasterId = Settings.Value.StreamerTokens.UserId };

		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		var adInfo2 = result.ConvertToDto();

		Log.LogInformation($"Ad started, delaying for {adInfo2.AdDurationInSeconds}");
		await Task.Delay(adInfo2.AdDurationInSeconds);


		var currentTimer2 = Cache.GetCurrentTimer();
		if (currentTimer2.TimerExpired())
		{
			await PomodoroService.StartTimer(adInfo2);
		}
		else
		{
			await Task.Delay(currentTimer2.TimeRemaining());
			await PomodoroService.StartTimer(adInfo2);
		}
	}
}

// == ⚫ CHAT RESPONSE == //

public static class AdBreakChatReply
{
	public static async Task AdsStarted(this ITwitchIrcService irc)
	{
		var message = "Ads now playing, RIP";
		await irc.BotSend(message);
	}

	public static async Task AdsFinished(this ITwitchIrcService irc)
	{
		var message = "Ad break done! Thank you for supporting the channel <3";
		await irc.BotSend(message);
	}
}