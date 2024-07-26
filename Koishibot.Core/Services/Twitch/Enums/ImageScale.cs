using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Enums;

public enum ImageScale
{
	Small_28px,
	Medium_56px,
	Large_112px
}

// == ⚫ == //

public class ImageScaleEnumConverter : JsonConverter<ImageScale>
{
	public override ImageScale Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"1.0" => ImageScale.Small_28px,
			"2.0" => ImageScale.Medium_56px,
			"3.0" => ImageScale.Large_112px,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, ImageScale value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			ImageScale.Small_28px => "1.0",
			ImageScale.Medium_56px => "2.0",
			ImageScale.Large_112px => "3.0",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}