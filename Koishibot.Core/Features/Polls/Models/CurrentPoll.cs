using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Persistence.Cache.Enums;
namespace Koishibot.Core.Features.Polls.Models;

public class CurrentPoll
{
	public string Id { get; set; } = null!;
	public string Title { get; set; } = string.Empty;
	public DateTimeOffset StartedAt { get; set; }
	public DateTimeOffset EndingAt { get; set; }
	public TimeSpan Duration { get; set; }
	public List<PollChoiceInfo> Choices { get; set; } = [];

	public TimeSpan CalculateDuration() => StartedAt - EndingAt;
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class CurrentPollExtensions
{
	public static void AddPoll(this IAppCache cache, CurrentPoll poll) =>
		cache.Add(CacheName.CurrentPoll, poll);

	public static string? GetCurrentPollId(this IAppCache cache)
	{
		var result = cache.Get<CurrentPoll>(CacheName.CurrentPoll);
		return result?.Id;
	}
}