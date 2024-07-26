using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;

public enum EmoteType
{
	BitsTier = 1,
	Follower,
	Subscriptions
}

// == ⚫ == //

public class EmoteTypeEnumConverter : JsonConverter<EmoteType>
{
	public override EmoteType Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"bitstier" => EmoteType.BitsTier,
			"follower" => EmoteType.Follower,
			"subscriptions" => EmoteType.Subscriptions,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, EmoteType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			EmoteType.BitsTier => "bitstier",
			EmoteType.Follower => "follower",
			EmoteType.Subscriptions => "subscriptions",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}