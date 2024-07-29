using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(UnbanStatusEnumConverter))]
public enum UnbanStatus
{
	Approved = 1,
	Canceled,
	Denied,
}

// == ⚫ == //

public class UnbanStatusEnumConverter : JsonConverter<UnbanStatus>
{
	public override UnbanStatus Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"approved" => UnbanStatus.Approved,
			"canceled" => UnbanStatus.Canceled,
			"denied" => UnbanStatus.Denied,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, UnbanStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			UnbanStatus.Approved => "approved",
			UnbanStatus.Canceled => "canceled",
			UnbanStatus.Denied => "denied",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}