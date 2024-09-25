using System.Globalization;
using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Converters;

public class RFCToDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
	public override DateTimeOffset? Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		switch (reader.TokenType)
		{
			case JsonTokenType.Number:
			{
				var unixTimestamp = reader.GetInt64();
				return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
			}
			case JsonTokenType.String:
			{
				var value = reader.GetString();
				if (value == "0")
				{
					return null;
					// return DateTimeOffset.UtcNow;
				}

				return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
			}
			default:
				throw new JsonException("Expected a string or number, but got " + reader.TokenType);
		}
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
	{
		if (value.HasValue)
		{
			writer.WriteStringValue(value.Value.ToString("o"));
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}