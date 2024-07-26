using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;

public enum AnnouncementColor
{
	Blue = 1,
	Green,
	Orange,
	Purple,
	Primary //Default
}

// == ⚫ == //

public class AnnouncementColorEnumConverter : JsonConverter<AnnouncementColor>
{
	public override AnnouncementColor Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"blue" => AnnouncementColor.Blue,
			"green" => AnnouncementColor.Green,
			"orange" => AnnouncementColor.Orange,
			"purple" => AnnouncementColor.Purple,
			"primary" => AnnouncementColor.Primary,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, AnnouncementColor value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			AnnouncementColor.Blue => "blue",
			AnnouncementColor.Green => "green",
			AnnouncementColor.Orange => "orange",
			AnnouncementColor.Purple => "purple",
			AnnouncementColor.Primary => "primary",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}
