using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.ChannelPoints.Models;

public class DragonEggQuest
{
	public int Id { get; set; }
	public string TwitchId { get; set; } = null!;
	public int Attempts { get; set; }
	public int UpperLimit { get; set; }
	public TwitchUser? SuccessfulUser { get; set; }

	// == ⚫  == //
	public DragonEggQuest Set(ChannelPointReward reward, int redemptionCount)
	{
		Id = reward.Id;
		TwitchId = reward.TwitchId;
		Attempts = redemptionCount;
		UpperLimit = redemptionCount == 0 ? 5 : redemptionCount * 5;
		return this;
	}

	public DragonEggQuest SetWinner(TwitchUser user)
	{
		SuccessfulUser = user;
		return this;
	}

	public DragonEggQuest IncreaseWinRangeBy(int increaseBy)
	{
		UpperLimit += increaseBy;
		return this;
	}

	public DragonEggQuest IncreaseAttemptCount(int increaseBy)
	{
		Attempts += increaseBy;
		return this;
	}
}