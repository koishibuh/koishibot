using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#add-channel-moderator">Twitch Documentation</see><br/>
	/// Adds a moderator to the broadcaster’s chat room.<br/>
	/// Required Scopes: channel:manage:moderators<br/>
	/// </summary>
	public async Task AddModerator(AddModeratorRequestParameter parameters)
	{
		var method = HttpMethod.Post;
		var url = "moderation/moderators";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class AddModeratorRequestParameter
{
	///<summary>
	///The ID of the broadcaster that owns the chat room.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the user to add as a moderator in the broadcaster’s chat room.
	///</summary>
	[JsonPropertyName("user_id")]
	public string NewModeratorId { get; set; }
}
