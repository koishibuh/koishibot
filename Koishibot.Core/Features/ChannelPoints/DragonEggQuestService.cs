//using Koishibot.Core.Features.ChannelPoints.Enums;
//using Koishibot.Core.Features.ChannelPoints.Extensions;
//using Koishibot.Core.Features.ChannelPoints.Interfaces;
//using Koishibot.Core.Features.ChannelPoints.Models;
//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.TwitchUsers.Models;
//using Koishibot.Core.Persistence;
//namespace Koishibot.Core.Features.ChannelPoints;

//public record DragonEggQuestService(
//	ILogger<DragonEggQuestService> Log,
//	IAppCache Cache, KoishibotDbContext Database,
//	IChatMessageService BotIrc, IChannelPointsApi ChannelPointsApi
//	) : IDragonEggQuestService
//{
//	public async Task Initialize()
//	{
//		if (TodayIsNotMondayOrThursday(DateTime.UtcNow)) { return; }

//		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");
//		if (reward is null)
//		{
//			Log.LogError("Unable to find Dragon Egg Quest in Repo");
//			return;
//		} // Need to create the reward

//		var redemptions = await Database.GetTodayRedemptionByRewardId(reward.Id);
//		if (redemptions.Count == 0 || redemptions is null)
//		{
//			var dragonEggQuest = new DragonEggQuest().Set(reward, 0);

//			await Cache
//				.AddDragonEggQuest(dragonEggQuest)
//				.UpdateDragonEggQuestServiceStatus(true);

//			await ChannelPointsApi.EnableRedemption(reward.TwitchId);
//			await BotIrc.DragonEggStatus(Code.DragonEggQuestEnabled);
//			return;
//		}

//		if (redemptions.WasSucessfullyRedeemedToday())
//		{
//			await ChannelPointsApi.DisableRedemption(reward.TwitchId);
//		}
//		else
//		{
//			await ChannelPointsApi.EnableRedemption(reward.TwitchId);

//			var quest = new DragonEggQuest().Set(reward, redemptions.Count);

//			Cache.AddDragonEggQuest(quest);

//			await BotIrc.DragonEggStatus(Code.DragonEggQuestEnabled);
//		}
//	}

//	// == ⚫ == //

//	public async Task GetResult(TwitchUser user, DateTimeOffset redeemedAt)
//	{
//		if (Cache.DragonEggQuestClosed())
//		{
//			await BotIrc.DragonEggStatus(Code.DragonEggQuestDisabled);
//			return;
//		}

//		var successRange = Cache.GetDragonEggQuest();

//		var chance = Toolbox.GenerateRandomNumber(100);

//		Log.LogInformation($"Number: {chance} vs Win: {successRange.UpperLimit}");

//		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");

//		if (Toolbox.NumberWithinRange(chance, successRange.UpperLimit))
//		{
//			await ChannelPointsApi.DisableRedemption(reward.TwitchId);

//			var quest = new DragonEggQuest().Set(reward, 0);

//			await Cache
//				.AddDragonEggQuest(quest)
//				.DisableDragonEggQuestService();

//			await BotIrc.PostDragonEggResult(Code.Successful, user, (successRange.Attempts + 1));

//			var redemption = new ChannelPointRedemption()
//				.Set(reward, user, redeemedAt, true);

//			await Database.UpdateRedemption(redemption);
//		}
//		else
//		{
//			successRange
//				.IncreaseWinRangeBy(8)
//				.IncreaseAttemptCount(1);

//			Cache.AddDragonEggQuest(successRange);

//			var redemption = new ChannelPointRedemption()
//				.Set(reward, user, redeemedAt, false);

//			await Database.UpdateRedemption(redemption);
//			await BotIrc.PostDragonEggResult(Code.Fail, user, successRange.Attempts);
//		}
//	}

//	public bool TodayIsNotMondayOrThursday(DateTime today)
//	{
//		return today.DayOfWeek == DayOfWeek.Tuesday
//				|| today.DayOfWeek == DayOfWeek.Wednesday
//				|| today.DayOfWeek == DayOfWeek.Friday
//				|| today.DayOfWeek == DayOfWeek.Saturday
//				|| today.DayOfWeek == DayOfWeek.Sunday;
//	}
//}

//// == ⚫ CHAT REPLIES == //

//public static class ChannelPointChatReply
//{
//	// == ⚫  == //

//	public static async Task PostDragonEggResult
//			(this IChatMessageService irc, Code code, TwitchUser user, int count)
//	{
//		var message = code switch
//		{
//			Code.Successful
//				=> $"On the {count} attempt, {user.Name}'s Dragon Egg Quest was successful! " +
//					$"Please choose: Alpine, Desert, Coast, Forest, Jungle, or Volcano",

//			Code.Fail
//				=> $"{user.Name} found an egg nest! However they were chased away by Momma Dragon " +
//					$"and returned empty handed. There has been {count} quest attempts today",

//			_ => $"Something bork here for DragonEggQuest lol"
//		};

//		await irc.BotSend(message);
//	}

//	// == ⚫  == //

//	public static async Task DragonEggStatus
//			(this IChatMessageService irc, Code code)
//	{
//		var message = code switch
//		{
//			Code.DragonEggQuestEnabled
//				=> $"It's a good day to explore for Dragon Eggs! (Dragon Egg Quest has been enabled!)",

//			Code.DragonEggQuestDisabled
//				=> $"Sorry adventurer, today is not a day for dragon egg questin'! (Why is this bork?)",

//			_ => $"Something bork for PostDragonEggStatus lol"
//		};

//		await irc.BotSend(message);
//	}
//}
