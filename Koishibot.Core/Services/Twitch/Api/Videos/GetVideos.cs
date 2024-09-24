using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;
using Newtonsoft.Json.Converters;
using System.Text.Json;

namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-videos">Twitch Documentation</see><br/>
	/// Gets information about one or more published videos - this does not list unpublished videos. You may get videos by ID, by user, or by game/category.<br/>
	/// Required Scopes: User Access Token
	/// </summary>

	public async Task<GetVideosResponse> GetVideos(GetVideosRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		const string url = "videos";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<GetVideosResponse>(response)
			?? throw new Exception("Failed to deserialize response");

		return result;
	}
}

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
public class GetVideosRequestParameters
{
		///<summary>
	///A list of IDs that identify the videos you want to get.<br/>
	///To get more than one video, include this parameter for each video you want to get. For example, id=1234 id=5678.<br/>
	///You may specify a maximum of 100 IDs. The endpoint ignores duplicate IDs and IDs that weren't found (if there's at least one valid ID).
	///The id, user_id, and game_id parameters are mutually exclusive.<br/>
	///</summary>
	[JsonPropertyName("id")]
	public List<string>? VideoIds { get; set; }

	///<summary>
	///The ID of the user whose list of videos you want to get.<br/>
	///The id, user_id, and game_id parameters are mutually exclusive.<br/>
	///</summary>
	[JsonPropertyName("user_id")]
	public string? BroadcasterId { get; set; } 

	///<summary>
	///A category or game ID.<br/>
	///The response contains a maximum of 500 videos that show this content.<br/>
	///To get category/game IDs, use the Search Categories endpoint.<br/>
	///The id, user_id, and game_id parameters are mutually exclusive.
	///</summary>
	[JsonPropertyName("game_id")]
	public string? CategoryId { get; set; } 

	///<summary>
	///A filter used to filter the list of videos by the language that the video owner broadcasts in.<br/>
	///For example, to get videos that were broadcast in German, set this parameter to the ISO 639-1 two-letter code for German (i.e., DE).<br/>
	///Specify this parameter only if you specify the game_id query parameter.
	///</summary>
	[JsonPropertyName("language")]
	public string? Language { get; set; }

	///<summary>
	///A filter used to filter the list of videos by when they were published.<br/>
	///For example, videos published in the last week.<br/>
	///The default is ""all,"" which returns videos published in all periods.<br/>
	///Specify this parameter only if you specify the game_id or user_id query parameter.
	///</summary>
	[JsonPropertyName("period")]
	[JsonConverter(typeof(VideoPeriodEnumConverter))]
	public VideoPeriod? TimeRange { get; set; }

	///<summary>
	///The order to sort the returned videos in.<br/>
	///The default is "time."<br/>
	///Specify this parameter only if you specify the game_id or user_id query parameter.
	///</summary>
	[JsonPropertyName("sort")]
	[JsonConverter(typeof(VideoSortEnumConverter))]
	public VideoSort? SortVideosBy { get; set; }

	///<summary>
	///A filter used to filter the list of videos by the video's type.<br/>
	///The default is "all" which returns all video types.<br/>
	///Specify this parameter only if you specify the game_id or user_id query parameter.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(VideoTypeEnumConverter))]
	public VideoType? VideoType { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100. The default is 20.<br/>
	///Specify this parameter only if you specify the game_id or user_id query parameter.
	///</summary>
	[JsonPropertyName("first")]
	public string? ItemsPerPage { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.<br/>
	///Specify this parameter only if you specify the user_id query parameter.
	///</summary>
	[JsonPropertyName("after")]
	public string? PaginationAfter { get; set; }

	///<summary>
	///The cursor used to get the previous page of results.<br/>
	///The Pagination object in the response contains the cursor’s value. <br/>
	///Specify this parameter only if you specify the user_id query parameter.
	///</summary>
	[JsonPropertyName("before")]
	public string? PaginationBefore { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetVideosResponse
{
	///<summary>
	///The list of published videos that match the filter criteria.
	///</summary>
	[JsonPropertyName("data")]
	public List<VideoData> Data { get; set; }

	///<summary>
	///Contains the information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}

public class VideoData
{
	///<summary>
	///An ID that identifies the video - matches GetLiveStream's Id
	///</summary>
	[JsonPropertyName("id")]
	public string VideoId { get; set; }

	///<summary>
	///The ID of the stream that the video originated from if the video's type is "archive;"<br/>
	///Otherwise, null.
	///</summary>
	[JsonPropertyName("stream_id")]
	public string? StreamId { get; set; }

	///<summary>
	///The ID of the broadcaster that owns the video.
	///</summary>
	[JsonPropertyName("user_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster's login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The broadcaster's display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The video's title.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }

	///<summary>
	///The video's description.
	///</summary>
	[JsonPropertyName("description")]
	public string Description { get; set; }

	///<summary>
	///The timestamp of when the video was created.<br/> 
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset CreatedAt { get; set; }

	///<summary>
	///The timestamp of when the video was published.<br/>
	///This is the same as CreatedAt
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("published_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset PublishedAt { get; set; }

	///<summary>
	///The video's URL - uses the Id, not StreamId.
	///</summary>
	[JsonPropertyName("url")]
	public string Url { get; set; }

	///<summary>
	///A URL to a thumbnail image of the video - uses the StreamId<br/>
	///Before using the URL, you must replace the %{width} and %{height} placeholders with the width and height of the thumbnail you want returned.<br/>
	///Due to current limitations, ${width} must be 320 and ${height} must be 180.
	///</summary>
	[JsonPropertyName("thumbnail_url")]
	public string ThumbnailUrl { get; set; }

	///<summary>
	///The video's viewable state. Always set to public.
	///</summary>
	[JsonPropertyName("viewable")]
	public string Viewable { get; set; }

	///<summary>
	///The number of times that users have watched the video.
	///</summary>
	[JsonPropertyName("view_count")]
	public int ViewCount { get; set; }

	///<summary>
	///The ISO 639-1 two-letter language code that the video was broadcast in.<br/>
	///For example, the language code is DE if the video was broadcast in German.<br/>
	///For a list of supported languages, see Supported Stream Language.<br/>
	///The language value is "other" if the video was broadcast in a language not in the list of supported languages.
	///</summary>
	[JsonPropertyName("language")]
	public string Language { get; set; }

	///<summary>
	///The video's type.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(VideoTypeEnumConverter))]
	public VideoType VideoType { get; set; }

	///<summary>
	///The video's length in ISO 8601 duration format.<br/>
	///For example, 3m21s represents 3 minutes, 21 seconds.
	///</summary>
	[JsonPropertyName("duration")]
	[JsonConverter(typeof(IsoTimespanConverter))]
	public TimeSpan Duration { get; set; }

	///<summary>
	///The segments that Twitch Audio Recognition muted; otherwise, null.
	///</summary>
	[JsonPropertyName("muted_segments")]
	public List<MutedSegment>? MutedSegments { get; set; }
}