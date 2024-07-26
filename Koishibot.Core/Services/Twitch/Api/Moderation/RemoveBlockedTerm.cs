using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#remove-blocked-term">Twitch Documentation</see><br/>
	/// Removes the word or phrase from the broadcaster’s list of blocked terms.<br/>
	/// Required Scopes: moderator:manage:blocked_terms<br/>
	/// </summary>
	public async Task RemoveBlockedTerm(RemoveBlockedTermRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "moderation/blocked_terms";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

	// == ⚫ REQUEST QUERY PARAMETERS == //

	public class RemoveBlockedTermRequestParameters
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

	///<summary>
	///The ID of the blocked term to remove from the broadcaster’s list of blocked terms.
	///</summary>
	[JsonPropertyName("id")]
	public string BlockedTermId { get; set; }
}