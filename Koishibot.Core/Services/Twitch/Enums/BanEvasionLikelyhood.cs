using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

public enum BanEvasionLikelyhood
{
	Unknown,
	Possible,
	Likely
}

// == ⚫ == //

public class BanEvasionLikelyhoodEnumConverter : JsonConverter<BanEvasionLikelyhood>
{
	public override BanEvasionLikelyhood Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"unknown" => BanEvasionLikelyhood.Unknown,
			"possible" => BanEvasionLikelyhood.Possible,
			"likely" => BanEvasionLikelyhood.Likely,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, BanEvasionLikelyhood value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			BanEvasionLikelyhood.Unknown => "unknown",
			BanEvasionLikelyhood.Possible => "possible",
			BanEvasionLikelyhood.Likely => "likely",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}