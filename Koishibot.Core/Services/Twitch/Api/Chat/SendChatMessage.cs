using Koishibot.Core.Services.Twitch;
using System.Text.Json;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-message">Twitch Documentation</see><br/>
	/// Sends a message to the broadcaster’s chat room.<br/>
	/// Required Scopes: user:write:chat<br/>
	/// </summary>
	public async Task SendChatMessage(SendChatMessageRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "chat/messages";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);

		var result = JsonSerializer.Deserialize<SendChatMessageResponse>(response)
				?? throw new Exception("Failed to deserialize response");

		if (result.Data[0]?.WasSent == false)
			throw new Exception($"Message was not sent because: {result.Data[0].DropReason.DropMessage}");
	}
}

// == ⚫ REQUEST BODY == //

public class SendChatMessageRequestBody
{
	///<summary>
	///The ID of the broadcaster whose chat room the message will be sent to.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the user sending the message.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("sender_id")]
	public string SenderId { get; set; }

	///<summary>
	///The message to send.<br/>
	///The message is limited to a maximum of 500 characters.<br/>
	///Chat messages can also include emoticons. To include emoticons, use the name of the emote.<br/>
	///The names are case sensitive. Don’t include colons around the name (e.g., :bleedPurple:).<br/>
	///If Twitch recognizes the name, Twitch converts the name to the emote before writing the chat message to the chat room
	///</summary>
	[JsonPropertyName("message")]
	public string Message { get; set; }

	///<summary>
	///The ID of the chat message being replied to.
	///</summary>
	[JsonPropertyName("reply_parent_message_id")]
	public string ReplyParentMessageId { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class SendChatMessageResponse
{
	[JsonPropertyName("data")]
	public List<ChatMessageData> Data { get; set; }
}

public class ChatMessageData
{
	///<summary>
	///The message id for the message that was sent.
	///</summary>
	[JsonPropertyName("message_id")]
	public string ChatMessageId { get; set; }

	///<summary>
	///If the message passed all checks and was sent.
	///</summary>
	[JsonPropertyName("is_sent")]
	public bool WasSent { get; set; }

	///<summary>
	///The reason the message was dropped, if any.
	///</summary>
	[JsonPropertyName("drop_reason")]
	public DropReason DropReason { get; set; }
}

public class DropReason
{
	///<summary>
	///Code for why the message was dropped.
	///</summary>
	[JsonPropertyName("code")]
	public string DropCode { get; set; }

	///<summary>
	///Message for why the message was dropped.
	///</summary>
	[JsonPropertyName("message")]
	public string DropMessage { get; set; }
}