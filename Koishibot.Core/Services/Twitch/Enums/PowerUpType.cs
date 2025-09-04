using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(PowerUpTypeEnumConverter))]
public enum PowerUpType
{
	MessageEffect,
	Celebration,
	GigantifyAnEmote
}

// == ⚫ == //

public class PowerUpTypeEnumConverter : JsonConverter<PowerUpType>
{
	public override PowerUpType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"message_effect" => PowerUpType.MessageEffect,
			"celebration" => PowerUpType.Celebration,
			"gigantify_an_emote" => PowerUpType.GigantifyAnEmote,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, PowerUpType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			PowerUpType.MessageEffect => "message_effect",
			PowerUpType.Celebration => "celebration",
			PowerUpType.GigantifyAnEmote => "gigantify_an_emote",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}