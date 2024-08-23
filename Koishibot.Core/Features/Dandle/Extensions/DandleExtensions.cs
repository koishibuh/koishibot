using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.Dandle.Extensions;
public static class DandleExtensions
{
	// MODEL



	// CACHE 

	public static void EnableDandle(this IAppCache cache)
	{
		cache.UpdateServiceStatus(ServiceName.Dandle, ServiceStatusString.Online);
	}

	public static void DisableDandle(this IAppCache cache)
	{
		cache.UpdateServiceStatus(ServiceName.Dandle, ServiceStatusString.Offline);
	}


	public static bool DandleDisabled(this IAppCache cache)
	{
		var result = cache.GetStatusByServiceName(ServiceName.Dandle);
		return result is false;
	}

	public static void UpdateDandle(this IAppCache cache, DandleGame dandleInfo)
	{
		cache.Add(CacheName.Dandle, dandleInfo);
	}

	public static void ResetDandle(this IAppCache cache)
	{
		cache.Remove(CacheName.Dandle);
	}

	public static DandleGame GetDandleInfo
		(this IAppCache cache)
	{
		var dandleInfo = cache.Get<DandleGame>(CacheName.Dandle);

		return dandleInfo is not null
			? dandleInfo
			: throw new Exception("Dandle Info not in cache");
	}


	public static bool DandleAcceptingSuggestions
		(this IAppCache cache)
	{
		var dandle = cache.Get<DandleGame>(CacheName.Dandle);

		return dandle is null
			? throw new Exception("Dandle Info not in cache")
			: dandle.GamePhase is Phase.Suggestions;
	}

	public static bool DandleAcceptingVotes
	(this IAppCache cache)
	{
		var dandle = cache.Get<DandleGame>(CacheName.Dandle);

		return dandle is null
			? throw new Exception("Dandle Info not in cache")
			: dandle.GamePhase is Phase.Voting;
	}


	public static bool DandleIsClosed(this IAppCache cache)
	{
		var result = cache.GetStatusByServiceName(ServiceName.Dandle);
		return result is false;
	}

	public static bool DandleIsEnabled(this IAppCache cache)
	{
		return cache.GetStatusByServiceName(ServiceName.Dandle);
	}

	public static bool DandleGameInProgress(this IAppCache cache)
	{
		var result = cache.Get<DandleGame>(CacheName.Dandle);
		return result is not null;
	}

	public static void OpenDandleSuggestions(this IAppCache cache)
	{
		var result = cache.Get<DandleGame>(CacheName.Dandle)
			?? throw new Exception("Dandle Info not found");

		result.GamePhase = Phase.Suggestions;
		cache.Add(CacheName.Dandle, result);
	}

	public static void CloseDandleSuggestions(this IAppCache cache)
	{
		var result = cache.Get<DandleGame>(CacheName.Dandle)
			?? throw new Exception("Dandle Info not found");

		result.GamePhase = Phase.Processing;
		cache.Add(CacheName.Dandle, result);
	}

	public static void OpenDandleVoting(this IAppCache cache)
	{
		var result = cache.Get<DandleGame>(CacheName.Dandle)
			?? throw new Exception("Dandle Info not found");

		result.GamePhase = Phase.Voting;
		cache.Add(CacheName.Dandle, result);
	}

	public static void CloseDandleVoting(this IAppCache cache)
	{
		var result = cache.Get<DandleGame>(CacheName.Dandle)
			?? throw new Exception("Dandle Info not found");

		result.GamePhase = Phase.Processing;
		cache.Add(CacheName.Dandle, result);
	}

	// DATABASE

	public static async Task<DandleWord> UpdateDandleWord
	(this KoishibotDbContext database, DandleWord word)
	{
		database.Update(word);
		await database.SaveChangesAsync();
		return word;
	}

	public static async Task RemoveDandleWord
	(this KoishibotDbContext database, DandleWord word)
	{
		database.Remove(word);
		await database.SaveChangesAsync();
	}

	public static async Task<List<DandleWord>> GetDandleWords
		(this KoishibotDbContext database)
	{
		var result = await database.DandleWords
		.AsNoTracking()
		.ToListAsync();

		return result.Count > 0
				? result
				: throw new Exception("0 Dandle Words found");
	}

	public static async Task<DandleWord?> FindDandleWord
		(this KoishibotDbContext database, string word)
	{
		return await database.DandleWords
			.AsNoTracking()
			.Where(x => x.Word == word)
			.FirstOrDefaultAsync();
	}

	// SIGNALR

	public static async Task EnableOverlay
		(this ISignalrService signalr, OverlayName overlayName)
	{
		var status = new OverlayStatus(overlayName, true);
		await signalr.SendOverlayStatus(new OverlayStatusVm(status.Name, status.Status));
	}

	public static async Task DisableOverlay
		(this ISignalrService signalr, OverlayName overlayName)
	{
		var status = new OverlayStatus(overlayName, false);
		await signalr.SendOverlayStatus(new OverlayStatusVm(status.Name, status.Status));
	}

	public static async Task ClearDandleBoard
		(this ISignalrService signalr)
	{
		await signalr.SendClearDandleBoard();
	}

}