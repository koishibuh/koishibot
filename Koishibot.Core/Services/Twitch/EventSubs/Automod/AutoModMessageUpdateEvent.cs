using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodmessageupdate">Twitch Documentation</see><br/>
/// When a message in the automod queue has its status changed.<br/>
/// Required Scopes: moderator:manage:automod<br/>
/// </summary>
public class AutoModMessageUpdateEvent
{
	///<summary>
	///The ID of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The login of the broadcaster specified in the request. (Lowercase)
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	///The user name of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	///<summary>
	///The message sender’s user ID.
	///</summary>
	[JsonPropertyName("user_id")]
	public string ViewerId { get; set; } = string.Empty;

	///<summary>
	///The message sender’s login name. (Lowercase)
	///</summary>
	[JsonPropertyName("user_login")]
	public string ViewerLogin { get; set; } = string.Empty;

	///<summary>
	///The message sender’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string ViewerName { get; set; } = string.Empty;

	///<summary>
	///The ID of the moderator.
	///</summary>
	[JsonPropertyName("moderator_user_id")]
	public string ModeratorId { get; set; } = string.Empty;

	///<summary>
	///TThe moderator’s user name.
	///</summary>
	[JsonPropertyName("moderator_user_name")]
	public string ModeratorName { get; set; } = string.Empty;

	///<summary>
	///The login of the moderator.
	///</summary>
	[JsonPropertyName("moderator_user_login")]
	public string ModeratorLogin { get; set; } = string.Empty;

	///<summary>
	///The ID of the message that was flagged by automod.
	///</summary>
	[JsonPropertyName("message_id")]
	public string MessageId { get; set; } = string.Empty;

	///<summary>
	///The body of the message.
	///</summary>
	[JsonPropertyName("message")]
	public List<Message> Message { get; set; } = null!;

	///<summary>
	///The category of the message.
	///</summary>
	[JsonPropertyName("category")]
	public string Category { get; set; } = string.Empty;

	///<summary>
	///The level of severity. Measured between 1 to 4.
	///</summary>
	[JsonPropertyName("level")]
	public int Level { get; set; }

	///<summary>
	///The message’s status.
	///</summary>
	[JsonPropertyName("status")]
	public AutomodMessageStatus Status { get; set; }

	///<summary>
	///The timestamp of when automod saved the message.<br/>
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("held_at")]
	public DateTimeOffset HeldAt { get; set; }
}