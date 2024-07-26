using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;

//TERMINATED — The poll was terminated before its scheduled end.
//ARCHIVED — The poll has been archived and is no longer visible on the channel.

public enum SetPollStatus
{
	Terminate,
	Archive
}

// == ⚫ == //

public class SetPollStatusEnumConverter : JsonConverter<SetPollStatus>
{
	public override SetPollStatus Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"TERMINATED" => SetPollStatus.Terminate,
			"ARCHIVED" => SetPollStatus.Archive,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, SetPollStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			SetPollStatus.Terminate => "TERMINATED",
			SetPollStatus.Archive => "ARCHIVED",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}
