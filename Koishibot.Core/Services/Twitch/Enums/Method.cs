using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

public enum Method
{
	Websocket = 1,
	Webhook
}

// == ⚫ == //

public class MethodEnumConverter : JsonConverter<Method>
{
	public override Method Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"websocket" => Method.Websocket,
			"webhook" => Method.Webhook,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, Method value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			Method.Websocket => "websocket",
			Method.Webhook => "webhook",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}