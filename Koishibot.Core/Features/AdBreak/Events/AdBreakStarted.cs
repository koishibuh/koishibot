using Koishibot.Core.Features.AdBreak.Enums;
using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Services.Twitch.EventSubs.AdBreak;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
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
	private AdScheduleDto? AdInfo { get; set; }

	public async Task Handle
		(AdBreakStartedCommand command, CancellationToken cancel)
	{
		await UpdateOverlayTimer(command);
		// TODO: Post to UI Add started?

		PomodoroService.CancelTimer();

		await ChatReplyService.App(Command.AdNowPlaying);

		AdInfo = await GetAdScheduleFromTwitch(command);

		await Signalr.SendInfo($"Ad started, delaying for {AdInfo.AdDurationInSeconds}");
		var timer = Toolbox.CreateTimer(AdInfo.AdDurationInSeconds, AdsCompleted);
		timer.Start();
	}

	private async void AdsCompleted()
	{
		await ChatReplyService.App(Command.AdCompleted);

		await PomodoroService.StartTimer(AdInfo);
	}

/*═════════◣ ◢═════════*/
	private async Task UpdateOverlayTimer(AdBreakStartedCommand command)
	{
		var totalMilliseconds = (int)command.Args.DurationInSeconds.TotalMilliseconds;
		var adBreakVm = new AdBreakVm(totalMilliseconds, DateTime.Now);
		await Signalr.SendAdStartedEvent(adBreakVm);
	}

/*═════════◣ ◢═════════*/
	private async Task<AdScheduleDto> GetAdScheduleFromTwitch(AdBreakStartedCommand command)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;
		var parameters = command.CreateParameters(streamerId);
		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		return result.ConvertToDto();
	}
}

/*════════════════════【 COMMAND 】════════════════════*/
public record AdBreakStartedCommand(AdBreakBeginEvent Args) : IRequest
{
	public GetAdScheduleRequestParameters CreateParameters(string streamerId)
		=> new() { BroadcasterId = streamerId };
};