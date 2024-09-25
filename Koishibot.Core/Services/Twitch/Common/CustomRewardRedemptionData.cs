using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Common;

public class CustomRewardRedemptionData
{
	///<summary>
	///The ID that uniquely identifies the broadcaster.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The ID that uniquely identifies this redemption.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; }

	///<summary>
	///The ID that uniquely identifies the user that redeemed the reward.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string UserName { get; set; }

	///<summary>
	///The text the user entered at the prompt when they redeemed the reward;<br/>
	///otherwise, an empty string if user input was not required.
	///</summary>
	[JsonPropertyName("user_input")]
	public string UserInput { get; set; } = string.Empty;

	///<summary>
	///The state of the redemption.
	///</summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(RewardRedemptionStatusEnumConverter))]
	public RewardRedemptionStatus Status { get; set; }

	///<summary>
	///The timestamp when the reward was redeemed.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("redeemed_at")]
	public DateTimeOffset RedeemedAt { get; set; }

	///<summary>
	///The reward that the user redeemed.
	///</summary>
	[JsonPropertyName("reward")]
	public Reward Reward { get; set; }
}

public class Reward
{
	///<summary>
	///The ID that uniquely identifies the redeemed reward.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///The reward’s title.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }

	///<summary>
	///The prompt displayed to the viewer if user input is required.
	///</summary>
	[JsonPropertyName("prompt")]
	public string Prompt { get; set; }

	///<summary>
	///The reward’s cost, in Channel Points.
	///</summary>
	[JsonPropertyName("cost")]
	public int Cost { get; set; }
}