using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;

//ACTIVE — The poll is running.
//COMPLETED — The poll ended on schedule(see the duration field).
//TERMINATED — The poll was terminated before its scheduled end.
//ARCHIVED — The poll has been archived and is no longer visible on the channel.
//MODERATED — The poll was deleted.
//INVALID — Something went wrong while determining the state
public enum PollApiStatus
{
	Active,
	Completed,
	Terminated,
	Archived,
	Moderated,
	Invalid
}

// == ⚫ == //

public class PollApiStatusEnumConverter : JsonConverter<PollApiStatus>
{
	public override PollApiStatus Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"ACTIVE" => PollApiStatus.Active,
			"COMPLETED" => PollApiStatus.Completed,
			"TERMINATED" => PollApiStatus.Terminated,
			"ARCHIVED" => PollApiStatus.Archived,
			"MODERATED" => PollApiStatus.Moderated,
			"INVALID" => PollApiStatus.Invalid,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, PollApiStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			PollApiStatus.Active => "ACTIVE",
			PollApiStatus.Completed => "COMPLETED",
			PollApiStatus.Terminated => "TERMINATED",
			PollApiStatus.Archived => "ARCHIVED",
			PollApiStatus.Moderated => "MODERATED",
			PollApiStatus.Invalid => "INVALID",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}
