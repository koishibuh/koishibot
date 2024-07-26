using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;


public enum AdLength
{
	ThirtySeconds,
	OneMinute,
	OneMinueThirtySeconds,
	TwoMinutes,
	TwoMinutesThirtySeconds,
	ThreeMinutes
}

// == ⚫ == //

public class AdLengthEnumConverter : JsonConverter<AdLength>
{
	public override AdLength Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetInt32();
		return value switch
		{
			30 => AdLength.ThirtySeconds,
			60 => AdLength.OneMinute,
			90 => AdLength.OneMinueThirtySeconds,
			120 => AdLength.TwoMinutes,
			150 => AdLength.TwoMinutesThirtySeconds,
			180 => AdLength.ThreeMinutes,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, AdLength value, JsonSerializerOptions options)
	{
		var intValue = value switch
		{
			AdLength.ThirtySeconds => 30,
			AdLength.OneMinute => 60,
			AdLength.OneMinueThirtySeconds => 90,
			AdLength.TwoMinutes => 120,
			AdLength.TwoMinutesThirtySeconds => 150,
			AdLength.ThreeMinutes => 180,
			_ => throw new JsonException()
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteNumberValue(intValue);
		}
	}
}