using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-vips">Twitch Documentation</see><br/>
	/// Gets a list of the broadcaster’s VIPs.<br/>
	/// Required Scopes: channel:read:vips<br/>
	/// </summary>
	public async Task GetVips(GetVipsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "channels/vips";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetVipsRequestParameters
{
	///<summary>
	///Filters the list for specific VIPs.<br/>
	///To specify more than one user, include the user_id parameter for each user to get.<br/>
	///For example, user_id=1234 user_id=5678. The maximum number of IDs that you may specify is 100.<br/>
	///Ignores the ID of those users in the list that aren’t VIPs.
	///</summary>
	[JsonPropertyName("user_id")]
	public List<string> VipIds { get; set; }

	///<summary>
	///The ID of the broadcaster whose list of VIPs you want to get.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

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

public class GetVipsResponseBody
{
	///<summary>
	///The list of VIPs. The list is empty if the broadcaster doesn’t have VIP users.
	///</summary>
	[JsonPropertyName("data")]
	public List<VipUser> Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}

public class VipUser
{
	///<summary>
	///An ID that uniquely identifies the VIP user.
	///</summary>
	[JsonPropertyName("user_id")]
	public string VipUserId { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string VipUserName	{ get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string VipUserLogin { get; set; }
}