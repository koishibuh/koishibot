using Koishibot.Core.Services.Twitch.EventSubs.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;

public class DateRange
{
	///<summary>
	///The reporting window’s start date.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("started_at")]
	[JsonConverter(typeof(DateTimeOffsetConverter))]
	public DateTimeOffset StartedAt { get; set; }

	///<summary>
	///The reporting window’s end date.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("ended_at")]
	[JsonConverter(typeof(DateTimeOffsetConverter))]
	public DateTimeOffset EndedAt { get; set; }
}
