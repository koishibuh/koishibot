using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ConduitShard.Enums;

public enum Method
{
    Websocket = 1,
    Webhook
}

// == ⚫ == //

public class MethodEnumConverter : JsonConverter<Method>
{
    public override Method Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "websocket" => Method.Websocket,
            "webhook" => Method.Webhook,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, Method value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}