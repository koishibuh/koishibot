using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{

	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-polls">Twitch Documentation</see><br/>
	/// Gets a list of polls that the broadcaster created.<br/>
	/// Polls are available for 90 days after they’re created.<br/>
	/// Required Scopes: channel:read:polls or channel:manage:polls<br/>
	/// </summary>
	public async Task GetPolls(GetPollsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "polls";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //


public class GetPollsRequestParameters
{
	///<summary>
	///The ID of the broadcaster that created the polls.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///A list of IDs that identify the polls to return.<br/>
	///To specify more than one ID, include this parameter for each poll you want to get.<br/>
	///For example, id=1234 id=5678. You may specify a maximum of 20 IDs.<br/>
	///Specify this parameter only if you want to filter the list that the request returns.<br/>
	///The endpoint ignores duplicate IDs and those not owned by this broadcaster.
	///</summary>
	[JsonPropertyName("id")]
	public List<string>? PollIds { get; set; }

	///<summary>
	/// 	The maximum number of items to return per page in the response.<br/>
	/// 	The minimum page size is 1 item per page and the maximum is 100 items per page. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }
}


// == ⚫ RESPONSE BODY == //

public class GetPollsResponse
{
	///<summary>
	///A list of polls.<br/>
	///The polls are returned in descending order of start time unless you specify IDs in the request, in which case they're returned in the same order as you passed them in the request.<br/>
	///The list is empty if the broadcaster hasn't created polls.
	///</summary>
	[JsonPropertyName("data")]
	public List<PollData> Data { get; set; }


	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}