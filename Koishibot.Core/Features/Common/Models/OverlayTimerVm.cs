namespace Koishibot.Core.Features.Common.Models;

public class OverlayTimerVm
{
	public string Title { get; set; } = string.Empty;
	public int Minutes { get; set; }
	public int Seconds { get; set; }

	public OverlayTimerVm Set(string title, TimeSpan duration)
	{
		Title = title;
		Minutes = duration.Minutes;
		Seconds = duration.Seconds;
		return this;
	}
};
