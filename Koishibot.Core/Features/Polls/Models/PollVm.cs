using Koishibot.Core.Features.RaidSuggestions.Models;
namespace Koishibot.Core.Features.Polls.Models;

public record PollVm(
		string Id,
		string Title,
		DateTimeOffset StartedAt,
		DateTimeOffset EndingAt,
		TimeSpan Duration,
		List<PollChoiceInfo> Choices
);