using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.EventSubs.Converters;

public class TimeSpanSecondsConverter : JsonConverter<TimeSpan>
{
	public override TimeSpan Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.Number)
		{
			throw new JsonException("Expected a number");
		}

		int seconds = reader.GetInt32();
		return TimeSpan.FromSeconds(seconds);
	}

	public override void Write
		(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
	{
		writer.WriteNumberValue((int)value.TotalSeconds);
	}
}