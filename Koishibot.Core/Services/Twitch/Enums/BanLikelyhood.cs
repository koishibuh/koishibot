using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(BanLikelyhoodEnumConverter))]
public enum BanLikelyhood
{
	Unknown = 1,
	Possible,
	Likely
}

// == ⚫ == //

public class BanLikelyhoodEnumConverter : JsonConverter<BanLikelyhood>
{
	public override BanLikelyhood Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"unknown" => BanLikelyhood.Unknown,
			"possible" => BanLikelyhood.Possible,
			"likely" => BanLikelyhood.Likely,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, BanLikelyhood value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			BanLikelyhood.Unknown => "unknown",
			BanLikelyhood.Possible => "possible",
			BanLikelyhood.Likely => "likely",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}