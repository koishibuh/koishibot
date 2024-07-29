using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(ContributionEventSubTypeEnumConverter))]
public enum ContributionEventSubType
{
	Bits = 1,
	Subscription,
	Other
}

// == ⚫ == //

public class ContributionEventSubTypeEnumConverter : JsonConverter<ContributionEventSubType>
{
	public override ContributionEventSubType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"bits" => ContributionEventSubType.Bits,
			"subscription" => ContributionEventSubType.Subscription,
			"other" => ContributionEventSubType.Other,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, ContributionEventSubType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			ContributionEventSubType.Bits => "bits",
			ContributionEventSubType.Subscription => "subscription",
			ContributionEventSubType.Other => "other",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}