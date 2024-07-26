using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-moderators">Twitch Documentation</see><br/>
	/// Gets all users allowed to moderate the broadcaster’s chat room.<br/>
	/// Required Scopes: moderation:read<br/>
	/// </summary>
	public async Task GetModerators(GetModeratorsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "moderation/moderators";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetModeratorsRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose list of moderators you want to get.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///A list of user IDs used to filter the results.<br/>
	///To specify more than one ID, include this parameter for each moderator you want to get.<br/>
	///For example, user_id=1234 user_id=5678. You may specify a maximum of 100 IDs.<br/>
	///The returned list includes only the users from the list who are moderators in the broadcaster’s channel.<br/>
	///The list is returned in the same order as you specified the IDs.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

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

public class GetModeratorsResponse
{
	///<summary>
	///The list of moderators.
	///</summary>
	[JsonPropertyName("data")]
	public List<ModeratorData> Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}

public class ModeratorData
{
	///<summary>
	///The ID of the user that has permission to moderate the broadcaster’s channel.
	///</summary>
	[JsonPropertyName("user_id")]
	public string ModeratorId { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string ModeratorLogin { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string ModeratorUsername { get; set; }
}