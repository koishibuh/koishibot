using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod.Enums;

public enum AutomodAction
{
    AddPermitted = 1,
    RemovePermitted,
    AddBlocked,
    RemoveBlocked
}

// == ⚫ == //

public class AutomodActionEnumConverter : JsonConverter<AutomodAction>
{
    public override AutomodAction Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "add_permitted" => AutomodAction.AddPermitted,
            "remove_permitted" => AutomodAction.RemovePermitted,
            "add_blocked" => AutomodAction.AddBlocked,
            "remove_blocked" => AutomodAction.RemoveBlocked,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, AutomodAction value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}