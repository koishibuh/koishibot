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
) : IAdBreakStartedHandler
{
	private AdScheduleDto? AdInfo { get; set; }

	public async Task Handle
		(AdBreakBeginEvent e)
	{
		await UpdateOverlayTimer(e);
		// TODO: Post to UI Add started?

		PomodoroService.CancelTimer();

		await ChatReplyService.CreateResponse(Response.AdNowPlaying);

		AdInfo = await GetAdScheduleFromTwitch(e);

		await Signalr.SendInfo($"Ad started, delaying for {AdInfo.AdDurationInSeconds}");
		var timer = Toolbox.CreateTimer(AdInfo.AdDurationInSeconds, async () => await AdsCompleted());
		timer.Start();
	}

	private async Task AdsCompleted()
	{
		await ChatReplyService.CreateResponse(Response.AdCompleted);

		await PomodoroService.StartTimer(AdInfo);
	}

/*═════════◣ ◢═════════*/
	private async Task UpdateOverlayTimer(AdBreakBeginEvent command)
	{
		var totalMilliseconds = (int)command.DurationInSeconds.TotalMilliseconds;
		var adBreakVm = new AdBreakVm(totalMilliseconds, DateTime.Now);
		await Signalr.SendAdStartedEvent(adBreakVm);
	}

/*═════════◣ ◢═════════*/
	private async Task<AdScheduleDto> GetAdScheduleFromTwitch(AdBreakBeginEvent command)
	{
		var streamerId = Settings.Value.StreamerTokens.UserId;
		var parameters = command.CreateParameters();
		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		return result.ConvertToDto();
	}
}

/*════════════════════【 EXTENSIONS 】════════════════════*/

public static class AdBreakStartedEventExtensions
{
	public static GetAdScheduleRequestParameters CreateParameters(this AdBreakBeginEvent e)
		=> new() { BroadcasterId = e.BroadcasterId };
}


/*══════════════════【 INTERFACE 】══════════════════*/
public interface IAdBreakStartedHandler
{
	Task Handle(AdBreakBeginEvent e);
}