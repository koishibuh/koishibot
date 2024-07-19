namespace Koishibot.Core.Features.Polls.Models;

public record PollVm(
		string Id,
		string Title,
		DateTimeOffset StartedAt,
		DateTimeOffset EndingAt,
		TimeSpan Duration,
		Dictionary<string, int> Choices
);