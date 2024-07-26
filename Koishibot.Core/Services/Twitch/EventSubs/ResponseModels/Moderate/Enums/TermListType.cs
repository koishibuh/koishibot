using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate.Enums;

public enum TermListType
{
    Blocked = 1,
    Permitted
}

// == ⚫ == //

public class TermListTypeEnumConverter : JsonConverter<TermListType>
{
    public override TermListType Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "blocked" => TermListType.Blocked,
            "permitted" => TermListType.Permitted,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, TermListType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}