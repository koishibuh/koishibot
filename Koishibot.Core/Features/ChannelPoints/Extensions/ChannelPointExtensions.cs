using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Common;

namespace Koishibot.Core.Features.ChannelPoints.Extensions;
public static class ChannelPointExtensions
{
	// MODEL

	public static bool WasSucessfullyRedeemedToday
		(this List<ChannelPointRedemption>? todayRedeems)
	{
		if (todayRedeems is null) { return false; }
		return todayRedeems.Select(x => x.WasSuccesful == true).Any();
	}

	// CACHE
	public static bool DragonEggQuestClosed(this IAppCache cache)
	{
		var result = cache.GetStatusByServiceName(ServiceName.DragonEggQuest);
		return result is false;
	}


	public static async Task<IAppCache> UpdateDragonEggQuestServiceStatus
		(this IAppCache cache, string status)
	{
		await cache.UpdateServiceStatus(ServiceName.DragonEggQuest, status);

		return cache;
	}
	public static DragonEggQuest GetDragonEggQuest(this IAppCache cache)
	{
		return cache.Get<DragonEggQuest>(CacheName.DragonEggQuest)
			?? throw new Exception("DragonEggQuest not in cache");

	}



	public static IAppCache AddDragonEggQuest(this IAppCache cache, DragonEggQuest quest)
	{
		cache.Add(CacheName.DragonEggQuest, quest);
		return cache;
	}

	public static async Task DisableDragonEggQuestService(this IAppCache cache)
	{
		await cache.UpdateServiceStatus(ServiceName.DragonEggQuest, ServiceStatusString.Offline);
	}

	// DATABASE

	public static async Task<int> UpdateReward
	(this KoishibotDbContext database, ChannelPointReward reward)
	{
		var result = await database.ChannelPointRewards
			.Where(x => x.TwitchId == reward.TwitchId)
			.FirstOrDefaultAsync()
				?? throw new Exception("Reward to update not found");

		result.Title = reward.Title;
		result.Description = reward.Description;
		result.Cost = reward.Cost;
		result.BackgroundColor = reward.BackgroundColor;
		result.IsEnabled = reward.IsEnabled;
		result.IsUserInputRequired = reward.IsUserInputRequired;
		result.IsMaxPerStreamEnabled = reward.IsMaxPerStreamEnabled;
		result.MaxPerStream = reward.MaxPerStream;
		result.IsMaxPerUserPerStreamEnabled = reward.IsMaxPerUserPerStreamEnabled;
		result.MaxPerUserPerStream = reward.MaxPerUserPerStream;
		result.IsGlobalCooldownEnabled = reward.IsGlobalCooldownEnabled;
		result.GlobalCooldownSeconds = reward.GlobalCooldownSeconds;
		result.IsPaused = reward.IsPaused;
		result.ShouldRedemptionsSkipRequestQueue = reward.ShouldRedemptionsSkipRequestQueue;
		result.ImageUrl = reward.ImageUrl;

		database.Update(result);
		database.SaveChanges();
		return reward.Id;
	}

	public static async Task<ChannelPointReward> GetChannelRewardByName
	(this KoishibotDbContext database, string rewardName)
	{
		var result = await database.ChannelPointRewards
			.AsNoTracking()
			.Where(p => p.Title == rewardName)
			.FirstOrDefaultAsync();

		return result is not null
			? result 
			: throw new Exception("Dragon Egg Quest not found in repo");
	}

	public static async Task<List<ChannelPointRedemption>> GetTodayRedemptionByRewardId
	(this KoishibotDbContext database, int rewardId)
	{
		return await database.ChannelPointRedemptions
			.Where(p => p.ChannelPointRewardId == rewardId
				&& p.RedeemedAt.Date == DateTime.UtcNow.Date)
			.ToListAsync();
	}

	public static async Task UpdateRedemption
		(this KoishibotDbContext database, ChannelPointRedemption redemption)
	{
		database.Update(redemption);
		await database.SaveChangesAsync();
	}

	public static async Task<List<ChannelPointReward>> AddRewards
		(this KoishibotDbContext database, List<CustomRewardData> rewards)
	{
		foreach (var reward in rewards)
		{
			var result = await database.ChannelPointRewards
				.Where(x => x.TwitchId == reward.RewardId).FirstOrDefaultAsync();
			if (result is null)
			{
				database.Add(reward);
			}
			else
			{
				result.Title = reward.Title;
				result.Description = reward.Description;
				result.Cost = reward.Cost;
				result.BackgroundColor = reward.BackgroundColor;
				result.IsEnabled = reward.IsEnabled;
				result.IsUserInputRequired = reward.IsUserInputRequired;
				result.IsMaxPerStreamEnabled = reward.MaxPerStreamSetting.IsEnabled;
				result.MaxPerStream = reward.MaxPerStreamSetting.MaxPerStream;
				result.IsMaxPerUserPerStreamEnabled = reward.MaxPerUserPerStreamSetting.IsEnabled;
				result.MaxPerUserPerStream = reward.MaxPerUserPerStreamSetting.MaxPerUserPerStream;
				result.IsGlobalCooldownEnabled = reward.GlobalCooldownSetting.IsEnabled;
				result.GlobalCooldownSeconds = reward.GlobalCooldownSetting.GlobalCooldownSeconds;
				result.IsPaused = reward.IsPaused;
				result.ShouldRedemptionsSkipRequestQueue = reward.ShouldRedemptionsSkipRequestQueue;
				result.ImageUrl = reward.CustomImage is null ? reward.DefaultImage.Url4X : reward.CustomImage.Url4X;

				database.Update(result);
			}
		}

		database.SaveChanges();

		return database.ChannelPointRewards.ToList();
	}

	public static async Task<List<ChannelPointRewardVm>> GetChannelPointRewards
		(this KoishibotDbContext database)
	{
		return await database.ChannelPointRewards
			.AsNoTracking()
			.Select(x => new ChannelPointRewardVm(
				x.TwitchId,
				x.Title,
				x.Cost,
				x.IsEnabled,
				x.IsUserInputRequired,
				x.Description,
				x.BackgroundColor,
				x.IsMaxPerStreamEnabled,
				x.MaxPerStream,
				x.IsMaxPerUserPerStreamEnabled,
				x.MaxPerUserPerStream,
				x.IsGlobalCooldownEnabled,
				x.GlobalCooldownSeconds,
				x.IsPaused,
				x.ShouldRedemptionsSkipRequestQueue,
				x.ImageUrl
				)).ToListAsync();
	}

	public static List<ChannelPointRewardVm> ConvertToVm(this List<ChannelPointReward> rewards)
	{
		return rewards
			.Select(reward => new ChannelPointRewardVm
			(
				reward.TwitchId,
				reward.Title,
				reward.Cost,
				reward.IsEnabled,
				reward.IsUserInputRequired,
				reward.Description,
				reward.BackgroundColor,
				reward.IsMaxPerStreamEnabled,
				reward.MaxPerStream,
				reward.IsMaxPerUserPerStreamEnabled,
				reward.MaxPerUserPerStream,
				reward.IsGlobalCooldownEnabled,
				reward.GlobalCooldownSeconds,
				reward.IsPaused,
				reward.ShouldRedemptionsSkipRequestQueue,
				reward.ImageUrl
			))
			.ToList();
	}
}
