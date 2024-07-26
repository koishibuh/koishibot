using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchEventSubNew.Enums;

public enum SessionStatusType
{
	Connected = 1,
	Reconnecting,
	Keepalive,
	Unknown
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
			"session_welcome" => SessionStatusType.Unknown,
			"session_keepalive" => SessionStatusType.Keepalive,
			"session_reconnect" => SessionStatusType.Reconnecting,
			"connected" => SessionStatusType.Connected,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, SessionStatusType value, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}
}