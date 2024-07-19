namespace Koishibot.Core.Features.RaidSuggestions.Models;

public class RaidCandidateVm
{
	public string MultistreamUrl { get; set; } = string.Empty;
	public List<RaidCandidateDto> RaidCandidates { get; set; } = null!;

}