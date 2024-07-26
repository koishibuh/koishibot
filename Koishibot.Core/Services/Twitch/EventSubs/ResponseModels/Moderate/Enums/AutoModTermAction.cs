using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate.Enums;

public enum AutoModTermAction
{
    Add = 1,
    Remove
}

// == ⚫ == //

public class AutoModTermActionEnumConverter : JsonConverter<AutoModTermAction>
{
    public override AutoModTermAction Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "add" => AutoModTermAction.Add,
            "remove" => AutoModTermAction.Remove,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, AutoModTermAction value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}