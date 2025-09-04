using Koishibot.Core.Services.Twitch.Common;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;


///<summary>
///<see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshared_chatupdate">Twitch Documentation</see><br/>
///Sends a notification when the active shared chat session the channel is in changes.<br/>
///</summary>
public class SharedChatUpdateEvent
{
	///<summary>
	/// The unique identifier for the shared chat session.
	///</summary>
	[JsonPropertyName("session_id")]
	public string SessionId { get; set; } = string.Empty;

	///<summary>
	///The User ID of the channel in the subscription condition which is now active in the shared chat session.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The display name of the channel in the subscription condition which is now active in the shared chat session.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	///<summary>
	///The user login of the channel in the subscription condition which is now active in the shared chat session. (Lowercase)
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;
	
	///<summary>
	///The User ID of the host channel.
	///</summary>
	[JsonPropertyName("host_broadcaster_user_id")]
	public string HostBroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The display name of the host channel.
	///</summary>
	[JsonPropertyName("host_broadcaster_user_name")]
	public string HostBroadcasterName { get; set; } = string.Empty;

	///<summary>
	///The user login of the host channel. (Lowercase)
	///</summary>
	[JsonPropertyName("host_broadcaster_user_login")]
	public string HostBroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	/// The list of participants in the session.
	///</summary>
	[JsonPropertyName("participants")]
	public List<Participant> Participants { get; set; }
}

