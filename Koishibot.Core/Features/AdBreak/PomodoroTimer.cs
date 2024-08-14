using Koishibot.Core.Features.AdBreak.Enums;
using Koishibot.Core.Features.AdBreak.Events;
using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Dandle;
using Koishibot.Core.Features.Obs;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record PomodoroTimer(
IOptions<Settings> Settings,
IChatReplyService ChatReplyService,
IObsService ObsService,
ISignalrService Signalr,
IAppCache Cache,
ITwitchApiRequest TwitchApiRequest,
IDandleService DandleService
) : IPomodoroTimer
{
	private ATimer? _timer;

	public async Task GetAdSchedule()
	{
		if (Settings.Value.DebugMode) { return; }

		var parameters = new GetAdScheduleRequestParameters
		{ BroadcasterId = Settings.Value.StreamerTokens.UserId };

		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		var adInfo = result.ConvertToDto();
		await StartTimer(adInfo);
	}

/*═════════◣ START ◢═════════*/
	/// <summary>
	/// <see cref="AdBreakStartedHandler">Ad Break Started</see>
	/// </summary>
	/// <param name="adInfo"></param>
	/// <returns></returns>
	public async Task StartTimer(AdScheduleDto adInfo)
	{
		var pomoTimer = new CurrentTimer().SetPomodoro(adInfo.NextAdScheduledAt);
		Cache.AddCurrentTimer(pomoTimer);

		await UpdateOverlayTimer(pomoTimer);
		await SendLog(adInfo);

		_timer = Toolbox.CreateTimer(adInfo.CalculateAdjustedTimeUntilNextAd(), ()
			=> SwitchToBreak());
		_timer.Start();
	}

	private async void SwitchToBreak()
	{
		await ChatReplyService.App(Command.PomodoroBreak);
		await ObsService.StartBreak();

		await DandleService.StartGame();

		var breakTimer = new CurrentTimer().SetBreak();
		Cache.AddCurrentTimer(breakTimer);
		var breakTimerVm = breakTimer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(breakTimerVm);
	}


/*═════════◣ CANCEL ◢═════════*/
	public void CancelTimer()
	{
		try
		{
			_timer?.Dispose();
		}
		catch
		{
			throw new Exception("Timer is null");
		}
	}

	private async Task UpdateOverlayTimer(CurrentTimer timer)
	{
		var pomoTimerVm = timer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(pomoTimerVm);
	}

	private async Task SendLog(AdScheduleDto adInfo)
	{
		await Signalr.SendInfo($"Pomodoro Delaying for {adInfo.CalculateAdjustedTimeUntilNextAd()} minutes");
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
	public interface IPomodoroTimer
	{
		Task GetAdSchedule();
		Task StartTimer(AdScheduleDto adInfo);
		void CancelTimer();
	}