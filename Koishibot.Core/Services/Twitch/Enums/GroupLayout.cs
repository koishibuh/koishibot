using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(GroupLayoutEnumConverter))]
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
		var mappedValue = value switch
		{
			GroupLayout.Tiled => "tiled",
			GroupLayout.Screenshare => "screenshare",
			GroupLayout.HorizontalTop => "horizontal_top",
			GroupLayout.HorizontalBottom => "horizontal_bottom",
			GroupLayout.VerticalLeft => "vertical_left",
			GroupLayout.VerticalRight => "vertical_right",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}