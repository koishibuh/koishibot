using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(SourceContextEnumConverter))]
public enum SourceContext
{
	Chat = 1,
	Whisper
}

// == ⚫ == //

public class SourceContextEnumConverter : JsonConverter<SourceContext>
{
	public override SourceContext Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"chat" => SourceContext.Chat,
			"whisper" => SourceContext.Whisper,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, SourceContext value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			SourceContext.Chat => "chat",
			SourceContext.Whisper => "whispe",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}