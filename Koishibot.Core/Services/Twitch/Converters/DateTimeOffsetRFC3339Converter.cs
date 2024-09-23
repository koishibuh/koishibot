using System.Globalization;
using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Converters;

public class DateTimeOffsetRFC3339Converter : JsonConverter<DateTimeOffset?>
{
	public override DateTimeOffset? Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return null;
		}

		return DateTimeOffset.Parse(reader.GetString(), null, System.Globalization.DateTimeStyles.RoundtripKind);
	}

	public override void Write
		(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
	{
		// "o" is the format specifier for RFC3339
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

public class DateTimeRFC3339Converter : JsonConverter<DateTime>
{
	public override DateTime Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateTime.Parse(reader.GetString(), null, System.Globalization.DateTimeStyles.RoundtripKind);
	}

	public override void Write
		(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString("o"));
	}
}


public class NEWDateTimeOffsetRfc3339Converter : JsonConverter<DateTimeOffset>
{
	private const string Rfc3339Format = "yyyy-MM-dd'T'HH:mm:ss.fffK";

	public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var dateString = reader.GetString();
		if (DateTimeOffset.TryParseExact(dateString, Rfc3339Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
		{
			return date;
		}
		throw new JsonException($"Unable to parse {dateString} as a valid RFC 3339 date.");
	}

	public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString(Rfc3339Format, CultureInfo.InvariantCulture));
	}
}