using System.Text.Json;
namespace Koishibot.Core.Services.Twitch.Enums;

//day — A day spans from 00:00:00 on the day specified in started_at and runs through 00:00:00 of the next day.
//week — A week spans from 00:00:00 on the Monday of the week specified in started_at and runs through 00:00:00 of the next Monday.
//month — A month spans from 00:00:00 on the first day of the month specified in started_at and runs through 00:00:00 of the first day of the next month.
//year — A year spans from 00:00:00 on the first day of the year specified in started_at and runs through 00:00:00 of the first day of the next year.
//all — Default.The lifetime of the broadcaster's channel.

public class Period
{
	public const string Day = "day";
	public const string Week = "week";
	public const string Month = "month";
	public const string Year = "year";
	public const string All = "all";
}

public class PeriodEnumConverter : JsonConverter<string>
{
	public override string Read
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
			_ => throw new JsonException()
		};
	}

	public override void Write
		(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
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