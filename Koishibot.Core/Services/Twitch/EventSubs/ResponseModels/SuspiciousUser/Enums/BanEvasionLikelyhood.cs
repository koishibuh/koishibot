using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser.Enums;

public enum BanEvasionLikelyhood
{
    Unknown,
    Possible,
    Likely
}

// == ⚫ == //

public class BanEvasionLikelyhoodEnumConverter : JsonConverter<BanEvasionLikelyhood>
{
    public override BanEvasionLikelyhood Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "unknown" => BanEvasionLikelyhood.Unknown,
            "possible" => BanEvasionLikelyhood.Possible,
            "likely" => BanEvasionLikelyhood.Likely,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, BanEvasionLikelyhood value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}