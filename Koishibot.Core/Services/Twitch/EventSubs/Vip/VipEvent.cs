namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Vip;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipadd">Twitch Documentation</see><br/>
/// When a VIP is added to the channel.<br/>
/// Required Scopes: channel:read:vips OR channel:manage:vips<br/><br/>
/// /// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelvipremove">Twitch Documentation</see><br/>
/// When a VIP is removed from the channel.<br/>
/// Required Scopes: channel:read:vips OR channel:manage:vips
/// </summary>
public class VipEvent
{
	/// <summary>
	///	The ID of the user who was added or removed as a VIP.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string ViewerId { get; set; } = string.Empty;

	/// <summary>
	/// The login of the user who was added or removed as a VIP. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string ViewerLogin { get; set; } = string.Empty;

	/// <summary>
	/// The display name of the user who was added or removed as a VIP.
	/// </summary>
	[JsonPropertyName("user_name")]
	public string ViewerName { get; set; } = string.Empty;

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
	public string BroadcasterUsername { get; set; } = string.Empty;
}
