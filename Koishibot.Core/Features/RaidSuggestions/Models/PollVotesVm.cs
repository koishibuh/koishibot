namespace Koishibot.Core.Features.RaidSuggestions.Models;

public class PollVotesVm
{
	public List<PollChoiceInfo> CurrentPollResults { get; set; } = [];

	public PollVotesVm Set(Dictionary<string, int> votes)
	{
		// CurrentPollResults.Add(new(votes.ElementAt(0).Key, votes.ElementAt(0).Value));
		// CurrentPollResults.Add(new(votes.ElementAt(1).Key, votes.ElementAt(1).Value));
		// CurrentPollResults.Add(new(votes.ElementAt(2).Key, votes.ElementAt(2).Value));

		foreach (var vote in votes)
		{
			CurrentPollResults.Add(new PollChoiceInfo(vote.Key, vote.Value));
		}

		return this;
	}
}

public record PollChoiceInfo(
string Choice,
int VoteCount
);