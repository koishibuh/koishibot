using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-user-block-list">Twitch Documentation</see><br/>
	/// Gets the list of users that the broadcaster has blocked.<br/>
	/// Required Scopes: user:read:blocked_users<br/>
	/// </summary>
	public async Task GetUserBlocklist(GetUserBlockListRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "users/blocks";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetUserBlockListRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose list of blocked users you want to get.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

///<summary>
///The maximum number of items to return per page in the response.<br/>
///The minimum page size is 1 item per page and the maximum is 100.<br/>
///The default is 20.
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

public class GetUserBlocklistResponse
{
	///<summary>
	///The list of blocked users.<br/>
	///The list is in descending order by when the user was blocked.
	///</summary>
	[JsonPropertyName("data")]
	public List<BlockedUserData> BlockedUsers { get; set; }
}

public class BlockedUserData
{
	///<summary>
	///An ID that identifies the blocked user.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The blocked user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; }

	///<summary>
	///The blocked user’s display name.
	///</summary>
	[JsonPropertyName("display_name")]
	public string DisplayName { get; set; }
}