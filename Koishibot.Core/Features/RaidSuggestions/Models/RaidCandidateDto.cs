namespace Koishibot.Core.Features.RaidSuggestions.Models;

public record RaidCandidateDto(
	string StreamerName,
	string SuggestedByName,
	string StreamTitle,
	string StreamGame
	);
