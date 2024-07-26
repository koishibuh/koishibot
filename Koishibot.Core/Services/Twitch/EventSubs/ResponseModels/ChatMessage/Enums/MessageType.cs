using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage.Enums;
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
        throw new NotImplementedException();
    }
}