using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-clips">Twitch Documentation</see><br/>
	/// ets one or more video clips that were captured from streams.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task<GetClipsResponse> GetClips(GetClipsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "clips";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<GetClipsResponse>(response)
			?? throw new Exception("Failed to deserialize response");

		return result;
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetClipsRequestParameters
{
	///<summary>
	///An ID that identifies the broadcaster whose video clips you want to get.<br/>
	///Use this parameter to get clips that were captured from the broadcaster’s streams.<br/>
	///REQUIRED.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;

	///<summary>
	///An ID that identifies the game whose clips you want to get.<br/>
	///Use this parameter to get clips that were captured from streams that were playing this game.
	///</summary>
	[JsonPropertyName("game_id")]
	public string? GameId { get; set; }

	///<summary>
	///An ID that identifies the clip to get.<br/>
	///To specify more than one ID, include this parameter for each clip you want to get.<br/>
	///For example, id=foo id=bar. You may specify a maximum of 100 IDs.<br/>
	///The API ignores duplicate IDs and IDs that aren’t found.
	///</summary>
	[JsonPropertyName("id")]
	public string? ClipId { get; set; }

	///<summary>
	///The start date used to filter clips.<br/>
	///The API returns only clips within the start and end date window.<br/>
	///Specify the date and time in RFC3339 format.
	///</summary>
	[JsonPropertyName("started_at")]
	[JsonConverter(typeof(DateTimeRFC3339Converter))]
	public DateTime StartedAt { get; set; }

	///<summary>
	///The end date used to filter clips.<br/>

	///If not specified, the time window is the start date plus one week.<br/>
	///Specify the date and time in RFC3339 format.
	///</summary>
	[JsonPropertyName("ended_at")]
	[JsonConverter(typeof(DateTimeRFC3339Converter))]
	public DateTime EndedAt { get; set; }

	///<summary>
	///The maximum number of clips to return per page in the response.<br/>
	///The minimum page size is 1 clip per page and the maximum is 100. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int ResultsPerPage { get; set; }

	///<summary>
	///The cursor used to get the previous page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("before")]
	public string Before { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }

	///<summary>
	///A Boolean value that determines whether the response includes featured clips.<br/>
	///If true, returns only clips that are featured.<br/>
	///If false, returns only clips that aren’t featured.<br/>
	///All clips are returned if this parameter is not present.
	///</summary>
	[JsonPropertyName("is_featured")]
	public bool IsFeatured { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetClipsResponse
{

	///<summary>
	///The list of video clips.<br/>
	///For clips returned by game_id or broadcaster_id, the list is in descending order by view count.<br/>
	///For lists returned by id, the list is in the same order as the input IDs.
	///</summary>
	[JsonPropertyName("data")]
	public List<ClipData> Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}

public class ClipData
{
	///<summary>
	///An ID that uniquely identifies the clip.
	///</summary>
	[JsonPropertyName("id")]
	public string ClipId { get; set; }

	///<summary>
	///A URL to the clip.
	///</summary>
	[JsonPropertyName("url")]
	public string ClipUrl	{ get; set; }

	///<summary>
	///A URL that you can use in an iframe to embed the clip (see Embedding Video and Clips).
	///</summary>
	[JsonPropertyName("embed_url")]
	public string EmbedUrl { get; set; }

	///<summary>
	///An ID that identifies the broadcaster that the video was clipped from.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///An ID that identifies the user that created the clip.
	///</summary>
	[JsonPropertyName("creator_id")]
	public string ClipCreatorId { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("creator_name")]
	public string ClipCreatorName { get; set; }

	///<summary>
	///An ID that identifies the video that the clip came from.<br/>
	///This field contains an empty string if the video is not available.
	///</summary>
	[JsonPropertyName("video_id")]
	public string VideoId { get; set; }

	///<summary>
	///The ID of the game that was being played when the clip was created.
	///</summary>
	[JsonPropertyName("game_id")]
	public string GameId { get; set; }

	///<summary>
	///The ISO 639-1 two-letter language code that the broadcaster broadcasts in.<br/>
	///For example, en for English. The value is other if the broadcaster uses a language that Twitch doesn’t support.
	///</summary>
	[JsonPropertyName("language")]
	public string Language { get; set; }

	///<summary>
	///The title of the clip.
	///</summary>
	[JsonPropertyName("title")]
	public string ClipTitle { get; set; }

	///<summary>
	///The number of times the clip has been viewed.
	///</summary>
	[JsonPropertyName("view_count")]
	public int ViewCount { get; set; }

	///<summary>
	///The timestamp when the clip was created.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]	
	public DateTimeOffset CreatedAt { get; set; }

	///<summary>
	///A URL to a thumbnail image of the clip.
	///</summary>
	[JsonPropertyName("thumbnail_url")]
	public string ThumbnailUrl { get; set; }

	///<summary>
	///The length of the clip, in seconds. Precision is 0.1.
	///</summary>
	[JsonPropertyName("duration")]
	public int DurationInSeconds { get; set; }

	///<summary>
	///The zero-based offset, in seconds, to where the clip starts in the video (VOD.)<br/>
	///Is null if the video is not available or hasn’t been created yet from the live stream (see video_id).<br/>
	///Note that there’s a delay between when a clip is created during a broadcast and when the offset is set.<br/>
	///During the delay period, vod_offset is null. The delay is indeterminant but is typically minutes long.
	///</summary>
	[JsonPropertyName("vod_offset")]
	public int VodOffset { get; set; }

	///<summary>
	///A Boolean value that indicates if the clip is featured or not.
	///</summary>
	[JsonPropertyName("is_featured")]
	public bool IsFeatured { get; set; }
}