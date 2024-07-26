using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;

public enum RewardRedemptionStatus
{
	CANCELED = 1,
	FULFILLED
}

// == ⚫ == //

public class RewardRedemptionStatusEnumConverter : JsonConverter<RewardRedemptionStatus>
{
	public override RewardRedemptionStatus Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"CANCELED" => RewardRedemptionStatus.CANCELED,
			"FULFILLED" => RewardRedemptionStatus.FULFILLED,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, RewardRedemptionStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			RewardRedemptionStatus.CANCELED => "CANCELED",
			RewardRedemptionStatus.FULFILLED => "FULFILLED",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}