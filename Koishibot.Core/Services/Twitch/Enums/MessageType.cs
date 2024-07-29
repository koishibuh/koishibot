using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(MessageTypeEnumConverter))]
public enum MessageType
{
	Text = 1,
	ChannelPointsHighlighted,
	ChannelPointsSubOnly,
	UserIntro,
	PowerUpsMessageEffect,
	PowerUpsGigantifiedEmote,
}

// == ⚫ == //

public class MessageTypeEnumConverter : JsonConverter<MessageType>
{
	public override MessageType Read
		(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"text" => MessageType.Text,
			"channel_points_highlighted" => MessageType.ChannelPointsHighlighted,
			"channel_points_sub_only" => MessageType.ChannelPointsSubOnly,
			"user_intro" => MessageType.UserIntro,
			"power_ups_message_effect" => MessageType.PowerUpsMessageEffect,
			"power_ups_gigantified_emote" => MessageType.PowerUpsGigantifiedEmote,
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, MessageType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			MessageType.Text => "text",
			MessageType.ChannelPointsHighlighted => "channel_points_highlighted",
			MessageType.ChannelPointsSubOnly => "channel_points_sub_only",
			MessageType.UserIntro => "user_intro",
			MessageType.PowerUpsMessageEffect => "power_ups_message_effect",
			MessageType.PowerUpsGigantifiedEmote => "power_ups_gigantified_emote",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}