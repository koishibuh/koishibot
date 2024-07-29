namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear_user_messages">Twitch Documentation</see><br/>
/// When a moderator or bot clears all messages for a specific user.<br/>
/// Required Scopes: user:read:chat, user:bot abd channel:bot (app access token)
/// </summary>
public class UserMessagesClearedEvent
{
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
	///	The ID of the user that was banned or put in a timeout. All of their messages are deleted.
	/// </summary>
	[JsonPropertyName("target_user_id")]
	public string ViewerUserId { get; set; } = string.Empty;

	/// <summary>
	/// The user login of the user that was banned or put in a timeout. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string ViewerUserLogin { get; set; } = string.Empty;

	/// <summary>
	/// The user name of the user that was banned or put in a timeout.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string ViewerUserName { get; set; } = string.Empty;
}
