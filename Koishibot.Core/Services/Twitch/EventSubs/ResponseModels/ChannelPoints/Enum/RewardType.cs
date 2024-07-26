using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints.Enum;

public enum RewardType
{
    SingleMessageBypassSubMode = 1,
    SendHighlightedMessage,
    RandomSubEmoteUnlock,
    ChosenSubEmoteUnlock,
    ChosenModifiedSubEmoteUnlock,
    MessageEffect,
    GigantifyAnEmote,
    Celebration
}

// == ⚫ == //

public class RewardTypeEnumConverter : JsonConverter<RewardType>
{
    public override RewardType Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "single_message_bypass_sub_mode" => RewardType.SingleMessageBypassSubMode,
            "send_highlighted_message" => RewardType.SendHighlightedMessage,
            "random_sub_emote_unlock" => RewardType.RandomSubEmoteUnlock,
            "chosen_sub_emote_unlock" => RewardType.ChosenModifiedSubEmoteUnlock,
            "chosen_modified_sub_emote_unlock" => RewardType.ChosenModifiedSubEmoteUnlock,
            "message_effect" => RewardType.MessageEffect,
            "gigantify_an_emote" => RewardType.GigantifyAnEmote,
            "celebration" => RewardType.Celebration,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, RewardType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}