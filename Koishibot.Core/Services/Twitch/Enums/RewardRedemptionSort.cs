using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;

public enum RewardRedemptionSort
{
	Oldest = 1,
	Newest
}

// == ⚫ == //

public class RewardRedemptionSortEnumConverter : JsonConverter<RewardRedemptionSort>
{
	public override RewardRedemptionSort Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"OLDEST" => RewardRedemptionSort.Oldest,
			"NEWEST" => RewardRedemptionSort.Newest,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, RewardRedemptionSort value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			RewardRedemptionSort.Oldest => "OLDEST",
			RewardRedemptionSort.Newest => "NEWEST",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}