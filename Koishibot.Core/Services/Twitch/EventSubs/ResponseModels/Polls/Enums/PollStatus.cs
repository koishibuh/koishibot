using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls.Enums;

public enum PollStatus
{
    Completed = 1,
    Archived,
    Terminated
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
            "completed" => PollStatus.Completed,
            "archived" => PollStatus.Archived,
            "terminated" => PollStatus.Terminated,
            _ => throw new JsonException()
        };
    }

    public override void Write
            (Utf8JsonWriter writer, PollStatus value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}