using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser.Enums;
public enum MessageFragmentType
{
    Text = 1,
    Cheermote,
    Emote
}


// == ⚫ == //

public class MessageFragmentTypeEnumConverter : JsonConverter<MessageFragmentType>
{
    public override MessageFragmentType Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "text" => MessageFragmentType.Text,
            "cheermote" => MessageFragmentType.Cheermote,
            "emote" => MessageFragmentType.Emote,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, MessageFragmentType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}