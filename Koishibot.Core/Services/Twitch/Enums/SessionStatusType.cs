using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(SessionStatusTypeEnumConverter))]
public enum SessionStatusType
{
	Connected = 1,
	Reconnecting
}

// == ⚫ == //

public class SessionStatusTypeEnumConverter : JsonConverter<SessionStatusType>
{
	public override SessionStatusType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"connected" => SessionStatusType.Connected,
			"reconnecting" => SessionStatusType.Reconnecting,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, SessionStatusType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			SessionStatusType.Connected => "connected",
			SessionStatusType.Reconnecting => "reconnecting",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}



//public enum SessionStatusType
//{
//	Connected = 1,
//	Reconnecting,
//	Keepalive,
//	Unknown
//}

//// == ⚫ == //

//public class SessionStatusTypeEnumConverter : JsonConverter<SessionStatusType>
//{
//	public override SessionStatusType Read
//		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//	{
//		var value = reader.GetString();
//		return value switch
//		{
//			"session_welcome" => SessionStatusType.Unknown,
//			"session_keepalive" => SessionStatusType.Keepalive,
//			"session_reconnect" => SessionStatusType.Reconnecting,
//			"connected" => SessionStatusType.Connected,
//			_ => throw new JsonException()
//		};
//	}

//	public override void Write
//		(Utf8JsonWriter writer, SessionStatusType value, JsonSerializerOptions options)
//	{
//		var mappedValue = value switch {
//			SessionStatusType.Unknown => "session_welcome",
//			SessionStatusType.Keepalive => "session_keepalive",
//			SessionStatusType.Reconnecting => "session_reconnect",
//			SessionStatusType.Connected => "connected",
//			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
//		};
//		if (writer.CurrentDepth.Equals(1))
//		{
//			writer.WriteStringValue(mappedValue);
//		}
//	}
//}