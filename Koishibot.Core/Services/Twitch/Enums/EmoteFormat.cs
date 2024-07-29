using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;


[JsonConverter(typeof(EmoteFormatEnumConverter))]
public enum EmoteFormat
{
	Animated = 1,
	Static
}

// == ⚫ == //

public class EmoteFormatEnumConverter : JsonConverter<EmoteFormat>
{
	public override EmoteFormat Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"animated" => EmoteFormat.Animated,
			"static" => EmoteFormat.Static,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, EmoteFormat value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			EmoteFormat.Animated => "animated",
			EmoteFormat.Static => "static",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}