using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

//ACTIVE — The poll is running.
//COMPLETED — The poll ended on schedule(see the duration field).
//TERMINATED — The poll was terminated before its scheduled end.
//ARCHIVED — The poll has been archived and is no longer visible on the channel.
//MODERATED — The poll was deleted.
//INVALID — Something went wrong while determining the state
[JsonConverter(typeof(PollStatusEnumConverter))]
public enum PollStatus
{
	Active = 1,
	Completed,
	Terminated,
	Archived,
	Moderated,
	Invalid
}

// == ⚫ == //

public class PollStatusEnumConverter : JsonConverter<PollStatus>
{
	public override PollStatus Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"ACTIVE" => PollStatus.Active,
			"COMPLETED" => PollStatus.Completed,
			"completed" => PollStatus.Completed, // EventSub
			"TERMINATED" => PollStatus.Terminated,
			"terminated" => PollStatus.Terminated, // EventSub
			"ARCHIVED" => PollStatus.Archived,
			"archived" => PollStatus.Archived, // EventSub
			"MODERATED" => PollStatus.Moderated,
			"INVALID" => PollStatus.Invalid,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, PollStatus value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			PollStatus.Active => "ACTIVE",
			PollStatus.Completed => "COMPLETED",
			PollStatus.Terminated => "TERMINATED",
			PollStatus.Archived => "ARCHIVED",
			PollStatus.Moderated => "MODERATED",
			PollStatus.Invalid => "INVALID",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}



