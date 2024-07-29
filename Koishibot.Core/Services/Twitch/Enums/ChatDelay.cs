using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(ChatDelayEnumConverter))]
public enum ChatDelay
{
	TwoSecondDelay = 1,
	FourSecondDelay,
	SixSecondDelay
}

// == ⚫ == //

public class ChatDelayEnumConverter : JsonConverter<ChatDelay>
{
	public override ChatDelay Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetInt64();
		return value switch
		{
			2 => ChatDelay.TwoSecondDelay,
			4 => ChatDelay.FourSecondDelay,
			6 => ChatDelay.SixSecondDelay,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, ChatDelay value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			ChatDelay.TwoSecondDelay => 2,
			ChatDelay.FourSecondDelay => 4,
			ChatDelay.SixSecondDelay => 6,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteNumberValue(mappedValue);
		}
	}
}