using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod.Enums;
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
        throw new NotImplementedException();
    }
}