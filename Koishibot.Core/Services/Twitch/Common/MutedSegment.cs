using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;

public class MutedSegment
{
	///<summary>
	///The duration of the muted segment, in seconds.
	///</summary>
	[JsonPropertyName("duration")]
	public int DurationInSeconds { get; set; }

	///<summary>
	///The offset, in seconds, from the beginning of the video to where the muted segment begins.
	///</summary>
	[JsonPropertyName("offset")]
	public int OffsetInSeconds { get; set; }
}