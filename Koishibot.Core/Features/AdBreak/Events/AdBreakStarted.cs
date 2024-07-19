using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Interfaces;
using Koishibot.Core.Features.AdBreak.Models;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
namespace Koishibot.Core.Features.AdBreak.Events;

// == ⚫ EVENT SUB == //

public class AdBreakStarted(
	IOptions<Settings> Settings, EventSubWebsocketClient EventSubClient,
	ITwitchAPI TwitchApi, IServiceScopeFactory ScopeFactory
	) : IAdBreakStarted
{
	public async Task SetupHandler()
	{
		EventSubClient.ChannelAdBreakBegin += OnAdBreakStarted;
		await SubToEvent();
	}

	public async Task SubToEvent()
	{
		await TwitchApi.CreateEventSubBroadcaster
				("channel.ad_break.begin", "1", Settings);
	}
	
	private async Task OnAdBreakStarted(object sender, ChannelAdBreakBeginArgs args)
	{
		if (Settings.Value.DebugMode is true) { return; }

		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		var seconds = args.Notification.Payload.Event.DurationSeconds;
		var miliseconds = seconds * 1000;

		await mediatr.Send(new AdBreakStartedCommand(miliseconds));
	}
}

// == ⚫ COMMAND == //

public record AdBreakStartedCommand(int AdLength) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelad_breakbegin">Channel Ad Break Begin</see>
/// </summary>
/// <param name="sender"></param>
/// <param name="args"></param>
/// <returns></returns>
/// <exception cref="NotImplementedException"></exception>
public record AdBreakStartedHandler(
	ILogger<AdBreakStartedHandler> Log,
	IAppCache Cache, IChatMessageService BotIrc,
	IPomodoroTimer PomodoroService, IAdsApi TwitchApi,
	ISignalrService Signalr
	) : IRequestHandler<AdBreakStartedCommand>
{
	public async Task Handle
		(AdBreakStartedCommand command, CancellationToken cancel)
	{
		Log.LogInformation("Ad started");

		await Signalr.SendAdStartedEvent(new AdBreakVm(command.AdLength, DateTime.Now));
		// TODO: Post to UI Add started?

		PomodoroService.CancelTimer();

		await BotIrc.AdsStarted();

		var adInfo = await TwitchApi.GetAdSchedule();

		Log.LogInformation($"Ad started, delaying for {adInfo.AdDurationInSeconds}");
		await Task.Delay(adInfo.AdDurationInSeconds);

		await BotIrc.AdsFinished();

		var currentTimer = Cache.GetCurrentTimer();

		if (currentTimer.TimerExpired())
		{
			await PomodoroService.StartTimer(adInfo);
		}
		else
		{
			await Task.Delay(currentTimer.TimeRemaining());
			await PomodoroService.StartTimer(adInfo);
		}
	}
}

// == ⚫ CHAT RESPONSE == //

public static class AdBreakChatReply
{
	public static async Task AdsStarted(this IChatMessageService irc)
	{
		var message = "Ads now playing, RIP";
		await irc.BotSend(message);
	}

	public static async Task AdsFinished(this IChatMessageService irc)
	{
		var message = "Ad break done! Thank you for supporting the channel <3";
		await irc.BotSend(message);
	}
}