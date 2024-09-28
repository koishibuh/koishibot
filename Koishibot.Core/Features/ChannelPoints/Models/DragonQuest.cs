using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.ChannelPoints.Models;

public class DragonQuest
{
	public DragonQuest(ChannelPointReward reward, int redemptionCount)
	{
		Id = reward.Id;
		TwitchId = reward.TwitchId;
		Attempts = redemptionCount;
		UpperLimit = redemptionCount == 0 ? 5 : redemptionCount * 5;
	}

	public int Id { get; set; }
	public string TwitchId { get; set; } = null!;
	public int Attempts { get; set; }
	public int UpperLimit { get; set; }
	public TwitchUser? SuccessfulUser { get; set; }

	/*═══════════════════【】═══════════════════*/
	public DragonQuest SetWinner(TwitchUser user)
	{
		SuccessfulUser = user;
		return this;
	}

	public DragonQuest IncreaseWinRangeBy(int increaseBy)
	{
		UpperLimit += increaseBy;
		return this;
	}

	public DragonQuest IncreaseAttemptCount(int increaseBy)
	{
		Attempts += increaseBy;
		return this;
	}
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class DragonQuestExtensions
{
	public static DragonQuest? GetDragonQuest(this IAppCache cache) =>
		cache.Get<DragonQuest>(CacheName.DragonQuest);

	public static bool DragonQuestClosed(this IAppCache cache)
	{
		var result = cache.GetStatusByServiceName(ServiceName.DragonQuest);
		return result is false;
	}

	public static IAppCache AddDragonQuest(this IAppCache cache, DragonQuest quest)
	{
		cache.Add(CacheName.DragonQuest, quest);
		return cache;
	}

	public static IAppCache RemoveDragonQuest(this IAppCache cache)
	{
		cache.Remove(CacheName.DragonQuest);
		return cache;
	}

	public static TwitchUser? GetDragonQuestWinner(this IAppCache cache)
	{
		var result = cache.Get<DragonQuest>(CacheName.DragonQuest)
			?? throw new Exception("DragonQuest not in cache");
		return result.SuccessfulUser;
	}
}