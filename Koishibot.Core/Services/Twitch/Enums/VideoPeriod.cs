using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;


[JsonConverter(typeof(VideoPeriodEnumConverter))]
public enum VideoPeriod
{
	All = 1,
	Day,
	Week,
	Month
}

// == ⚫ == //

public class VideoPeriodEnumConverter : JsonConverter<VideoPeriod>
{
	public override VideoPeriod Read
			(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"all" => VideoPeriod.All,
			"day" => VideoPeriod.Day,
			"week" => VideoPeriod.Week,
			"month" => VideoPeriod.Month,
			_ => throw new JsonException()
		};
	}

	public override void Write
			(Utf8JsonWriter writer, VideoPeriod value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			VideoPeriod.All => "all",
			VideoPeriod.Day => "day",
			VideoPeriod.Week => "week",
			VideoPeriod.Month => "month",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}