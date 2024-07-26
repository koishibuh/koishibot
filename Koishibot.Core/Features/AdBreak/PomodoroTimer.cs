using Koishibot.Core.Features.AdBreak.Controllers;
using Koishibot.Core.Features.AdBreak.Events;
using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.AdBreak.Interfaces;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Obs;
using Koishibot.Core.Features.Obs.Interfaces;
using Koishibot.Core.Services.Twitch.Irc;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Features.AdBreak;

// == ⚫ SERVICE == //

public record PomodoroTimer(
	IOptions<Settings> Settings,
	ILogger<PomodoroTimer> Log,
	 //IChatMessageService BotIrc,
	 ITwitchIrcService BotIrc,
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

		var pomoTimerVm = pomoTimer.ConvertToVm();
		await Signalr.UpdateTimerOverlay(pomoTimerVm);

		Log.LogInformation($"Pomdoro Delaying for {adInfo.CalculateAdjustedTimeUntilNextAd()} minutes", adInfo);
		timer = Toolbox.CreateTimer(adInfo.CalculateAdjustedTimeUntilNextAd(), () => SwitchToBreak());
		timer.Start();
	}

	public async void SwitchToBreak()
	{
		await BotIrc.PostMandatoryBreak();
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
}

// == ⚫ CHAT RESPONSE == //

public static class PomodoroTimerChatReply
{
	public static async Task PostMandatoryBreak(this ITwitchIrcService irc)
	{
		var message = "🍅 Mandatory Break! 🍅 " +
			"We've been sitting at the PC for awhile, time for a self care break: " +
			"rest your eyes, stretch those legs, and stay hydrated! " +
			"Or enjoy a game of !Dandle with chat (WIP). BE BACK SOON!";

		await irc.BotSend(message);
	}
}

/// == ⚫ SERVICE == //

//public record PomodoroTimer(
//	IOptions<Settings> Settings,
//	ILogger<PomodoroTimer> Log,
//	IChatMessageService BotIrc, IObsService ObsService, IAdsApi AdsApi,
//	ISignalrService Signalr, IAppCache Cache
//	) : IPomodoroTimer
//{
//	private CancellationTokenSource _cancelToken = new();
//	public ATimer? timer;

//	// == ⚫ == //

//	public async Task GetAdSchedule()
//	{
//		if (Settings.Value.DebugMode) { return; }
//		var adInfo = await AdsApi.GetAdSchedule();
//		await StartTimer(adInfo);
//	}

//	// == ⚫ == //

//	/// <summary>
//	/// <see cref="AdBreakStartedHandler">Ad Break Started</see>
//	/// </summary>
//	/// <param name="adInfo"></param>
//	/// <returns></returns>
//	public async Task StartTimer(AdBreakInfo adInfo)
//	{
//		var pomoTimer = new CurrentTimer().SetPomodoro(adInfo.NextAdAt);
//		Cache.AddCurrentTimer(pomoTimer);
//		var pomoTimerVm = pomoTimer.ConvertToVm();
//		await Signalr.UpdateTimerOverlay(pomoTimerVm);

//		Log.LogInformation($"Pomdoro Delaying for {adInfo.CalculateTimeUntilNextAd()} minutes", adInfo);
//		timer = Toolbox.CreateTimer(adInfo.CalculateTimeUntilNextAd(), () => SwitchToBreak());
//		timer.Start();
//	}

//	public async void SwitchToBreak()
//	{
//		await BotIrc.PostMandatoryBreak();
//		await ObsService.StartBreak();

//		var breakTimer = new CurrentTimer().SetBreak();
//		Cache.AddCurrentTimer(breakTimer);
//		var breakTimerVm = breakTimer.ConvertToVm();
//		await Signalr.UpdateTimerOverlay(breakTimerVm);
//	}

//	// == ⚫ == //

//	public void CancelTimer()
//	{
//		try
//		{
//			timer?.Dispose();
//		}
//		catch
//		{
//			throw new Exception("Timer is null");
//		}
//	}
//}

//// == ⚫ CHAT RESPONSE == //

//public static class PomodoroTimerChatReply
//{
//	public static async Task PostMandatoryBreak(this IChatMessageService irc)
//	{
//		var message = "🍅 Mandatory Break! 🍅 " +
//			"We've been sitting at the PC for awhile, time for a self care break: " +
//			"rest your eyes, stretch those legs, and stay hydrated! " +
//			"Or enjoy a game of !Dandle with chat (WIP). BE BACK SOON!";

//		await irc.BotSend(message);
//	}
//}

//public record PomodoroTimer(
//	IOptions<Settings> Settings,
//	ILogger<PomodoroTimer> Log,
//	IChatMessageService BotIrc, IObsService ObsService, IAdsApi AdsApi,
//	ISignalrService Signalr, IAppCache Cache
//	) : IPomodoroTimer
//{
//	private CancellationTokenSource _cancelToken = new();

//	// == ⚫ == //

//	public async Task GetAdSchedule()
//	{
//		if (Settings.Value.DebugMode) { return; }
//		var adInfo = await AdsApi.GetAdSchedule();
//		await StartTimer(adInfo);
//	}

//	// == ⚫ == //

//	/// <summary>
//	/// <see cref="AdBreakStartedHandler">Ad Break Started</see>
//	/// </summary>
//	/// <param name="adInfo"></param>
//	/// <returns></returns>
//	public async Task StartTimer(AdBreakInfo adInfo)
//	{
//		var pomoTimer = new CurrentTimer().SetPomodoro(adInfo.NextAdAt);
//		Cache.AddCurrentTimer(pomoTimer);
//		var pomoTimerVm = pomoTimer.ConvertToVm();
//		await Signalr.UpdateTimerOverlay(pomoTimerVm);

//		Log.LogInformation($"Pomdoro Delaying for {adInfo.CalculateTimeUntilNextAd()} minutes", adInfo);

//		await Task.Delay(adInfo.CalculateTimeUntilNextAd(), _cancelToken.Token);

//		await BotIrc.PostMandatoryBreak();
//		await ObsService.StartBreak();

//		var breakTimer = new CurrentTimer().SetBreak();
//		Cache.AddCurrentTimer(breakTimer);
//		var breakTimerVm = breakTimer.ConvertToVm();
//		await Signalr.UpdateTimerOverlay(breakTimerVm);
//	}

//	// == ⚫ == //

//	public void CancelTimer()
//	{
//		_cancelToken.Cancel();
//		_cancelToken.Dispose();
//		_cancelToken = new();
//	}
//}