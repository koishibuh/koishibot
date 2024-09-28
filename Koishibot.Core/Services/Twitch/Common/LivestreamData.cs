using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.Common;

public class LivestreamData
{
	///<summary>
	///An Id that identifies the stream<br/>
	///This id matches the StreamId field on GetVideos, but you can't query for the StreamId.<br/>
	///Need to fetch list of videos and see if the StreamId matches - this only shows public videos, not unpublished.
	///</summary>
	[JsonPropertyName("id")]
	public string? StreamId { get; set; }

	///<summary>
	///The ID of the user that’s broadcasting the stream.
	///</summary>
	[JsonPropertyName("user_id")]
	public string? BroadcasterId { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string? BroadcasterLogin { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string? BroadcasterName { get; set; }

	///<summary>
	///The ID of the category or game being played.
	///</summary>
	[JsonPropertyName("game_id")]
	public string? CategoryId { get; set; }

	///<summary>
	///The ID of the category or game being played.
	///</summary>
	[JsonPropertyName("game_name")]
	public string? CategoryName { get; set; }

	///<summary>
	///The type of stream. Possible values are:live<br/>
	///If an error occurs, this field is set to an empty string.
	///</summary>
	[JsonPropertyName("type")]
	public StreamType Type { get; set; }

	///<summary>
	///The stream’s title. Is an empty string if not set.
	///</summary>
	[JsonPropertyName("title")]
	public string StreamTitle { get; set; } = string.Empty;

	///<summary>
	///The number of users watching the stream.
	///</summary>
	[JsonPropertyName("viewer_count")]
	public int ViewerCount { get; set; }

	///<summary>
	///The timestamp of when the broadcast began.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset StartedAt { get; set; }

	///<summary>
	///The language that the stream uses.<br/>
	///This is an ISO 639-1 two-letter language code or other if the stream uses a language not in the list of supported stream languages.
	///</summary>
	[JsonPropertyName("language")]
	public string? Language { get; set; }

	///<summary>
	///A URL to an image of a frame from the last 5 minutes of the stream.<br/>
	///Replace the width and height placeholders in the URL ({width}x{height}) with the size of the image you want, in pixels.
	///</summary>
	[JsonPropertyName("thumbnail_url")]
	public string? ThumbnailUrl { get; set; }

	///<summary>
	///The tags applied to the stream.
	///</summary>
	[JsonPropertyName("tags")]
	public List<string>? Tags { get; set; }

	///<summary>
	///A Boolean value that indicates whether the stream is meant for mature audiences.
	///</summary>
	[JsonPropertyName("is_mature")]
	public bool IsMature { get; set; }

/*═════════◣ DTO ◢═════════*/
	public LiveStreamInfo ConvertToDto()
	{
		return new LiveStreamInfo(
			BroadcasterId ?? string.Empty,
			StreamId ?? string.Empty,
			CategoryId ?? string.Empty,
			CategoryName ?? string.Empty,
			StreamTitle,
			ViewerCount,
			StartedAt,
			ThumbnailUrl ?? string.Empty);
	}
}