using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;

// ACTIVE — The Prediction is running and viewers can make predictions.
// CANCELED — The broadcaster canceled the Prediction and refunded the Channel Points to the participants.
// LOCKED — The broadcaster locked the Prediction, which means viewers can no longer make predictions.
// RESOLVED — The winning outcome was determined and the Channel Points were distributed to the viewers who predicted the correct outcome.

public enum PredictionApiStatus
{
	Active = 1,
	Canceled,
	Locked,
	Resolved
}

// == ⚫ == //

public class PredictionApiStatusEnumConverter : JsonConverter<PredictionApiStatus>
{
	public override PredictionApiStatus Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"ACTIVE" => PredictionApiStatus.Active,
			"CANCELED" => PredictionApiStatus.Canceled,
			"LOCKED" => PredictionApiStatus.Locked,
			"RESOLVED" => PredictionApiStatus.Resolved,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, PredictionApiStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			PredictionApiStatus.Active => "ACTIVE",
			PredictionApiStatus.Canceled => "CANCELED",
			PredictionApiStatus.Locked => "LOCKED",
			PredictionApiStatus.Resolved => "RESOLVED",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}