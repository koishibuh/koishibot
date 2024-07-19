using Koishibot.Core.Features.Common.Enums;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Raids.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.Raids.Extensions;
public static class RaidExtensions
{

	// MODEL

	public static StreamEventVm ConvertToModel(this IncomingRaid raid)
	{
		return new StreamEventVm
		{
			EventType = StreamEventType.Raid,
			Timestamp = (DateTimeOffset.UtcNow).ToString("yyyy-MM-dd HH:mm"),
			Message = $"{raid.RaidedByUser} has raided with {raid.ViewerCount}"
		};
	}

	// CACHE

	public static int GetRaidTargetSuggestorId(this IAppCache cache)
	{
		var result = cache.Get<Raid>(CacheName.Raid)
			?? throw new Exception("Raid Target not found");

		if (result.RaidTarget is not null)
		{
			return result.RaidTarget.SuggestedByUser is not null
				? result.RaidTarget.SuggestedByUser.Id
				: 1;
		}
		else
		{
			throw new Exception("Raid not found in Cache");		
		}

	}

	// DATABASE

	public static async Task AddRaid
		(this KoishibotDbContext database, OutgoingRaid raid)
	{
		database.Update(raid);
		await database.SaveChangesAsync();
	}

	public static async Task AddRaid
		(this KoishibotDbContext database, IncomingRaid raid)
	{
		database.Update(raid);
		await database.SaveChangesAsync();
	}

}
