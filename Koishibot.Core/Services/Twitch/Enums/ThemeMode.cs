using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(ThemeModeEnumConverter))]
public enum ThemeMode
{
	Dark = 1,
	Light
}

// == ⚫ == //

public class ThemeModeEnumConverter : JsonConverter<ThemeMode>
{
	public override ThemeMode Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"dark" => ThemeMode.Dark,
			"light" => ThemeMode.Light,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, ThemeMode value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			ThemeMode.Dark => "dark",
			ThemeMode.Light => "light",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}