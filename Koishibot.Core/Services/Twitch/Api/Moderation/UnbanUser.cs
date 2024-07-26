using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#unban-user">Twitch Documentation</see><br/>
	/// Removes the ban or timeout that was placed on the specified user.<br/>
	/// Required Scopes: moderator:manage:banned_users<br/>
	/// </summary>
	public async Task UnbanUser(UnbanUserRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "moderation/bans";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class UnbanUserRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose chat room the user is banned from chatting in.
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
	///The ID of the user to remove the ban or timeout from.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UnbannedUserId { get; set; }
}