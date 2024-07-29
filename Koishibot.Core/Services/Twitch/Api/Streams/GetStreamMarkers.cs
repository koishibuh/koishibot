using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-stream-markers">Twitch Documentation</see><br/>
	/// Gets a list of markers from the user’s most recent stream or from the specified VOD/video.<br/>
	/// Required Scopes: user:read:broadcast or channel:manage:broadcast<br/>
	/// </summary>
	public async Task GetStreamMarkers(GetStreamMarkersRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "streams/markers";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetStreamMarkersRequestParameters
{
	///<summary>
	///A user ID.<br/>
	///The request returns the markers from this user’s most recent video.<br/>
	///This ID must match the user ID in the access token or the user in the access token must be one of the broadcaster’s editors.<br/>
	///This parameter and the video_id query parameter are mutually exclusive.
	///</summary>
	[JsonPropertyName("user_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///A video on demand (VOD)/video ID. The request returns the markers from this VOD/video.<br/>
	///The user in the access token must own the video or the user must be one of the broadcaster’ editors.<br/>
	///This parameter and the user_id query parameter are mutually exclusive.
	///</summary>
	[JsonPropertyName("video_id")]
	public string VideoId { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page.<br/>
	///The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public string First { get; set; }

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
}

// == ⚫ RESPONSE BODY == //

public class GetStreamMarkersResponse
{
	///<summary>
	///The list of markers grouped by the user that created the marks.
	///</summary>
	[JsonPropertyName("data")]
	public List<StreamMarkerData> Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}

public class StreamMarkerData
{
	///<summary>
	///The ID of the user that created the marker.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string UserName { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; }

	///<summary>
	///A list of videos that contain markers.<br/>
	///The list contains a single video.
	///</summary>
	[JsonPropertyName("videos")]
	public List<Video> Videos { get; set; }

	///<summary>
	///The list of markers in this video.<br/>
	///The list in ascending order by when the marker was created.
	///</summary>
	[JsonPropertyName("markers")]
	public List<Marker> Markers { get; set; }
}

public class Video
{
	///<summary>
	///An ID that identifies this video.
	///</summary>
	[JsonPropertyName("video_id")]
	public string VideoId { get; set; }
}

public class Marker
{
	///<summary>
	///An ID that identifies this marker.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///The timestamp of when the user created the marker.<br/>
	///(RFC3339 format converted to DateTimeOffset) 
	///</summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset CreatedAt { get; set; }

	///<summary>
	///The description that the user gave the marker to help them remember why they marked the location.<br/>
	///Is an empty string if the user didn’t provide one.
	///</summary>
	[JsonPropertyName("description")]
	public string Description { get; set; }

	///<summary>
	///The relative offset (in seconds) of the marker from the beginning of the stream.
	///</summary>
	[JsonPropertyName("position_seconds")]
	public int PositionInSeconds { get; set; }

	///<summary>
	///A URL that opens the video in Twitch Highlighter.
	///</summary>
	[JsonPropertyName("url")]
	public string Url { get; set; }
}