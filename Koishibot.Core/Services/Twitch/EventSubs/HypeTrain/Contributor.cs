using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

/// <summary>
/// <see href=" https://dev.twitch.tv/docs/eventsub/eventsub-reference/#top-contributions">Twitch Documentation</see><br/>
/// The top contributor for a contribution type. <br/>
/// For example, the top contributor using BITS (by aggregate) or the top contributor using subscriptions (by count).
/// </summary>
///
public class Contributor
{
	/// <summary>
	/// The ID of the user that made the contribution.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string? UserId { get; set; }

	/// <summary>
	/// The user's login name. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string? UserLogin { get; set; }

	/// <summary>
	/// The user's display name.
	/// </summary>
	[JsonPropertyName("user_name")]
	public string? UserName { get; set; }

	/// <summary>
	/// The contribution method used.
	/// </summary>
	[JsonPropertyName("type")]
	public ContributionEventSubType Type { get; set; }
	
	/// <summary>
	/// The total amount contributed.<br/>
	/// If type is bits, total represents the amount of Bits used.<br/>
	/// If type is subscription, total is 500, 1000, or 2500 to represent tier 1, 2, or 3 subscriptions, respectively.
	/// </summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }
}