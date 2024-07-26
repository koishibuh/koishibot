using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.SuspiciousUser.Enums;

public enum SuspiciousType
{
    Manual = 1,
    BanEvaderDetector,
    SharedChannelBan
}


// == ⚫ == //

public class SuspiciousTypeEnumConverter : JsonConverter<SuspiciousType>
{
    public override SuspiciousType Read
            (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "manual" => SuspiciousType.Manual,
            "ban_evader_detector" => SuspiciousType.BanEvaderDetector,
            "shared_channel_ban" => SuspiciousType.SharedChannelBan,
            _ => throw new JsonException()
        };
    }

    public override void Write
            (Utf8JsonWriter writer, SuspiciousType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}