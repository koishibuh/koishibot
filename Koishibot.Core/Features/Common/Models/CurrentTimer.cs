namespace Koishibot.Core.Features.Common.Models;

public class CurrentTimer
{
	public string Title { get; set; } = string.Empty;
	public DateTimeOffset EndsAt { get; set; }
	public OverlayTimerVm Model { get; set; } = new();

	// == ⚫ == //

	public bool TimerExpired()
	{
		return TimeRemaining() <= TimeSpan.Zero;
	}

	public TimeSpan TimeRemaining()
	{
		return EndsAt - DateTimeOffset.Now;
	}

	public CurrentTimer SetStartingSoon()
	{
		Title = "Starting Soon";
		EndsAt = DateTimeOffset.Now + new TimeSpan(0, 5, 0);
		return this;
	}

	public CurrentTimer SetPomodoro(DateTimeOffset? endsAt)
	{
		Title = "Next Break";
		EndsAt = endsAt ?? DateTimeOffset.Now;
		return this;
	}

	public CurrentTimer SetBreak()
	{
		Title = "Short Break";
		EndsAt = DateTimeOffset.Now + new TimeSpan(0, 5, 0);
		return this;
	}

	//public CurrentTimer SetPoll(CurrentPoll e)
	//{
	//	Title = "Poll";
	//	EndsAt = DateTimeOffset.Now + e.Duration;
	//	return this;
	//}

	public CurrentTimer SetSuggestionPoll()
	{
		Title = "Suggestions Open";
		EndsAt = DateTimeOffset.Now + TimeSpan.FromMinutes(2);
		return this;
	}

	public CurrentTimer SetEndingStream()
	{
		Title = "Raiding Soon";
		EndsAt = DateTimeOffset.Now + TimeSpan.FromSeconds(90);
		return this;
	}

	public CurrentTimer ClearTimer()
	{
		Title = "";
		EndsAt = DateTimeOffset.UtcNow;
		return this;
	}

	public OverlayTimerVm ConvertToVm()
	{
		var duration = EndsAt - DateTimeOffset.Now;

		return new OverlayTimerVm
		{ Title = Title, Minutes = duration.Minutes, Seconds = duration.Seconds };
	}
};
