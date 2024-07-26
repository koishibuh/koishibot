using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.EventSubs.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-followed-channels">Twitch Documentation</see><br/>
	/// Gets a list of broadcasters that the specified user follows. You can also use this endpoint to see whether a user follows a specific broadcaster.<br/>
	/// Required Scopes: user:read:follows<br/>
	/// </summary>
	public async Task GetFollowedChannels(GetFollowedChannelsRequestParamaters parameters)
	{
		var method = HttpMethod.Get;
		var url = "channels/followed";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetFollowedChannelsRequestParamaters
{
	///<summary>
	///A user’s ID. Returns the list of broadcasters that this user follows.<br/>
	///This ID must match the user ID in the user OAuth token.
	///REQUIRED
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///A broadcaster’s ID. Use this parameter to see whether the user follows this broadcaster.<br/>
	///If specified, the response contains this broadcaster if the user follows them.<br/>
	///If not specified, the response contains all broadcasters that the user follows.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string? BroadcasterId { get; set; }

	///<summary>
	/// The maximum number of items to return per page in the response.<br/>
	/// The minimum page size is 1 item per page and the maximum is 100.<br/>
	/// The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int? First { get; set; }

	///<summary>
	/// The cursor used to get the next page of results.<br/>
	/// The Pagination object in the response contains the cursor’s value
	///</summary>
	[JsonPropertyName("after")]
	public string? After { get; set; }
}


// == ⚫ RESPONSE BODY == //

/// <summary>
///  List of broadcasters that the user follows.<br/>
///  The list is in descending order by followed_at (with the most recently followed broadcaster first).<br/>
///  The list is empty if the user doesn’t follow anyone.
/// </summary>
public class GetFollowedChannelsResponse
{
	[JsonPropertyName("data")]
	public List<FollowedChannelData> FollowedChannels { get; set; }

	///<summary>
	///Contains the information used to page through the list of results. The object is empty if there are no more pages left to page through. Read more.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }

	///<summary>
	///The total number of broadcasters that the user follows. As someone pages through the list, the number may change as the user follows or unfollows broadcasters.
	///</summary>
	[JsonPropertyName("total")]
	public int TotalChannelsFollowed { get; set; }
}


public class FollowedChannelData
{
	///<summary>
	///An ID that uniquely identifies the broadcaster that this user is following.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The timestamp when the user started following the broadcaster.
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("followed_at")]
	[JsonConverter(typeof(DateTimeOffsetConverter))]
	public DateTimeOffset FollowedAt { get; set; }
}