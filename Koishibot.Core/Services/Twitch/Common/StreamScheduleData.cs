using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;
public class StreamScheduleData
{
	///<summary>
	///The ID of the broadcaster that owns the broadcast schedule.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The list of broadcasts in the broadcaster’s streaming schedule.
	///</summary>
	[JsonPropertyName("segments")]
	public List<StreamSegmentsData> StreamSegements { get; set; }

	///<summary>
	///The dates when the broadcaster is on vacation and not streaming.<br/>
	///Is set to null if vacation mode is not enabled.
	///</summary>
	[JsonPropertyName("vacation")]
	public VacationData Vacation { get; set; }

}

public class StreamSegmentsData
{
	///<summary>
	///An ID that identifies this broadcast segment.
	///</summary>
	[JsonPropertyName("id")]
	public string StreamSegmentId { get; set; }

	///<summary>
	///The UTC date and time (in RFC3339 format) of when the broadcast starts.
	///</summary>
	[JsonPropertyName("start_time")]
	public DateTimeOffset StartTime
	{ get; set; }

	///<summary>
	///The UTC date and time (in RFC3339 format) of when the broadcast ends.
	///</summary>
	[JsonPropertyName("end_time")]
	public DateTimeOffset EndTime { get; set; }

	///<summary>
	///The broadcast segment’s title.
	///</summary>
	[JsonPropertyName("title")]
	public string StreamSegmentTitle { get; set; }

	///<summary>
	///Indicates whether the broadcaster canceled this segment of a recurring broadcast.<br/>
	///If the broadcaster canceled this segment, this field is set to the same value that’s in the end_time field;<br/>
	///otherwise, it’s set to null.
	///</summary>
	[JsonPropertyName("canceled_until")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? CanceledUntil { get; set; }

	///<summary>
	///The type of content that the broadcaster plans to stream or null if not specified.
	///</summary>
	[JsonPropertyName("category")]
	public string Category { get; set; }

	///<summary>
	///An ID that identifies the category that best represents the content that the broadcaster plans to stream.<br/>
	///For example, the game’s ID if the broadcaster will play a game or the Just Chatting ID if the broadcaster will host a talk show.
	///</summary>
	[JsonPropertyName("id")]
	public string CategoryId { get; set; }

	///<summary>
	///The name of the category.<br/>
	///For example, the game’s title if the broadcaster will playing a game or Just Chatting if the broadcaster will host a talk show.
	///</summary>
	[JsonPropertyName("name")]
	public string CategoryName { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcast is part of a recurring series that streams at the same time each week or is a one-time broadcast.<br/>
	///Is true if the broadcast is part of a recurring series.
	///</summary>
	[JsonPropertyName("is_recurring")]
	public bool IsRecurring { get; set; }
}

public class VacationData
{
	///<summary>
	///The UTC date and time (in RFC3339 format) of when the broadcaster’s vacation starts.
	///</summary>
	[JsonPropertyName("start_time")]
	public string StartTime { get; set; }

	///<summary>
	///The UTC date and time (in RFC3339 format) of when the broadcaster’s vacation ends.
	///</summary>
	[JsonPropertyName("end_time")]
	public string EndTime { get; set; }
}