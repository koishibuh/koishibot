﻿using HtmlAgilityPack;
using Koishibot.Core.Features.ChannelPoints.Enums;
using Koishibot.Core.Features.ChannelPoints.Extensions;
using Koishibot.Core.Features.ChannelPoints.Interfaces;
using Koishibot.Core.Features.ChannelPoints.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.ChannelPoints;

public record DragonEggQuestService(
ILogger<DragonEggQuestService> Log,
IAppCache Cache,
KoishibotDbContext Database,
IChatReplyService ChatReplyService,
IChannelPointsApi ChannelPointsApi,
IHttpClientFactory HttpClientFactory
) : IDragonEggQuestService
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
		if (redemptions.Count == 0)
		{
			var dragonEggQuest = new DragonEggQuest().Set(reward, 0);

			await Cache
			.AddDragonEggQuest(dragonEggQuest)
			.UpdateServiceStatusOnline(ServiceName.DragonEggQuest);

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

/*════════════════【 GET RESULT 】════════════════*/
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

			// TODO: Reset this at the end of stream
			var quest = new DragonEggQuest().Set(reward, 0).SetWinner(user);

			Cache.AddDragonEggQuest(quest);
			// .DisableDragonEggQuestService();

			var data = new { User = user.Name, Number = (successRange.Attempts + 1) };
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

			var data = new { User = user.Name, Number = successRange.Attempts };
			await ChatReplyService.App(Command.DragonEggQuestFailed, data);
		}
	}

	public async Task<List<string>> GetEggDescriptions(string url)
	{
		var httpClient = HttpClientFactory.CreateClient("Default");
		using var response = await httpClient.GetAsync(url);
		response.EnsureSuccessStatusCode();

		var content = await response.Content.ReadAsStringAsync();
		var webpage = new HtmlDocument();
		webpage.LoadHtml(content);

		var eggNodes = webpage.DocumentNode.SelectNodes("//div[@class='eggs']//div")
		               ?? throw new Exception("Error finding egg on page");

		return eggNodes
		.Select(egg => egg.SelectSingleNode(".//span")
		               ?? throw new Exception("Error finding egg description"))
		.Select(span => span.InnerText)
		.ToList();
	}

	public async Task AddEggToWebsite()
	{
		//
	}
}