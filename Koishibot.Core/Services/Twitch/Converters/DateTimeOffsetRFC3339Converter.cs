using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Converters;

public class DateTimeOffsetRFC3339Converter : JsonConverter<DateTimeOffset>
{
	public override DateTimeOffset Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return DateTimeOffset.Parse(reader.GetString(), null, System.Globalization.DateTimeStyles.RoundtripKind);
	}

	public override void Write
		(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
	{
		// "o" is the format specifier for RFC3339
		writer.WriteStringValue(value.ToString("o")); 
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