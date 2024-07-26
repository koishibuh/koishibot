using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchEventSubNew.Enums;
public enum MessageType : byte
{
	SessionWelcome = 1,
	SessionKeepalive = 2,
	Notification = 3,
	SessionReconnect = 4,
	Revocation = 5,
}

// == ⚫ == //

public class MessageTypeEnumConverter : JsonConverter<MessageType>
{
	public override MessageType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"session_welcome" => MessageType.SessionWelcome,
			"session_keepalive" => MessageType.SessionKeepalive,
			"notification" => MessageType.Notification,
			"session_reconnect" => MessageType.SessionReconnect,
			"revocation" => MessageType.Revocation,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, MessageType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			MessageType.SessionWelcome => "session_welcome",
			MessageType.SessionKeepalive => "session_keepalive",
			MessageType.Notification => "notification",
			MessageType.SessionReconnect => "session_reconnect",
			MessageType.Revocation => "revocation",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}