using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-banned-users">Twitch Documentation</see><br/>
	/// Gets all users that the broadcaster banned or put in a timeout.<br/>
	/// Required Scopes: moderation:read or moderator:manage:banned_users<br/>
	/// </summary>
	public async Task GetBannedUsers(GetBannedUsersRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		const string url = "moderation/banned";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

/*════════════════【 REQUEST BODY 】════════════════*/
public class GetBannedUsersRequestParameters
{

	///<summary>
	///The ID of the broadcaster whose list of banned users you want to get.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string Broadcaster { get; set; }

	///<summary>
	///A list of user IDs used to filter the results.<br/>
	///To specify more than one ID, include this parameter for each user you want to get.<br/>
	///For example, user_id=1234 user_id=5678. You may specify a maximum of 100 IDs.<br/>
	///The returned list includes only those users that were banned or put in a timeout.<br/>
	///The list is returned in the same order that you specified the IDs.
	///</summary>
	[JsonPropertyName("user_id")]
	public List<string> UserIds { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }

	///<summary>
	///The cursor used to get the previous page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("before")]
	public string Before { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetBannedUsersResponse
{
	///<summary>
	///The list of users that were banned or put in a timeout.
	///</summary>
	[JsonPropertyName("data")]
	public List<BannedUserListData> Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}

public class BannedUserListData
{
	///<summary>
	///The ID of the banned user.
	///</summary>
	[JsonPropertyName("user_id")]
	public string BannedUserId { get; set; }

	///<summary>
	///The banned user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string BannedUserLogin { get; set; }

	///<summary>
	///The banned user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string BannedUsername { get; set; }

	///<summary>
	///The timestamp of when the timeout expires, or an empty string if the user is permanently banned.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("expires_at")]
	public DateTimeOffset? ExpiresAt { get; set; }

	///<summary>
	///The timestamp of when the user was banned.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	public DateTimeOffset? CreatedAt { get; set; }

	///<summary>
	///The reason the user was banned or put in a timeout if the moderator provided one.
	///</summary>
	[JsonPropertyName("reason")]
	public string BanReason { get; set; }

	///<summary>
	///The ID of the moderator that banned the user or put them in a timeout.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }

	///<summary>
	///The moderator’s login name.
	///</summary>
	[JsonPropertyName("moderator_login")]
	public string ModeratorLogin { get; set; }

	///<summary>
	///The moderator’s disp
	///</summary>
	[JsonPropertyName("moderator_name")]
	public string ModeratorName { get; set; }
}