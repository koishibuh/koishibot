using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-followers">Twitch Documentation</see><br/>
	/// Gets a list of users that follow the specified broadcaster. You can also use this endpoint to see whether a specific user follows the broadcaster.<br/>
	/// Required Scopes: moderator:read:followers<br/>
	/// </summary>
	public async Task GetFollowers(GetFollowersRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "channels/followers";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetFollowersRequestParameters
{
	///<summary>
	///A user’s ID. Use this parameter to see whether the user follows this broadcaster.<br/>
	///If specified, the response contains this user if they follow the broadcaster.<br/>
	///If not specified, the response contains all users that follow the broadcaster.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }
	///<summary>
	///The broadcaster’s ID. Returns the list of users that follow this broadcaster.
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

public class GetFollowersResponse
{
	///<summary>
	///The list of users that follow the specified broadcaster. The list is in descending order by followed_at (with the most recent follower first). The list is empty if nobody follows the broadcaster, the specified user_id isn’t in the follower list, the user access token is missing the moderator:read:followers scope, or the user isn’t the broadcaster or moderator for the channel.
	///</summary>
	[JsonPropertyName("data")]
	public List<FollowerData> Data { get; set; }

	///<summary>
	///Contains the information used to page through the list of results. The object is empty if there are no more pages left to page through. Read more.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }

	///<summary>
	///The total number of users that follow this broadcaster. As someone pages through the list, the number of users may change as users follow or unfollow the broadcaster.
	///</summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }
}

public class FollowerData
{
	///<summary>
	///The timestamp when the user started following the broadcaster.<br/>
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("followed_at")]
	public DateTimeOffset FollowedAt { get; set; }

	///<summary>
	///An ID that uniquely identifies the user that’s following the broadcaster.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string UserName { get; set; }
}