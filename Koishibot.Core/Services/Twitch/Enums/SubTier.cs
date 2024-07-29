using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(SubTierEnumConverter))]
public enum SubTier
{
	Tier1 = 1, // Or Prime
	Tier2,
	Tier3
}

// == ⚫ == //

public class SubTierEnumConverter : JsonConverter<SubTier>
{
	public override SubTier Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"1000" => SubTier.Tier1,
			"2000" => SubTier.Tier2,
			"3000" => SubTier.Tier3,
			_ => throw new JsonException()
		};
	}

	public override void Write(Utf8JsonWriter writer, SubTier value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			SubTier.Tier1 => "1000",
			SubTier.Tier2 => "2000",
			SubTier.Tier3 => "3000",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}