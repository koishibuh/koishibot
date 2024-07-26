using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate.Enums;

public enum ContentClassificationLabelId
{
    DrugsIntoxication = 1,
    Gambling,
    MatureGame,
    ProfanityVulgarity,
    SexualThemes,
    ViolentGraphic
}

// == ⚫ == //

public class ContentClassificationLabelIdEnumConverter : JsonConverter<ContentClassificationLabelId>
{
    public override ContentClassificationLabelId Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "DrugsIntoxication" => ContentClassificationLabelId.DrugsIntoxication,
            "Gambling" => ContentClassificationLabelId.Gambling,
            "MatureGame" => ContentClassificationLabelId.MatureGame,
            "ProfanityVulgarity" => ContentClassificationLabelId.ProfanityVulgarity,
            "SexualThemes" => ContentClassificationLabelId.SexualThemes,
            "ViolentGraphic" => ContentClassificationLabelId.ViolentGraphic,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, ContentClassificationLabelId value, JsonSerializerOptions options)
    {
        var mappedValue = value switch
        {
            ContentClassificationLabelId.DrugsIntoxication => "DrugsIntoxication",
            ContentClassificationLabelId.Gambling => "Gambling",
            ContentClassificationLabelId.MatureGame => "MatureGame",
            ContentClassificationLabelId.ProfanityVulgarity => "ProfanityVulgarity",
            ContentClassificationLabelId.SexualThemes => "SexualThemes",
            ContentClassificationLabelId.ViolentGraphic => "ViolentGraphic",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
        if (writer.CurrentDepth.Equals(1))
        {
            writer.WriteStringValue(mappedValue);
        }
    }
}