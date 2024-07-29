namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.BanUser;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#channel-unban-event">Twitch Documentation</see><br/>
/// When a viewer is unbanned from the specified channel.<br/>
/// Required Scopes: channel:moderate
/// </summary>
public class UnbannedUserEvent
{
	/// <summary>
	/// The user id for the user who was unbanned on the specified channel.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string UnbannedUserId { get; set; } = string.Empty;

	/// <summary>
	/// The user login for the user who was unbanned on the specified channel. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string UnbannedUserLogin { get; set; } = string.Empty;

	/// <summary>
	/// The user display name for the user who was unbanned on the specified channel.
	/// </summary>
	[JsonPropertyName("user_name")]
	public string UnbannedUserName { get; set; } = string.Empty;

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
	/// The user ID of the issuer of the unban.
	/// </summary>
	[JsonPropertyName("moderator_user_id")]
	public string ModeratorId { get; set; } = string.Empty;

	/// <summary>
	/// The user login of the issuer of the unban. (Lowercase)
	/// </summary>
	[JsonPropertyName("moderator_user_login")]
	public string ModeratorLogin { get; set; } = string.Empty;

	/// <summary>
	/// The user name of the issuer of the unban.
	/// </summary>
	[JsonPropertyName("moderator_user_name")]
	public string ModeratorName { get; set; } = string.Empty;
}