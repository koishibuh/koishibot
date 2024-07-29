using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(GuestStateEnumConverter))]
public enum GuestState
{
	Invited = 1,
	Accepted,
	Ready,
	Backstage,
	Live,
	Removed
}

//invited — The guest has transitioned to the invite queue.This can take place when the guest was previously assigned a slot, but have been removed from the call and are sent back to the invite queue.
//accepted — The guest has accepted the invite and is currently in the process of setting up to join the session.
//ready — The guest has signaled they are ready and can be assigned a slot.
//backstage — The guest has been assigned a slot in the session, but is not currently seen live in the broadcasting software.
//live — The guest is now live in the host's broadcasting software.

//removed — The guest was removed from the call or queue.
//accepted — The guest has accepted the invite to the call.

// == ⚫ == //

public class GuestStateEnumConverter : JsonConverter<GuestState>
{
	public override GuestState Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"invited" => GuestState.Invited,
			"accepted" => GuestState.Accepted,
			"ready" => GuestState.Ready,
			"backstage" => GuestState.Backstage,
			"live" => GuestState.Live,
			"removed" => GuestState.Removed,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, GuestState value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			GuestState.Invited => "invited",
			GuestState.Accepted => "accepted",
			GuestState.Ready => "ready",
			GuestState.Backstage => "backstage",
			GuestState.Live => "live",
			GuestState.Removed => "removed",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}