using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.GuestStar.Enums;
public enum GroupLayout
{
    Tiled = 1,
    Screenshare,
    HorizontalTop,
    HorizontalBottom,
    VerticalLeft,
    VerticalRight
}

//tiled — All live guests are tiled within the browser source with the same size.
//screenshare — All live guests are tiled within the browser source with the same size.If there is an active screen share, it is sized larger than the other guests.
//horizontal_top — Indicates the group layout will contain all participants in a top-aligned horizontal stack.
//horizontal_bottom — Indicates the group layout will contain all participants in a bottom-aligned horizontal stack.
//vertical_left — Indicates the group layout will contain all participants in a left-aligned vertical stack.
//vertical_right — Indicates the group layout will contain all participants in a right-aligned vertical stack.

// == ⚫ == //

public class GroupLayoutEnumConverter : JsonConverter<GroupLayout>
{
    public override GroupLayout Read
            (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {

            "tiled" => GroupLayout.Tiled,
            "screenshare" => GroupLayout.Screenshare,
            "horizontal_top" => GroupLayout.HorizontalTop,
            "horizontal_bottom" => GroupLayout.HorizontalBottom,
            "vertical_left" => GroupLayout.VerticalLeft,
            "vertical_right" => GroupLayout.VerticalRight,
            _ => throw new JsonException()
        };
    }

    public override void Write
            (Utf8JsonWriter writer, GroupLayout value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}