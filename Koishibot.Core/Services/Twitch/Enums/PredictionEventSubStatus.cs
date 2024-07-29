using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(PredictionEventSubStatusEnumConverter))]
public enum PredictionEventSubStatus
{
	Resolved = 1,
	Canceled
}

// == ⚫ == //

public class PredictionEventSubStatusEnumConverter : JsonConverter<PredictionEventSubStatus>
{
	public override PredictionEventSubStatus Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"resolved" => PredictionEventSubStatus.Resolved,
			"canceled" => PredictionEventSubStatus.Canceled,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, PredictionEventSubStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			PredictionEventSubStatus.Resolved => "resolved",
			PredictionEventSubStatus.Canceled => "canceled",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}