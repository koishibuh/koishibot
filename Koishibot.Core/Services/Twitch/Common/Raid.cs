namespace Koishibot.Core.Services.Twitch.Common;
public class Raid
{
	/// <summary>
	/// The user ID of the broadcaster raiding this channel.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; } = string.Empty;

	/// <summary>
	/// The user name of the broadcaster raiding this channel.
	/// </summary>
	[JsonPropertyName("user_name")]
	public string UserName { get; set; } = string.Empty;

	/// <summary>
	/// The login name of the broadcaster raiding this channel.
	/// </summary> 
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; } = string.Empty;

	/// <summary>
	/// The number of viewers raiding this channel from the broadcaster’s channel.
	/// </summary>
	[JsonPropertyName("viewer_count")]
	public int ViewerCount { get; set; }

	/// <summary>
	/// Profile image URL of the broadcaster raiding this channel.
	/// (On ChatNotification EventSub)
	/// </summary>
	[JsonPropertyName("profile_image_url")]
	public string? ProfileImageUrl { get; set; }
}