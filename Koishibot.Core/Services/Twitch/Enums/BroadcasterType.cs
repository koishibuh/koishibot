using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;

public enum BroadcasterType
{
	Partner,
	Affiliate,
	Default
}

// == ⚫ == //

public class BroadcasterTypeEnumConverter : JsonConverter<BroadcasterType>
{
	public override BroadcasterType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"partner" => BroadcasterType.Partner,
			"affiliate" => BroadcasterType.Affiliate,
			"default" => BroadcasterType.Default,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, BroadcasterType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			BroadcasterType.Partner => "partner",
			BroadcasterType.Affiliate => "affiliate",
			BroadcasterType.Default => "default",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}