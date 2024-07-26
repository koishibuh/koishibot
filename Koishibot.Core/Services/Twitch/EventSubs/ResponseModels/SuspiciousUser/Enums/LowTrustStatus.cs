using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser.Enums;

public enum LowTrustStatus
{
    None = 1,
    ActiveMonitoring,
    Restricted
}

// == ⚫ == //

public class LowTrustStatusEnumConverter : JsonConverter<LowTrustStatus>
{
    public override LowTrustStatus Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "none" => LowTrustStatus.None,
            "active_monitoring" => LowTrustStatus.ActiveMonitoring,
            "restricted" => LowTrustStatus.Restricted,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, LowTrustStatus value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}