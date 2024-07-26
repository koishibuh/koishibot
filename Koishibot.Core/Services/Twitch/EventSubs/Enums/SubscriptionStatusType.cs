using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchEventSubNew.Enums;

public enum SubscriptionStatusType
{
	Enabled = 1,
	Disbabled,
	Unknown,
}

// == ⚫ == //

public class SubscriptionStatusTypeEnumConverter : JsonConverter<SubscriptionStatusType>
{
	public override SubscriptionStatusType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		if (string.IsNullOrEmpty(value))
		{
			return SubscriptionStatusType.Unknown;
		}
		return value switch
		{
			"enabled" => SubscriptionStatusType.Enabled,
			"disabled" => SubscriptionStatusType.Disbabled,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, SubscriptionStatusType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			SubscriptionStatusType.Enabled => "enabled",
			SubscriptionStatusType.Disbabled => "disabled",
			SubscriptionStatusType.Unknown => "unknown",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}