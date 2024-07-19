using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.RaidSuggestions.Models;
namespace Koishibot.Core.Features.Raids.Models;

public class Raid
{
	public Phase RaidPhase { get; set; } = Phase.Suggestions;
	public List<RaidSuggestion> RaidSuggestions { get; set; } = [];
	public List<RaidSuggestion> RaidCandidates { get; set; } = [];
	public RaidSuggestion? RaidTarget { get; set; }

	//

	public Raid EnableSuggestions()
	{
		RaidPhase = Phase.Suggestions;
		return this;
	}

	public Raid EnableVoting()
	{
		RaidPhase = Phase.Voting;
		return this;	
	}
}
