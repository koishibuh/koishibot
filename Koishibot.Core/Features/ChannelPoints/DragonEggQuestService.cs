using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.ChannelPoints;

public record DragonEggQuestService(
	ILogger<DragonEggQuestService> Log,
	IAppCache Cache, KoishibotDbContext Database,
	IChatReplyService ChatReplyService, IChannelPointsApi ChannelPointsApi
	) : IDragonEggQuestService
{
	public async Task Initialize()
	{
		if (TodayIsNotMondayOrThursday(DateTime.UtcNow)) { return; }

		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");
		if (reward is null)
		{
			Log.LogError("Unable to find Dragon Egg Quest in Repo");
			return;
		} // Need to create the reward

		var redemptions = await Database.GetTodayRedemptionByRewardId(reward.Id);
		if (redemptions.Count == 0 || redemptions is null)
		{
			var dragonEggQuest = new DragonEggQuest().Set(reward, 0);

			await Cache
				.AddDragonEggQuest(dragonEggQuest)
				.UpdateDragonEggQuestServiceStatus(true);

			await ChannelPointsApi.EnableRedemption(reward.TwitchId);
			await ChatReplyService.App(Command.DragonEggQuestEnabled);
			return;
		}

		if (redemptions.WasSucessfullyRedeemedToday())
		{
			await ChannelPointsApi.DisableRedemption(reward.TwitchId);
		}
		else
		{
			await ChannelPointsApi.EnableRedemption(reward.TwitchId);

			var quest = new DragonEggQuest().Set(reward, redemptions.Count);

			Cache.AddDragonEggQuest(quest);

			await ChatReplyService.App(Command.DragonEggQuestEnabled);
		}
	}

	// == ⚫ == //

	public async Task GetResult(TwitchUser user, DateTimeOffset redeemedAt)
	{
		if (Cache.DragonEggQuestClosed())
		{
			await ChatReplyService.App(Command.DragonEggQuestDisabled);
			return;
		}

		var successRange = Cache.GetDragonEggQuest();

		var chance = Toolbox.GenerateRandomNumber(100);

		Log.LogInformation($"Number: {chance} vs Win: {successRange.UpperLimit}");

		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");

		if (Toolbox.NumberWithinRange(chance, successRange.UpperLimit))
		{
			await ChannelPointsApi.DisableRedemption(reward.TwitchId);

			var quest = new DragonEggQuest().Set(reward, 0);

			await Cache
				.AddDragonEggQuest(quest)
				.DisableDragonEggQuestService();

			var data = new UserCountData(user.Name, (successRange.Attempts + 1));

			await ChatReplyService.App(Command.DragonEggQuestSuccessful, data);

			var redemption = new ChannelPointRedemption()
				.Set(reward, user, redeemedAt, true);

			await Database.UpdateRedemption(redemption);
		}
		else
		{
			successRange
				.IncreaseWinRangeBy(8)
				.IncreaseAttemptCount(1);

			Cache.AddDragonEggQuest(successRange);

			var redemption = new ChannelPointRedemption()
				.Set(reward, user, redeemedAt, false);

			await Database.UpdateRedemption(redemption);

			var data = new UserCountData(user.Name, successRange.Attempts)
			await ChatReplyService.App(Command.DragonEggQuestFailed, data);
		}
	}

	public bool TodayIsNotMondayOrThursday(DateTime today)
	{
		return today.DayOfWeek == DayOfWeek.Tuesday
				|| today.DayOfWeek == DayOfWeek.Wednesday
				|| today.DayOfWeek == DayOfWeek.Friday
				|| today.DayOfWeek == DayOfWeek.Saturday
				|| today.DayOfWeek == DayOfWeek.Sunday;
	}
}