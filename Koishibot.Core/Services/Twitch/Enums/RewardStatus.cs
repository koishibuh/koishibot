using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(RewardStatusEnumConverter))]
public enum RewardStatus
{
	Unknown = 1,
	Unfulfilled,
	Fulfilled,
	Canceled
}

// == ⚫ == //

public class RewardStatusEnumConverter : JsonConverter<RewardStatus>
{
	public override RewardStatus Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"unknown" => RewardStatus.Unknown,
			"unfulfilled" => RewardStatus.Unfulfilled,
			"fulfilled" => RewardStatus.Fulfilled,
			"canceled" => RewardStatus.Canceled,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, RewardStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			RewardStatus.Unknown => "unknown",
			RewardStatus.Unfulfilled => "unfulfilled",
			RewardStatus.Fulfilled => "fulfilled",
			RewardStatus.Canceled => "canceled",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}