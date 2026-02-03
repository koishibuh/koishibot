using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json;
namespace Koishibot.Core.Services.Kofi.Enums;

[JsonConverter(typeof(KofiTypeEnumConverter))]
public enum KofiType
{
	Donation,
	Subscription,
	ShopOrder
}

public class KofiTypeEnumConverter : JsonConverter<KofiType>
{
	public override KofiType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"Donation" => KofiType.Donation,
			"Subscription" => KofiType.Subscription,
			"Shop Order" => KofiType.ShopOrder,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, KofiType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			KofiType.Donation => "Donation",
			KofiType.Subscription => "Subscription",
			KofiType.ShopOrder => "Shop Order",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}