using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.RaidSuggestions.Models;

public static class RaidSuggestionExtensions
{
	public static void Add(this IAppCache cache, List<RaidSuggestion> dtos)
	{
		var result = cache.Get<Raid>(CacheName.Raid);
		if (result is null)
		{
			result = new Raid();
		}

		result.RaidSuggestions = dtos;

		cache.Add(CacheName.Raid, result);
	}


	// == ⚫ Validation

	public static bool StreamerAlreadySuggested(this List<RaidSuggestion> dtos, string Streamer)
	{
		var result = dtos.FirstOrDefault(u => u.Streamer.Login == Streamer);
		return result != null;
	}

	public static List<RaidSuggestion> Select3RandomCandidates(this List<RaidSuggestion> suggestions)
	{
		return suggestions
						.OrderBy(s => new Random().Next())
						.Take(3)
						.ToList();
	}

	public static RaidSuggestion? SelectARandomCandidate(this List<RaidSuggestion> suggestions)
	{
		return suggestions
						.OrderBy(s => new Random().Next())
						.Take(1)
						.FirstOrDefault();
	}


	// == ⚫ Voting
	public static List<RaidSuggestion> RemoveRaidCandidates(this List<RaidSuggestion> suggestions, List<RaidSuggestion> candidates)
	{
		suggestions.RemoveAll(s => candidates.Contains(s));
		return suggestions;
	}

	public static List<string> CreatePollChoices(this List<RaidSuggestion> dto)
	{
		return dto.Select(x => x.Streamer.Name).ToList();
	}

	public static RaidCandidateVm ConvertToVm(this List<RaidSuggestion> candidates)
	{
		var link = $"https://multistre.am/" +
							$"{candidates[0].Streamer.Name}/" +
							$"{candidates[1].Streamer.Name}/" +
							$"{candidates[2].Streamer.Name}/layout7/";

		var dto = candidates.Select(x => new RaidCandidateDto
		(x.Streamer.Name, x.SuggestedByUser.Name, x.StreamInfo.Title, x.StreamInfo.GameName))
			.ToList();

		return new RaidCandidateVm
		{
			MultistreamUrl = link,
			RaidCandidates = dto
		};
	}
}