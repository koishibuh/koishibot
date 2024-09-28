using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.ChannelPoints;

public record DragonQuestService(
ILogger<DragonQuestService> Log,
IAppCache Cache,
KoishibotDbContext Database,
IChatReplyService ChatReplyService,
IChannelPointsApi ChannelPointsApi
) : IDragonQuestService
{
	/*════════════════【 INITIALIZE 】════════════════*/
	public async Task Initialize()
	{
		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");
		if (reward is null)
		{
			Log.LogError("Unable to find Dragon Egg Quest in Repo");
			return;
		} // Need to create the reward

		var redemptions = await Database.GetTodayRedemptionByRewardId(reward.Id);
		if (redemptions.NoRedemptionsToday())
		{
			var dragonQuest = new DragonQuest(reward, 0);

			await Cache
				.AddDragonQuest(dragonQuest)
				.UpdateServiceStatusOnline(ServiceName.DragonQuest);

			await ChannelPointsApi.EnableRedemption(reward.TwitchId);
			await ChatReplyService.App(Command.DragonQuestEnabled);
			return;
		}

		if (redemptions.WasSuccessfullyRedeemedToday())
		{
			await ChannelPointsApi.DisableRedemption(reward.TwitchId);
		}
		else
		{
			await ChannelPointsApi.EnableRedemption(reward.TwitchId);

			var quest = new DragonQuest(reward, redemptions.Count);
			Cache.AddDragonQuest(quest);

			await ChatReplyService.App(Command.DragonQuestEnabled);
		}
	}

/*════════════════【 GET RESULT 】════════════════*/
	public async Task GetResult(TwitchUser user, DateTimeOffset redeemedAt)
	{
		if (Cache.DragonQuestClosed())
		{
			await ChatReplyService.App(Command.DragonQuestDisabled);
			return;
		}

		var successRange = Cache.GetDragonQuest();
		var chance = Toolbox.GenerateRandomNumber(100);

		Log.LogInformation($"Number: {chance} vs Win: {successRange.UpperLimit}");

		var reward = await Database.GetChannelRewardByName("Dragon Egg Quest");

		if (Toolbox.NumberWithinRange(chance, successRange.UpperLimit))
		{
			await ChannelPointsApi.DisableRedemption(reward.TwitchId);

			var quest = new DragonQuest(reward, 0).SetWinner(user);
			Cache.AddDragonQuest(quest);

			var data = new { User = user.Name, Number = (successRange.Attempts + 1) };
			await ChatReplyService.App(Command.DragonQuestSuccessful, data);

			var redemption = new ChannelPointRedemption()
				.Set(reward, user, redeemedAt, true);

			await Database.UpdateEntry(redemption);
		}
		else
		{
			successRange
				.IncreaseWinRangeBy(8)
				.IncreaseAttemptCount(1);

			Cache.AddDragonQuest(successRange);

			var redemption = new ChannelPointRedemption()
				.Set(reward, user, redeemedAt, false);

			await Database.UpdateEntry(redemption);

			var data = new { User = user.Name, Number = successRange.Attempts };
			await ChatReplyService.App(Command.DragonQuestFailed, data);
		}
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IDragonQuestService
{
	Task Initialize();
	Task GetResult(TwitchUser user, DateTimeOffset redeemedAt);
}