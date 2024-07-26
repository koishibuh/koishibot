using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-followed-streams">Twitch Documentation</see><br/>
	/// Gets the list of broadcasters that the user follows and that are streaming live.<br/>
	/// Required Scopes: user:read:follows<br/>
	/// </summary>
	public async Task GetFollowedLiveStreams
		(GetFollowedLiveStreamsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "streams/followed";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetFollowedLiveStreamsRequestParameters
{
	///<summary>
	///The ID of the user whose list of followed streams you want to get.<br/>
	///This ID must match the user ID in the access token.
	///REQUIRED
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; } = null!;

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page.<br/>
	///The default is 100.
	///</summary>
	[JsonPropertyName("first")]
	public int? First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("after")]
	public string? After { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetFollowedLiveStreamsResponse
{
	///<summary>
	///The list of live streams of broadcasters that the specified user follows.<br/>
	///The list is in descending order by the number of viewers watching the stream.<br/>
	///Because viewers come and go during a stream, it’s possible to find duplicate or missing streams in the list as you page through the results.<br/>
	///The list is empty if none of the followed broadcasters are streaming live.
	///</summary>
	[JsonPropertyName("data")]
	public List<LivestreamData> Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}