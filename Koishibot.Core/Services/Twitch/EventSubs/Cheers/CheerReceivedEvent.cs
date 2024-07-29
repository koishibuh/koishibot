namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcheer">Twitch Documentation</see><br/>
/// When a user cheers on the specified channel.<br/>
/// Required Scopes: bits:read
/// </summary>
public class CheerReceivedEvent
{
	/// <summary>
	/// Whether the user cheered anonymously or not. (No longer in use)
	/// </summary>
	[JsonPropertyName("is_anonymous")]
	public bool IsAnonymous { get; set; }

	/// <summary>
	/// The user ID for the user who cheered on the specified channel.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string CheererId { get; set; } = string.Empty;

	/// <summary>
	/// The user login for the user who cheered on the specified channel. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string CheererLogin { get; set; } = string.Empty;

	/// <summary>
	/// The user display name for the user who cheered on the specified channel. 
	/// </summary>
	[JsonPropertyName("user_name")]
	public string CheererName { get; set; } = string.Empty;

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
	/// The message sent with the cheer.
	/// </summary>
	[JsonPropertyName("message")]
	public string Message { get; set; } = string.Empty;

	/// <summary>
	/// The number of bits cheered.
	/// </summary>
	[JsonPropertyName("bits")]
	public int BitAmount { get; set; }
}
