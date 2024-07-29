using System.Text.Json;

namespace Koishibot.Core.Services.Twitch.Enums;

[JsonConverter(typeof(AnalyticReportTypeEnumConverter))]
public enum AnalyticsReportType
{
	OverviewV2 = 1
}

// == ⚫ == //

public class AnalyticReportTypeEnumConverter : JsonConverter<AnalyticsReportType>
{
	public override AnalyticsReportType Read
					(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"overview_v2" => AnalyticsReportType.OverviewV2,
			_ => throw new JsonException()
		};
	}

	public override void Write
					(Utf8JsonWriter writer, AnalyticsReportType value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			AnalyticsReportType.OverviewV2 => "overview_v2",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}