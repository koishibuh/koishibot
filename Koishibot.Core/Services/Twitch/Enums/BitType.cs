using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;


[JsonConverter(typeof(BitTypeEnumConverter))]
public enum BitType
{
	Cheer,
	PowerUp
}

// == ⚫ == //

public class BitTypeEnumConverter : JsonConverter<BitType>
{
	public override BitType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"cheer" => BitType.Cheer,
			"power_up" => BitType.PowerUp,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, BitType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			BitType.Cheer => "cheer",
			BitType.PowerUp => "power_up",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}