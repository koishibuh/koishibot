using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodmessagehold">Twitch Documentation</see><br/>
/// When a message was caught by automod for review.<br/>
/// Required Scopes: moderator:manage:automod<br/>
/// </summary>
public class AutomodMessageHoldEvent
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
	public string Viewername { get; set; } = string.Empty;

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
	///The timestamp of when automod saved the message.<br/>
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("held_at")]
	[JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset HeldAt { get; set; }
}