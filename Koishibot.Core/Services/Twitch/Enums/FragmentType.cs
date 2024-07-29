using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(FragmentTypeEnumConverter))]
public enum FragmentType
{
	Text = 1,
	Cheermote,
	Emote,
	Mention
}

// == ⚫ == //

public class FragmentTypeEnumConverter : JsonConverter<FragmentType>
{
	public override FragmentType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"text" => FragmentType.Text,
			"cheermote" => FragmentType.Cheermote,
			"emote" => FragmentType.Emote,
			"mention" => FragmentType.Mention,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, FragmentType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			FragmentType.Text => "text",
			FragmentType.Cheermote => "cheermote",
			FragmentType.Emote => "emote",
			FragmentType.Mention => "mention",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}