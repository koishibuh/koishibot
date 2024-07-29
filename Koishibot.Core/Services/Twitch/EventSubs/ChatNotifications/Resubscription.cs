using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;
public class Resubscription
{
	/// <summary>
	/// The total number of months the user has subscribed.
	/// </summary>
	[JsonPropertyName("cumulative_months")]
	public int CumulativeMonths { get; set; }

	/// <summary>
	/// The number of months the subscription is for.
	/// </summary>
	[JsonPropertyName("duration_months")]
	public int DurationMonths { get; set; }

	/// <summary>
	/// The total number of months the user has subscribed.
	/// </summary>
	[JsonPropertyName("streak_months")]
	public int StreakMonths { get; set; }

	/// <summary>
	/// The type of subscription plan being used.
	/// </summary>
	[JsonPropertyName("sub_tier")]
	public SubTier SubTier { get; set; }

	/// <summary>
	/// Optional. The number of consecutive months the user has subscribed.
	/// </summary>
	[JsonPropertyName("is_prime")]
	public bool? IsPrime { get; set; }

	/// <summary>
	/// Whether or not the resub was a result of a gift.
	/// </summary>
	[JsonPropertyName("is_gift")]
	public bool IsGift { get; set; }

	/// <summary>
	/// Optional. Whether or not the gift was anonymous.
	/// </summary>
	[JsonPropertyName("gifter_is_anonymous")]
	public bool? GifterIsAnonymous { get; set; }

	/// <summary>
	/// Optional. The user ID of the subscription gifter. Null if anonymous.
	/// </summary>
	[JsonPropertyName("gifter_user_id")]
	public string? GifterUserId { get; set; }

	/// <summary>
	/// Optional. The user name of the subscription gifter. Null if anonymous.
	/// </summary>
	[JsonPropertyName("gifter_user_name")]
	public string? GifterUserName { get; set; }

	/// <summary>
	/// Optional. The user login of the subscription gifter. Null if anonymous.
	/// </summary>
	[JsonPropertyName("gifter_user_login")]
	public string? GifterUserLogin { get; set; }
}