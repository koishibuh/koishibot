namespace Koishibot.Core.Features.Polls.Models;

public class CurrentPoll
{
	public string Id { get; set; } = null!;
	public string Title { get; set; } = string.Empty;
	public DateTimeOffset StartedAt { get; set; }
	public DateTimeOffset EndingAt { get; set; }
	public TimeSpan Duration { get; set; }
	public Dictionary<string, int> Choices { get; set; } = [];

	public TimeSpan CalculateDuration()
	{
		return StartedAt - EndingAt;
	}

	public PollVm ConvertToVm()
	{
		return new PollVm(Id, Title, StartedAt, EndingAt, Duration, Choices);
	}
};