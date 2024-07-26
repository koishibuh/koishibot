using Koishibot.Core.Features.AdBreak.Controllers;

namespace Koishibot.Core.Features.AdBreak.Interfaces;

public interface IPomodoroTimer
{
	Task GetAdSchedule();
	Task StartTimer(AdScheduleDto adInfo);
	void CancelTimer();
}