using Koishibot.Core.Services.Twitch.Common;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatuser_message_hold">Twitch Documentation</see><br/>
/// When a user's message is caught by automod.<br/>
/// Required Scopes: user:read:chat scope<br/>
/// </summary>
public class UserMessageHoldEvent
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
    public string UserId { get; set; } = string.Empty;


	///<summary>
	///The message sender’s login.
	///</summary>
	[JsonPropertyName("user_login")]
    public string UserLogin { get; set; } = string.Empty;


	///<summary>
	///The message sender’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
    public string UserName { get; set; } = string.Empty;


	///<summary>
	///The ID of the message that was flagged by automod.
	///</summary>
	[JsonPropertyName("message_id")]
    public string MessageId { get; set; } = string.Empty;


	///<summary>
	///The body of the message.
	///</summary>
	[JsonPropertyName("message")]
    public List<Message> Message { get; set; }
}