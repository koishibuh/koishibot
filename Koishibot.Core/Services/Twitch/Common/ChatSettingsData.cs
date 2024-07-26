using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Common;

public class ChatSettingsData
{
	///<summary>
	///The ID of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///A Boolean value that determines whether chat messages must contain only emotes.<br/>
	///Is true if chat messages may contain only emotes; otherwise, false.
	///</summary>
	[JsonPropertyName("emote_mode")]
	public bool EmoteMode { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster restricts the chat room to followers only.<br/>
	///Is true if the broadcaster restricts the chat room to followers only; otherwise, false.<br/>
	///See the follower_mode_duration field for how long users must follow the broadcaster before being able to participate in the chat room.
	///</summary>
	[JsonPropertyName("follower_mode")]
	public bool FollowerMode { get; set; }

	///<summary>
	///The length of time, in minutes, that users must follow the broadcaster before being able to participate in the chat room.<br/>
	///Is null if follower_mode is false.
	///</summary>
	[JsonPropertyName("follower_mode_duration")]
	public int FollowerModeDurationInMinutes { get; set; }

	///<summary>
	///The moderator’s ID.<br/>
	///The response includes this field only if the request specifies a user access token that includes the moderator:read:chat_settings scope.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster adds a short delay before chat messages appear in the chat room.<br/>
	///This gives chat moderators and bots a chance to remove them before viewers can see the message.<br/>
	///See the non_moderator_chat_delay_duration field for the length of the delay.<br/>
	///Is true if the broadcaster applies a delay; otherwise, false.<br/>
	///The response includes this field only if the request specifies a user access token that includes the moderator:read:chat_settings scope and the user in the moderator_id query parameter is one of the broadcaster’s moderators.
	///</summary>
	[JsonPropertyName("non_moderator_chat_delay")]
	public bool NonModeratorChatDelay { get; set; }

	///<summary>
	///The amount of time, in seconds, that messages are delayed before appearing in chat.<br/>
	///Is null if non_moderator_chat_delay is false.<br/>
	///The response includes this field only if the request specifies a user access token that includes the moderator:read:chat_settings scope and the user in the moderator_id query parameter is one of the broadcaster’s moderators.
	///</summary>
	[JsonPropertyName("non_moderator_chat_delay_duration")]
	[JsonConverter(typeof(ChatDelayEnumConverter))]
	public int NonModeratorChatDelayDurationInSeconds { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster limits how often users in the chat room are allowed to send messages.<br/>
	///Is true if the broadcaster applies a delay; otherwise, false.<br/>
	///See the slow_mode_wait_time field for the delay.
	///</summary>
	[JsonPropertyName("slow_mode")]
	public bool SlowMode { get; set; }

	///<summary>
	///The amount of time, in seconds, that users must wait between sending messages.<br/>
	///Is null if slow_mode is false.
	///</summary>
	[JsonPropertyName("slow_mode_wait_time")]
	public int SlowModeWaitTimeInSeconds { get; set; }

	///<summary>
	///A Boolean value that determines whether only users that subscribe to the broadcaster’s channel may talk in the chat room.<br/>
	///Is true if the broadcaster restricts the chat room to subscribers only; otherwise, false.
	///</summary>
	[JsonPropertyName("subscriber_mode")]
	public bool SubscriberMode { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster requires users to post only unique messages in the chat room.<br/>
	///Is true if the broadcaster requires unique messages only; otherwise, false.
	///</summary>
	[JsonPropertyName("unique_chat_mode")]
	public bool UniqueChatMode { get; set; }
}