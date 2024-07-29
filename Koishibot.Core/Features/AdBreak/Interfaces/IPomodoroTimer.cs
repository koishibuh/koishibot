using Koishibot.Core.Features.AdBreak.Models;

namespace Koishibot.Core.Features.AdBreak.Interfaces;

public interface IPomodoroTimer
{
	Task GetAdSchedule();
	Task StartTimer(AdScheduleDto adInfo);
	void CancelTimer();
}