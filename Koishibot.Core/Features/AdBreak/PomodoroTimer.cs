using Koishibot.Core.Features.AdBreak.Enums;
using Koishibot.Core.Features.AdBreak.Events;
using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Interfaces;
using Koishibot.Core.Features.AdBreak.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Obs;
using Koishibot.Core.Features.Obs.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak;

// == ⚫ SERVICE == //

public record PomodoroTimer(
	IOptions<Settings> Settings,
	ILogger<PomodoroTimer> Log,
	IChatReplyService ChatReplyService,
	IObsService ObsService,
	ISignalrService Signalr, IAppCache Cache,
	ITwitchApiRequest TwitchApiRequest
	) : IPomodoroTimer
{
	private CancellationTokenSource _cancelToken = new();
	public ATimer? timer;

	// == ⚫ == //

	public async Task GetAdSchedule()
	{
		if (Settings.Value.DebugMode) { return; }

		var parameters = new GetAdScheduleRequestParameters
		{ BroadcasterId = Settings.Value.StreamerTokens.UserId };

		var result = await TwitchApiRequest.GetAdSchedule(parameters);
		var adInfo = result.ConvertToDto();
		await StartTimer(adInfo);
	}

	// == ⚫ == //

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

		timer = Toolbox.CreateTimer(adInfo.CalculateAdjustedTimeUntilNextAd(), () => SwitchToBreak());
		timer.Start();
	}

	public async void SwitchToBreak()
	{
		await ChatReplyService.App(Command.PomdoroBreak);
		await ObsService.StartBreak();

		var breakTimer = new CurrentTimer().SetBreak();
		Cache.AddCurrentTimer(breakTimer);
		var breakTimerVm = breakTimer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(breakTimerVm);
	}

	// == ⚫ == //

	public void CancelTimer()
	{
		try
		{
			timer?.Dispose();
		}
		catch
		{
			throw new Exception("Timer is null");
		}
	}

	public async Task UpdateOverlayTimer(CurrentTimer timer)
	{
		var pomoTimerVm = timer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(pomoTimerVm);
	}

	public async Task SendLog(AdScheduleDto adInfo)
	{
		await Signalr.SendLog
			(new LogVm($"Pomdoro Delaying for {adInfo.CalculateAdjustedTimeUntilNextAd()} minutes", "Info"));
	}
}