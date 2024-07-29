using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(TermListTypeEnumConverter))]
public enum TermListType
{
	Blocked = 1,
	Permitted
}

// == ⚫ == //

public class TermListTypeEnumConverter : JsonConverter<TermListType>
{
	public override TermListType Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"blocked" => TermListType.Blocked,
			"permitted" => TermListType.Permitted,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, TermListType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			TermListType.Blocked => "blocked",
			TermListType.Permitted => "permitted",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}