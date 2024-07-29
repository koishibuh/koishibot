using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;
namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-chat-messages">Twitch Documentation</see><br/>
	/// Removes a single chat message or all chat messages from the broadcaster’s chat room.<br/>
	/// Required Scopes: moderator:manage:chat_messages<br/>
	/// </summary>
	public async Task DeleteChatMessage(DeleteChatMessageParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "moderation/chat";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class DeleteChatMessageParameters
{
	///<summary>
	///The ID of the broadcaster that owns the chat room to remove messages from.
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
	///The ID of the message to remove. <br/>
	///The id tag in the PRIVMSG tag contains the message’s ID.<br/>
	///If not specified, the request removes all messages in the broadcaster’s chat room.
	///Restrictions:<br/>
	///The message must have been created within the last 6 hours.<br/>
	///The message must not belong to the broadcaster.<br/>
	///The message must not belong to another moderator.<br/>
	///</summary>
	[JsonPropertyName("message_id")]
	public string MessageId { get; set; }
}