using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-chatters">Twitch Documentation</see><br/>
	/// Gets the list of users that are connected to the broadcaster’s chat session.<br/>
	/// Required Scopes: moderator:read:chatters<br/>
	/// </summary>
	public async Task GetChatters(GetChattersRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "chat/chatters";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetChattersRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose list of chatters you want to get.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;

	///<summary>
	///The ID of the broadcaster or one of the broadcaster’s moderators.<br/>
	///This ID must match the user ID in the user access token.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; } = null!;

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 1,000. The default is 100.
	///</summary>
	[JsonPropertyName("first")]
	public int? First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetChattersResponse
{
	///<summary>
	///The list of users that are connected to the broadcaster’s chat room. The list is empty if no users are connected to the chat room.
	///</summary>
	[JsonPropertyName("data")]
	public List<GetChattersData> Data { get; set; }
}

public class GetChattersData
{
	///<summary>
	///The ID of a user that’s connected to the broadcaster’s chat room.
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

	///<summary>
	///Contains the information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. Read More
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }

	///<summary>
	///The total number of users that are connected to the broadcaster’s chat room.<br/>
	///As you page through the list, the number of users may change as users join and leave the chat room.
	///</summary>
	[JsonPropertyName("total")]
	public int TotalChatters { get; set; }
}