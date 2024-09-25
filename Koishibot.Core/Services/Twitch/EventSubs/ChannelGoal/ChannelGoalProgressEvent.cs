using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelGoal;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelgoalprogress">Twitch Documentation</see><br/>
/// When progress is made towards the specified broadcaster’s goal. Progress could be positive (added followers) or negative (lost followers).<br/>
/// Possible to receive Progress events before receiving the Begin event.<br/>
/// Required Scopes: channel:read:goals
/// </summary>
public class ChannelGoalProgressEvent
{
	/// <summary>
	/// An ID that identifies this event.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>
	///	The broadcaster’s user ID.
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user login. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user display name.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	/// <summary>
	/// The type of goal.
	/// </summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(GoalTypeEnumConverter))]
	public string? Type { get; set; }

	/// <summary>
	/// A description of the goal, if specified. The description may contain a maximum of 40 characters.
	/// </summary>
	[JsonPropertyName("description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// "The goal’s current value. The goal’s type determines how this value is increased or decreased.<br/>
	/// Follow: Set to the broadcaster's current number of followers. Increases with new followers and decreases when users unfollow.<br/>
	/// Subscription: Increased and decreased by the points value associated with the subscription tier. For example, if a tier-two subscription is worth 2 points, this field is increased or decreased by 2, not 1.<br/>
	/// SubscriptionCount: Increased by 1 for each new subscription and decreased by 1 for each user that unsubscribes.<br/>
	/// NewSubscription: Increased by the points value associated with the subscription tier. For example, if a tier-two subscription is worth 2 points, this field is increased by 2, not 1.<br/>
	/// NewSubscriptionCount: Increased by 1 for each new subscription.
	/// </summary>
	[JsonPropertyName("current_amount")]
	public int CurrentAmount { get; set; }

	/// <summary>
	/// The goal’s target value. For example, if the broadcaster has 200 followers before creating the goal, and their goal is to double that number, this field is set to 400.
	/// </summary>
	[JsonPropertyName("target_amount")]
	public int TargetAmount { get; set; }

	/// <summary>
	/// The timestamp which indicates when the broadcaster created the goal.<br/>
	/// (RFC3339 format converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset StartedAt { get; set; }
}