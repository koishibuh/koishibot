using Koishibot.Core.Features.AdBreak.Enums;
using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Interfaces;
using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Services.Twitch.EventSubs.AdBreak;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelad_breakbegin">Channel Ad Break Begin</see>
/// </summary>
/// <exception cref="NotImplementedException"></exception>
public record AdBreakStartedHandler(
	IOptions<Settings> Settings,
	IAppCache Cache, 
	IChatReplyService ChatReplyService,
	IPomodoroTimer PomodoroService,
	ITwitchApiRequest TwitchApiRequest,
	ISignalrService Signalr
	) : IRequestHandler<AdBreakStartedCommand>
{
	public string StreamerId = Settings.Value.StreamerTokens.UserId;

	public async Task Handle
		(AdBreakStartedCommand command, CancellationToken cancel)
	{
		await UpdateOverlayTimer(command);
		// TODO: Post to UI Add started?

		PomodoroService.CancelTimer();

		await ChatReplyService.App(Command.AdNowPlaying);

		var adInfo = await GetAdScheduleFromTwitch(command);

		await Signalr.SendInfo($"Ad started, delaying for {adInfo.AdDurationInSeconds}");
		await Task.Delay(adInfo.AdDurationInSeconds);

		await ChatReplyService.App(Command.AdCompleted);

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

	// == ⚫ == //

	public async Task UpdateOverlayTimer(AdBreakStartedCommand command)
	{
		var adBreakVm = new AdBreakVm(command.args.DurationInSeconds, DateTime.Now);
		await Signalr.SendAdStartedEvent(adBreakVm);
	}

	public async Task<AdScheduleDto> GetAdScheduleFromTwitch(AdBreakStartedCommand command)
	{
		var parameters = command.CreateParameters(StreamerId);
		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		return result.ConvertToDto();
	}
}

// == ⚫ COMMAND == //

public record AdBreakStartedCommand(AdBreakBeginEvent args) : IRequest
{
	public GetAdScheduleRequestParameters CreateParameters(string streamerId)
		=> new GetAdScheduleRequestParameters { BroadcasterId = streamerId };
};