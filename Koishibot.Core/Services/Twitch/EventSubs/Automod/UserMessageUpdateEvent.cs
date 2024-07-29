using Koishibot.Core.Services.Twitch.Common;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatuser_message_update">Twitch Documentation</see><br/>
/// When a user's message automod status is updated.<br/>
/// Required Scopes: user:read:chat scope<br/>
/// </summary>
public class UserMessageUpdateEvent
{
	///<summary>
	///The ID of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;


	///<summary>
	///The login of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;


	///<summary>
	///The user name of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;


	///<summary>
	///The User ID of the message sender.
	///</summary>
	[JsonPropertyName("user_id")]
	public string ViewerId { get; set; } = string.Empty;


	///<summary>
	///The message sender’s login.
	///</summary>
	[JsonPropertyName("user_login")]
	public string ViewerLogin { get; set; } = string.Empty;


	///<summary>
	///The message sender’s user name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string ViewerName { get; set; } = string.Empty;


	///<summary>
	///The message’s status. Possible values are:approved
	//denied
	//invalid
	///</summary>
	[JsonPropertyName("status")]
	public string Status { get; set; } = string.Empty;

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
}