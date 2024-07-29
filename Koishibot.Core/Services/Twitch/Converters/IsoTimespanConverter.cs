using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Converters;


public class IsoTimespanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string durationString = reader.GetString();

        // Parse the ISO 8601 duration string
        var duration = System.Xml.XmlConvert.ToTimeSpan(durationString);

        return duration;
    }

    public override void Write
        (Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        string durationString = System.Xml.XmlConvert.ToString(value);
        writer.WriteStringValue(durationString);
    }
}