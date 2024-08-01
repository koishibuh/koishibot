using Koishibot.Core.Features.AdBreak.Enums;
using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Interfaces;
using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Services.Twitch.EventSubs.AdBreak;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Events;

// == ⚫ COMMAND == //

public record AdBreakStartedCommand(AdBreakBeginEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelad_breakbegin">Channel Ad Break Begin</see>
/// </summary>
/// <exception cref="NotImplementedException"></exception>
public record AdBreakStartedHandler(
	IOptions<Settings> Settings,
	ILogger<AdBreakStartedHandler> Log,
	IAppCache Cache, IChatReplyService ChatReplyService,
	IPomodoroTimer PomodoroService,
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

		await ChatReplyService.App(Command.AdNowPlaying);

		var parameters = CreateParameters();

		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		var adInfo2 = result.ConvertToDto();

		Log.LogInformation($"Ad started, delaying for {adInfo2.AdDurationInSeconds}");
		await Task.Delay(adInfo2.AdDurationInSeconds);

		await ChatReplyService.App(Command.AdCompleted);

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

	public GetAdScheduleRequestParameters CreateParameters()
	{
		return new GetAdScheduleRequestParameters
		{ BroadcasterId = Settings.Value.StreamerTokens.UserId };
	}
}