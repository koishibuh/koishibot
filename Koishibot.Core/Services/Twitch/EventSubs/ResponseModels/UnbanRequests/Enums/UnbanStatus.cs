using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.UnbanRequests.Enums;

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
        throw new NotImplementedException();
    }
}