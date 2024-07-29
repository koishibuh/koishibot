using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(AutoModTermActionEnumConverter))]
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
		var mappedValue = value switch
		{
			AutoModTermAction.Add => "add",
			AutoModTermAction.Remove => "remove",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}