using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs;

public class EventMessage<T> where T : class
{
	/// <summary>
	/// An object that identifies the message.
	/// </summary>
	[JsonPropertyName("metadata")]
	public Metadata Metadata { get; set; } = null!;

	[JsonPropertyName("payload")]
	public Payload<T>? Payload { get; set; }
}

///

public class Metadata
{
	/// <summary>
	/// An ID that uniquely identifies the message.<br/>
	/// Twitch may send this message twice.
	/// </summary>
	[JsonPropertyName("message_id")]
	public string MessageId { get; set; } = string.Empty;

	/// <summary>
	/// The type of message.
	/// </summary>
	[JsonPropertyName("message_type")]
	[JsonConverter(typeof(EventSubMessageTypeEnumConverter))]
	public EventSubMessageType Type { get; set; }

	/// <summary>
	/// The timestamp of when the message was sent.
	/// (Converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("message_timestamp")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset Timestamp { get; set; }

	/// <summary>
	/// The type of event sent in the message.
	/// </summary>
	[JsonPropertyName("subscription_type")]
	[JsonConverter(typeof(SubscriptionTypeEnumConverter))]
	public EventSubSubscriptionType SubscriptionType { get; set; }

	/// <summary>
	/// The version number of the subscription type's definition.<br/>
	/// This is the same value specified in the subscription request.
	/// </summary>
	[JsonPropertyName("subscription_version")]
	public string SubscriptionVersion { get; set; } = string.Empty;
}

///

public class Payload<T> where T : class
{
	/// <summary>
	/// Contains information about the connection (Welcome, Reconnect)
	/// </summary>
	[JsonPropertyName("session")]
	public Session? Session { get; set; }

	/// <summary>
	/// Contains information about your subscription. (Notification, Revocation)
	/// </summary>
	[JsonPropertyName("subscription")]
	public Subscription? Subscription { get; set; }

	/// <summary>
	/// The event’s data. (Notification)
	/// </summary>
	[JsonPropertyName("event")]
	public T? Event { get; set; }
}

///

public class Session
{
	/// <summary>
	/// An ID that uniquely identifies this WebSocket connection.<br/>
	/// Use this ID to set the session_id field in all subscription requests.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = null!;

	/// <summary>
	/// The connection’s status
	/// </summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(SessionStatusTypeEnumConverter))]
	public SessionStatusType SessionStatus { get; set; }

	/// <summary>
	/// The maximum number of seconds that you should expect silence before receiving a keepalive message.<br/>
	/// For a welcome message, this is the number of seconds that you have to subscribe to an event after receiving the welcome message.<br/>
	/// If you don’t subscribe to an event within this window, the socket is disconnected.
	/// </summary>
	[JsonPropertyName("keepalive_timeout_seconds")]
	public int KeepAliveTimeoutSeconds { get; set; }

	/// <summary>
	/// The URL to reconnect to if you get a Reconnect message.<br/>
	/// Is set to null.
	/// </summary>
	[JsonPropertyName("reconnect_url")]
	public string? ReconnectUrl { get; set; }

	/// <summary>
	/// The timestamp when the connection was created.</br>
	/// (Convert to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("connected_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset ConnectedAt { get; set; }
}

///

public class Subscription
{
	/// <summary>
	/// An ID that uniquely identifies this subscription.
	/// </summary>
	[JsonPropertyName("id")]
	public string? Id { get; set; }

	/// <summary>
	/// The subscription’s status.
	/// </summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(SubscriptionStatusTypeEnumConverter))]
	public SubscriptionStatusType Status { get; set; }

	/// <summary>
	/// The type of event sent in the message. See the event field.
	/// </summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(SubscriptionTypeEnumConverter))]
	public EventSubSubscriptionType Type { get; set; }

	/// <summary>
	/// The version number of the subscription type’s definition.
	/// </summary>
	[JsonPropertyName("version")]
	public string? Version { get; set; }

	/// <summary>
	/// The event’s cost.
	/// </summary>
	[JsonPropertyName("cost")]
	public byte Cost { get; set; }

	/// <summary>
	/// The conditions under which the event fires.
	/// </summary>
	[JsonPropertyName("condition")]
	public Condition? Condition { get; set; }

	/// <summary>
	/// Contains information about the transport used for notifications.
	/// </summary>
	[JsonPropertyName("transport")]
	public TransportMethod? Transport { get; set; }

	/// <summary>
	/// The timestamp when the subscription was created.
	/// (Converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset CreatedAt { get; set; }
}


public class Condition
{
	/// <summary>
	/// User ID of the broadcaster (channel).
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string? BroadcasterId { get; set; }

	/// <summary>
	/// Docs for CharityEventSub example show Broadcaster_Id
	/// </summary>
	[JsonPropertyName("broadcaster_id")]
	private string? BroadcasterId2 { set { BroadcasterId = value; } }

	/// <summary>
	/// User ID of the moderator.
	/// </summary>
	[JsonPropertyName("moderator_user_id")]
	public string? ModeratorUserId { get; set; }

	/// <summary>
	/// The ID of the user.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string? UserId { get; set; }

	/// <summary>
	/// User ID of the broadcaster that the event is happening to.
	/// </summary>
	[JsonPropertyName("to_broadcaster_user_id")]
	public string? ToBroadcasterId { get; set; }

	/// <summary>
	/// User ID of the broadcaster where the event is happening from.
	/// </summary>
	[JsonPropertyName("from_broadcaster_user_id")]
	public string? FromBroadcasterId { get; set; }

	/// <summary>
	/// Reward Id for to get notifications about specific reward.
	/// </summary>
	[JsonPropertyName("reward_id")]
	public string? RewardId { get; set; }
}