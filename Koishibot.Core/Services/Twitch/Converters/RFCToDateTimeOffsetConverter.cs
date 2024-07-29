using System.Globalization;
using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Converters;

public class RFCToDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
	public override DateTimeOffset Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Number)
		{
			var unixTimestamp = reader.GetInt64();
			return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
		}
		else if (reader.TokenType == JsonTokenType.String)
		{
			var value = reader.GetString();
			if (value == "0")
			{
				//return null;
				return DateTimeOffset.UtcNow;
			}

			return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
		}
		else
		{
			throw new JsonException("Expected a string or number, but got " + reader.TokenType);
		}
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString("o"));
	}
}