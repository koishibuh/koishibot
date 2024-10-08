﻿using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(PredictionColorEnumConverter))]
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
			"pink" => PredictionColor.Pink,
			"BLUE" => PredictionColor.Blue,
			"blue" => PredictionColor.Blue,
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