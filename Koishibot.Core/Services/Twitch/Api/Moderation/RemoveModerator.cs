using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	public async Task RemoveModerator(RemoveModeratorRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "moderation/moderators";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class RemoveModeratorRequestParameters
{
	///<summary>
	///The ID of the broadcaster that owns the chat room.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	/// 	The ID of the user to remove as a moderator from the broadcaster’s chat room.
	///</summary>
	[JsonPropertyName("user_id")]
	public string ModeratorId { get; set; }
}