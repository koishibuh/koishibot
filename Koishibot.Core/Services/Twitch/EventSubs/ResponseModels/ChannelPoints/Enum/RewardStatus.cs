using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelPoints.Enum;

public enum RewardStatus
{
    Unknown = 1,
    Unfulfilled,
    Fulfilled,
    Canceled
}

// == ⚫ == //

public class RewardStatusEnumConverter : JsonConverter<RewardStatus>
{
    public override RewardStatus Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "unknown" => RewardStatus.Unknown,
            "unfulfilled" => RewardStatus.Unfulfilled,
            "fulfilled" => RewardStatus.Fulfilled,
            "canceled" => RewardStatus.Canceled,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, RewardStatus value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}