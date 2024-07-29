using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(AutomodActionEnumConverter))]
public enum AutomodMessageStatus
{
	Approved = 1,
	Denied,
	Expired
}

// == ⚫ == //

public class AutomodMessageStatusConverter : JsonConverter<AutomodMessageStatus>
{
	public override AutomodMessageStatus Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"approved" => AutomodMessageStatus.Approved,
			"denied" => AutomodMessageStatus.Denied,
			"expired" => AutomodMessageStatus.Expired,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, AutomodMessageStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			AutomodMessageStatus.Approved => "approved",
			AutomodMessageStatus.Denied => "denied",
			AutomodMessageStatus.Expired => "expired",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}