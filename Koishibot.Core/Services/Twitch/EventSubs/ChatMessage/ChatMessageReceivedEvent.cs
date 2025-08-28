using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchatmessage">Twitch Documentation</see><br/>
/// When any user sends a message to a specific chat room.<br/>
/// Required Scopes: user:read:chat<br/>
/// </summary>
public class ChatMessageReceivedEvent
{
	///<summary>
	///The broadcaster user ID.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The broadcaster display name.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	///<summary>
	///The broadcaster login. (Lowercase)
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	///The user ID of the user that sent the message.
	///</summary>
	[JsonPropertyName("chatter_user_id")]
	public string ChatterId { get; set; } = string.Empty;

	///<summary>
	///The user name of the user that sent the message.
	///</summary>
	[JsonPropertyName("chatter_user_name")]
	public string ChatterName { get; set; } = string.Empty;

	///<summary>
	///The user login of the user that sent the message. (Lowercase)
	///</summary>
	[JsonPropertyName("chatter_user_login")]
	public string ChatterLogin { get; set; } = string.Empty;

	///<summary>
	///A UUID that identifies the message.
	///</summary>
	[JsonPropertyName("message_id")]
	public string MessageId { get; set; } = string.Empty;

	///<summary>
	///The structured chat message.
	///</summary>
	[JsonPropertyName("message")]
	public Message Message { get; set; } = null!;

	///<summary>
	///The type of message.
	///</summary>
	[JsonPropertyName("message_type")]
	[JsonConverter(typeof(MessageTypeEnumConverter))]
	public MessageType MessageType { get; set; }

	///<summary>
	///List of chat badges.
	///</summary>
	[JsonPropertyName("badges")]
	public List<Badge> Badges { get; set; } = [];

	///<summary>
	///Optional. Metadata if this message is a cheer.
	///</summary>
	[JsonPropertyName("cheer")]
	public Cheer? Cheer { get; set; }

	///<summary>
	///The color of the user’s name in the chat room. This is a hexadecimal RGB color code in the form, #&lt;RGB&gt;. This tag may be empty if it is never set.
	///</summary>
	[JsonPropertyName("color")]
	public string Color { get; set; } = "#000000";

	///<summary>
	///Optional. Metadata if this message is a reply.
	///</summary>
	[JsonPropertyName("reply")]
	public Reply? Reply { get; set; }

	///<summary>
	///Optional. The ID of a channel points custom reward that was redeemed.
	///</summary>
	[JsonPropertyName("channel_points_custom_reward_id")]
	public string? ChannelPointsCustomRewardId { get; set; }

	///<summary>
	///Optional. The broadcaster user ID of the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_broadcaster_user_id")]
	public string? SourceBroadcasterUserId { get; set; }
	
	///<summary>
	///Optional. The user name of the broadcaster of the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_broadcaster_user_name")]
	public string? SourceBroadcasterUsername { get; set; }
	
	///<summary>
	///Optional. The login of the broadcaster of the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_broadcaster_user_login")]
	public string? SourceBroadcasterUserLogin { get; set; }
	
	///<summary>
	///Optional. The UUID that identifies the source message from the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_message_id")]
	public string? SourceMessageId { get; set; }
	
	///<summary>
	///Optional. The list of chat badges for the chatter in the channel the message was sent from. Is null when the message happens in the same channel as the broadcaster.<br/>
	///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
	///</summary>
	[JsonPropertyName("source_badges")]
	public List<Badge>? SourceBadges { get; set; }
	
	///<summary>
	///Optional. Determines if a message delivered during a shared chat session is only sent to the source channel.<br/>
	///Has no effect if the message is not sent during a shared chat session.
	///</summary>
	[JsonPropertyName("is_source_only")]
	public bool IsSourceOnly { get; set; }
}


public class Cheer
{
	///<summary>
	///The amount of Bits the user cheered.
	///</summary>
	[JsonPropertyName("bits")]
	public int Bits { get; set; }
}


public class Reply
{
	///<summary>
	///An ID that uniquely identifies the parent message that this message is replying to.
	///</summary>
	[JsonPropertyName("parent_message_id")]
	public string? ParentMessageId { get; set; }

	///<summary>
	///The message body of the parent message.
	///</summary>
	[JsonPropertyName("parent_message_body")]
	public string? ParentMessageBody { get; set; }

	///<summary>
	///User ID of the sender of the parent message.
	///</summary>
	[JsonPropertyName("parent_user_id")]
	public string? ParentUserId { get; set; }

	///<summary>
	///User name of the sender of the parent message.
	///</summary>
	[JsonPropertyName("parent_user_name")]
	public string? ParentUserName { get; set; }

	///<summary>
	///User login of the sender of the parent message.
	///</summary>
	[JsonPropertyName("parent_user_login")]
	public string? ParentUserLogin { get; set; }

	///<summary>
	///An ID that identifies the parent message of the reply thread.
	///</summary>
	[JsonPropertyName("thread_message_id")]
	public string? ThreadMessageId { get; set; }

	///<summary>
	///User ID of the sender of the thread’s parent message.
	///</summary>
	[JsonPropertyName("thread_user_id")]
	public string? ThreadUserId { get; set; }

	///<summary>
	///User name of the sender of the thread’s parent message.
	///</summary>
	[JsonPropertyName("thread_user_name")]
	public string? ThreadUserName { get; set; }

	///<summary>
	///User login of the sender of the thread’s parent message.
	///</summary>
	[JsonPropertyName("thread_user_login")]
	public string? ThreadUserLogin { get; set; }
}
