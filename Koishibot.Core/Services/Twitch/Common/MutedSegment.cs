using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.Twitch.Common;

public class MutedSegment
{
	///<summary>
	///The duration of the muted segment, in seconds.
	///</summary>
	[JsonPropertyName("duration")]
	[JsonConverter(typeof(TimeSpanSecondsConverter))]
	public TimeSpan DurationInSeconds { get; set; }

	///<summary>
	///The offset, in seconds, from the beginning of the video to where the muted segment begins.
	///</summary>
	[JsonPropertyName("offset")]
	[JsonConverter(typeof(TimeSpanSecondsConverter))]
	public TimeSpan OffsetInSeconds { get; set; }
}