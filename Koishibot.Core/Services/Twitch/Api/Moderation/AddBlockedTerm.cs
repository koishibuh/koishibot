using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#add-blocked-term">Twitch Documentation</see><br/>
	/// Adds a word or phrase to the broadcaster’s list of blocked terms. These are the terms that the broadcaster doesn’t want used in their chat room.<br/>
	/// Required Scopes: moderator:manage:blocked_terms<br/>
	/// </summary>
	public async Task AddBlockedTerm
		(AddBlockedTermRequestParameters parameters, AddBlockedTermRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "moderation/blocked_terms";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class AddBlockedTermRequestParameters
{
	///<summary>
	///The ID of the broadcaster that owns the list of blocked terms.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the broadcaster or a user that has permission to moderate the broadcaster’s chat room.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class AddBlockedTermRequestBody
{
	///<summary>
	///The word or phrase to block from being used in the broadcaster’s chat room.<br/>
	///The term must contain a minimum of 2 characters and may contain up to a maximum of 500 characters.<br/>
	///Terms may include a wildcard character(*). The wildcard character must appear at the beginning or end of a word or set of characters.<br/>
	///For example, *foo or foo*.<br/>
	///	If the blocked term already exists, the response contains the existing blocked term.
	///</summary>
	[JsonPropertyName("text")]
	public string TermToBlock { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class AddBlockedTermResponse
{
	///<summary>
	///A list that contains the single blocked term that the broadcaster added.
	///</summary>
	[JsonPropertyName("data")]
	public List<BlockedTermData> Data { get; set; }
}

