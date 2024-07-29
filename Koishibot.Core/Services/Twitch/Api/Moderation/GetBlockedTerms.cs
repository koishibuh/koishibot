using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#remove-blocked-term">Twitch Documentation</see><br/>
	/// Gets the broadcaster’s list of non-private, blocked words or phrases. These are the terms that the broadcaster or moderator added manually or that were denied by AutoMod.<br/>
	/// Required Scopes: moderator:read:blocked_terms or moderator:manage:blocked_terms<br/>
	/// </summary>
	public async Task GetBlockedTerms(GetBlockedTermsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "moderation/blocked_terms";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetBlockedTermsRequestParameters
{

	///<summary>
	/// The ID of the broadcaster whose blocked terms you’re getting.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	/// The ID of the broadcaster or a user that has permission to moderate the broadcaster’s chat room.<br/>
	/// This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }

	///<summary>
	/// The maximum number of items to return per page in the response.<br/>
	/// The minimum page size is 1 item per page and the maximum is 100 items per page.<br/>
	/// The default is 20.
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

public class GetBlockedTermsResponseBody
{
	///<summary>
	///The list of blocked terms.<br/>
	///The list is in descending order of when they were created (see the created_at timestamp).
	///</summary>
	[JsonPropertyName("data")]
	public List<BlockedTermData> Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}