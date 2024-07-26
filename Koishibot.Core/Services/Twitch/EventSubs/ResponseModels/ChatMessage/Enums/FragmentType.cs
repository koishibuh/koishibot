using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage.Enums;

public enum FragmentType
{
    Text = 1,
    Cheermote,
    Emote,
    Mention
}

// == ⚫ == //

public class FragmentTypeEnumConverter : JsonConverter<FragmentType>
{
    public override FragmentType Read
            (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "text" => FragmentType.Text,
            "cheermote" => FragmentType.Cheermote,
            "emote" => FragmentType.Emote,
            "mention" => FragmentType.Mention,
            _ => throw new JsonException()
        };
    }

    public override void Write
            (Utf8JsonWriter writer, FragmentType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}