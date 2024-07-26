using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;


public enum PredictionColor
{
	Pink = 1,
	Blue
}

// == ⚫ == //

public class PredictionColorEnumConverter : JsonConverter<PredictionColor>
{
	public override PredictionColor Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"PINK" => PredictionColor.Pink,
			"BLUE" => PredictionColor.Blue,
			_ => throw new JsonException()
		};
	}

	public override void Write(Utf8JsonWriter writer, PredictionColor value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			PredictionColor.Pink => "PINK",
			PredictionColor.Blue => "BLUE",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}