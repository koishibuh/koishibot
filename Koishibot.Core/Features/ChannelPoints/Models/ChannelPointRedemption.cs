using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.ChannelPoints.Models;

public class ChannelPointRedemption
{
	public int Id { get; set; }
	public int ChannelPointRewardId { get; set; }
	public DateTimeOffset RedeemedAt { get; set; }
	public int UserId { get; set; }
	public bool WasSuccesful { get; set; }
	public ChannelPointReward ChannelPointReward { get; set; } = null!;
	public TwitchUser User { get; set; } = null!;

	// == ⚫  == //

	public ChannelPointRedemption Set(ChannelPointReward reward,
			TwitchUser user, DateTimeOffset redeemedAt, bool wasSuccesful)
	{
		ChannelPointRewardId = reward.Id;
		RedeemedAt = redeemedAt;
		UserId = user.Id;
		WasSuccesful = wasSuccesful;
		return this;
	}
}