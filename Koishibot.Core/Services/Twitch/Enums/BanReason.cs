using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;
public enum BanReason
{
	Harassment = 1,
	Spam,
	Other
}

// == ⚫ == //

public class BanReasonEnumConverter : JsonConverter<BanReason>
{
	public override BanReason Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"harassment" => BanReason.Harassment,
			"spam" => BanReason.Spam,
			"other" => BanReason.Other,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, BanReason value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			BanReason.Harassment => "harassment",
			BanReason.Spam => "spam",
			BanReason.Other => "other",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}