using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(AutomodActionEnumConverter))]
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
		var mappedValue = value switch
		{
			AutomodAction.AddPermitted => "add_permitted",
			AutomodAction.RemovePermitted => "remove_permitted",
			AutomodAction.AddBlocked => "add_blocked",
			AutomodAction.RemoveBlocked => "remove_blocked",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}