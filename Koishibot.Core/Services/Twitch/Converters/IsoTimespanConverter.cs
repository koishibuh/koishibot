using System.Text.Json;
using System.Text.RegularExpressions;

namespace Koishibot.Core.Services.Twitch.Converters;

public class IsoTimespanConverter : JsonConverter<TimeSpan>
{
	public override TimeSpan Read
	(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		//43h54m58s
		//1m30s
		var durationString = reader.GetString();

		var match = Regex.Match(durationString, @"(?:(\d+)h)?(?:(\d+)m)?(?:(\d+)s)?");
		var hours = match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0;
		var minutes = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;
		var seconds = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;

		return new TimeSpan(hours, minutes, seconds);

		// // Parse the ISO 8601 duration string
		// var duration = System.Xml.XmlConvert.ToTimeSpan(durationString);
	}

	public override void Write
	(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
	{
		string durationString = System.Xml.XmlConvert.ToString(value);
		writer.WriteStringValue(durationString);
	}
}