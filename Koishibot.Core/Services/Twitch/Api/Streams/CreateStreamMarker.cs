using Koishibot.Core.Services.Twitch.EventSubs.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ POST == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-stream-marker">Twitch Documentation</see><br/>
	/// Adds a marker to a live stream. A marker is an arbitrary point in a live stream that the broadcaster or editor wants to mark, so they can return to that spot later to create video highlights<br/>
	/// Cannot add markers if the stream is offline, stream does not have VODs enabled, stream is a premiere or rerun.
	/// Required Scopes: channel:manage:broadcast<br/>
	/// </summary>
	public async Task CreateStreamMarker(CreateStreamMarkerRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "streams/markers";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);
	}
}

// == ⚫ REQUEST BODY == //

public class CreateStreamMarkerRequestBody
{
	///<summary>
	///The ID of the broadcaster that’s streaming content.<br/>
	///This ID must match the user ID in the access token or the user in the access token must be one of the broadcaster’s editors.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///A short description of the marker to help the user remember why they marked the location.<br/>
	///The maximum length of the description is 140 characters.
	///</summary>
	[JsonPropertyName("description")]
	public string Description { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class CreateStreamMarkerResponse
{
	///<summary>
	///A list that contains the single marker that you added.
	///</summary>
	[JsonPropertyName("data")]
	public List<CreatedStreamMarkerData> Data { get; set; }
}

public class CreatedStreamMarkerData
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
	[JsonConverter(typeof(DateTimeOffsetConverter))]
	public string CreatedAt { get; set; }

	///<summary>
	///The relative offset (in seconds) of the marker from the beginning of the stream.
	///</summary>
	[JsonPropertyName("position_seconds")]

	public int PositionInSeconds { get; set; }
	///<summary>
	///A description that the user gave the marker to help them remember why they marked the location.
	///</summary>
	[JsonPropertyName("description")]
	public string Description { get; set; }
}