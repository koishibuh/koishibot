namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DeleteMessages;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatclear">Twitch Documentation</see><br/>
/// When a moderator or bot clears all messages from the chat room.<br/>
/// Required Scopes: user:read:chat, user:bot abd channel:bot (app access token)
/// </summary>
public class ChatClearedEvent
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
}