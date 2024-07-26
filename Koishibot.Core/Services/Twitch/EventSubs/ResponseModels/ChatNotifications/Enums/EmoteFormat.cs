using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications.Enums;
public enum EmoteFormat
{
    Animated = 1,
    Static
}

// == ⚫ == //

public class EmoteFormatEnumConverter : JsonConverter<EmoteFormat>
{
    public override EmoteFormat Read
            (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "animated" => EmoteFormat.Animated,
            "static" => EmoteFormat.Static,
            _ => throw new JsonException()
        };
    }

    public override void Write
            (Utf8JsonWriter writer, EmoteFormat value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}