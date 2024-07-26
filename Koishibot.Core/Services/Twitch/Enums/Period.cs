using System.Text.Json;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.Enums;

//day — A day spans from 00:00:00 on the day specified in started_at and runs through 00:00:00 of the next day.
//week — A week spans from 00:00:00 on the Monday of the week specified in started_at and runs through 00:00:00 of the next Monday.
//month — A month spans from 00:00:00 on the first day of the month specified in started_at and runs through 00:00:00 of the first day of the next month.
//year — A year spans from 00:00:00 on the first day of the year specified in started_at and runs through 00:00:00 of the first day of the next year.
//all — Default.The lifetime of the broadcaster's channel.

public enum Period
{
	Day = 1,
	Week,
	Month,
	Year,
	All
}


// == ⚫ == //

public class PeriodEnumConverter : JsonConverter<Period>
{
	public override Period Read
									(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value switch
		{
			"day" => Period.Day,
			"week" => Period.Week,
			"month" => Period.Month,
			"year" => Period.Year,
			"all" => Period.All,
		};
	}

	public override void Write
									(Utf8JsonWriter writer, Period value, JsonSerializerOptions options)
	{
		var mappedValue = value switch
		{
			Period.Day => "day",
			Period.Week => "week",
			Period.Month => "month",
			Period.Year => "year",
			Period.All => "all",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
		};
		if (writer.CurrentDepth.Equals(1))
		{
			writer.WriteStringValue(mappedValue);
		}
	}
}

