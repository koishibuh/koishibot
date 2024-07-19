namespace Koishibot.Core.Features.RaidSuggestions.Models;

public class RaidPollVm
{
	public List<PollChoiceInfo> CurrentPollResults { get; set; } = [];

	public RaidPollVm Set(Dictionary<string, int> votes)
	{
		CurrentPollResults.Add(new(votes.ElementAt(0).Key, votes.ElementAt(0).Value));
		CurrentPollResults.Add(new(votes.ElementAt(1).Key, votes.ElementAt(1).Value));
		CurrentPollResults.Add(new(votes.ElementAt(2).Key, votes.ElementAt(2).Value));

		return this;
	}
}

public record PollChoiceInfo(
	string Choice,
	int VoteCount
	);