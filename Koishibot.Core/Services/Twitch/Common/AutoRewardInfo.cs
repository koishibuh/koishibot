using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.Common;

public class AutoRewardInfo
{
	/// <summary>
	/// The type of reward. 
	/// </summary>
	[JsonPropertyName("type")]
	public RewardType Type { get; set; }

	/// <summary>
	/// Number of channel points used.
	/// </summary>
	[JsonPropertyName("channel_points")]
	public int Cost { get; set; }

	/// <summary>
	/// Optional. Emote associated with the reward.
	/// </summary>
	[JsonPropertyName("unlocked_emote")]
	public RewardEmote? UnlockedEmote { get; set; }
}