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
	/// The reward cost.
	/// </summary>
	[JsonPropertyName("cost")]
	public int Cost { get; set; }

	/// <summary>
	/// Optional. Emote that was unlocked.
	/// </summary>
	[JsonPropertyName("unlocked_emote")]
	public UnlockedEmote? UnlockedEmote { get; set; }
}

public class UnlockedEmote
{
	/// <summary>
	/// The emote ID.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The human readable emote token.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}