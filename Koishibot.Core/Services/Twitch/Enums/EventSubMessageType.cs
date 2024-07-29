using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(EventSubMessageTypeEnumConverter))]
public enum EventSubMessageType : byte
{
	/// <summary>
	/// Message received after connecting to the server and contains the socket’s ID.
	/// </summary>
	SessionWelcome = 1,
	/// <summary>
	/// Indicates that the connection is healthy.
	/// </summary>
	SessionKeepalive = 2,
	/// <summary>
	/// Contains the event’s data.
	/// </summary>
	Notification = 3,
	/// <summary>
	/// Indicates that the EventSub WebSocket server requires the client to reconnect.
	/// </summary>
	SessionReconnect = 4,
	/// <summary>
	/// Contains the reason why Twitch revoked your subscription.
	/// </summary>
	Revocation = 5,
}

// == ⚫ == //

public class EventSubMessageTypeEnumConverter : JsonConverter<EventSubMessageType>
{
	public override EventSubMessageType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"session_welcome" => EventSubMessageType.SessionWelcome,
			"session_keepalive" => EventSubMessageType.SessionKeepalive,
			"notification" => EventSubMessageType.Notification,
			"session_reconnect" => EventSubMessageType.SessionReconnect,
			"revocation" => EventSubMessageType.Revocation,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, EventSubMessageType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			EventSubMessageType.SessionWelcome => "session_welcome",
			EventSubMessageType.SessionKeepalive => "session_keepalive",
			EventSubMessageType.Notification => "notification",
			EventSubMessageType.SessionReconnect => "session_reconnect",
			EventSubMessageType.Revocation => "revocation",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}