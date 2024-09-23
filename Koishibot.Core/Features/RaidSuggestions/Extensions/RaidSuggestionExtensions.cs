using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.RaidSuggestions.Extensions;
public static class RaidSuggestionExtensions
{
	// CACHE
	public static void EnableRaidSuggestions(this IAppCache cache)
	{
		cache.UpdateServiceStatus(ServiceName.Raid, Status.Online);

		var raid = new Raid().EnableSuggestions();
		cache.Add(CacheName.Raid, raid);
	}

	public static IAppCache DisableRaidSuggestions(this IAppCache cache)
	{
		cache.UpdateServiceStatus(ServiceName.Raid, Status.Offline);
		return cache;
	}

	public static IAppCache ClearRaidSuggestions(this IAppCache cache)
	{
		cache.Add(CacheName.Raid, new Raid(), TimeSpan.FromHours(1));
		return cache;
	}

	public static RaidSuggestion GetCandidateByName
		(this List<RaidSuggestion> raidCandidates, string name)
	{
		return raidCandidates.Find(x => x.Streamer.Name == name)
			?? throw new FileNotFoundException();
	}

	public static void AddRaidSuggestions
		(this IAppCache cache, List<RaidSuggestion> raidSuggestions)
	{
		var raid = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid not found in cache");
		raid.RaidSuggestions = raidSuggestions;
		cache.Add(CacheName.Raid, raid);
	}

	public static void AddRaidCandidatesAndSuggestions
	(this IAppCache cache, List<RaidSuggestion> suggestions, List<RaidSuggestion> candidates)
	{
		var raid = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid not found in cache");

		raid.RaidSuggestions = suggestions;
		raid.RaidCandidates = candidates;
		cache.Add(CacheName.Raid, raid);
	}

	public static void AddRaidCandidates
		(this IAppCache cache, List<RaidSuggestion> candidates)
	{
		var raid = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid not found in cache");
		raid.RaidCandidates = candidates;
		cache.Add(CacheName.Raid, raid);
	}

	public static bool UpdateStatusToVoting(this IAppCache cache)
	{
		var raid = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid not found in cache");
		return raid.RaidPhase == Phase.Voting;
	}


	public static bool RaidSuggestionDisabled(this IAppCache cache)
	{
		var raid = cache.GetStatusByServiceName(ServiceName.Raid);
		return raid is false;
	}

	public static List<RaidSuggestion> GetRaidSuggestions(this IAppCache cache)
	{
		var results = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid not found in cache");
		return results.RaidSuggestions;
	}


	public static List<RaidSuggestion> GetRaidCandidates(this IAppCache cache)
	{
		var results = cache.Get<Raid>(CacheName.Raid)
			?? throw new NullReferenceException();

		return results.RaidCandidates is not null
			? results.RaidCandidates
			: throw new NullReferenceException();
	}

	public static RaidSuggestion GetRaidTarget(this IAppCache cache)
	{
		var results = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid not found in cache");

		return results.RaidTarget is not null
			? results.RaidTarget
			: throw new Exception("Raid Target not found");
	}

	public static RaidSuggestion? AddRaidTarget
		(this IAppCache cache, RaidSuggestion raidSuggestion)
	{
		var raid = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid not found in cache");

		raid.RaidTarget = raidSuggestion;
		cache.Add(CacheName.Raid, raid);
		return raid.RaidTarget;
	}


}
