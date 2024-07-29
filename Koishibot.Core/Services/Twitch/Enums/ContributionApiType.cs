using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(ContributionApiTypeEnumConverter))]
public enum ContributionApiType
{
	Bits = 1,
	Subs,
	Other
}

// == ⚫ == //

public class ContributionApiTypeEnumConverter : JsonConverter<ContributionApiType>
{
	public override ContributionApiType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"BITS" => ContributionApiType.Bits,
			"SUBS" => ContributionApiType.Subs,
			"OTHER" => ContributionApiType.Other,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, ContributionApiType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			ContributionApiType.Bits => "BITS",
			ContributionApiType.Subs => "SUBS",
			ContributionApiType.Other => "OTHER",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}