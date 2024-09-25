using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchannel_points_custom_reward_redemptionadd">Twitch Documentation</see><br/>
/// When a viewer has redeemed a custom channel points reward on the specified channel.<br/>
/// Required Scopes: channel:read:redemptions or channel:manage:redemptions<br/>
/// </summary>
public class RewardRedemptionAddedEvent
{
	///<summary>
	///The redemption identifier.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///The requested broadcaster ID.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The requested broadcaster login.
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	///The requested broadcaster display name.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	///<summary>
	///User ID of the user that redeemed the reward.
	///</summary>
	[JsonPropertyName("user_id")]
	public string ViewerId { get; set; } = string.Empty;

	///<summary>
	///Login of the user that redeemed the reward.
	///</summary>
	[JsonPropertyName("user_login")]
	public string ViewerLogin { get; set; } = string.Empty;

	///<summary>
	///Display name of the user that redeemed the reward.
	///</summary>
	[JsonPropertyName("user_name")]
	public string ViewerName { get; set; } = string.Empty;

	///<summary>
	///The user input provided. Empty string if not provided.
	///</summary>
	[JsonPropertyName("user_input")]
	public string? UserInput { get; set; }

	///<summary>
	///Defaults to unfulfilled. Possible values are unknown, unfulfilled, fulfilled, and canceled.
	///</summary>
	[JsonPropertyName("status")]
	public RewardStatus Status { get; set; }

	///<summary>
	///Basic information about the reward that was redeemed, at the time it was redeemed.
	///</summary>
	[JsonPropertyName("reward")]
	public RewardInfo? Reward { get; set; }

	///<summary>
	///Timestamp of when the reward was redeemed.<br/>
	///(RFC3339 converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("redeemed_at")]
	public DateTimeOffset RedeemedAt { get; set; }
}