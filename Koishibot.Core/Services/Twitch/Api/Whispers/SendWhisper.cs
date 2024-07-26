using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-whisper">Twitch Documentation</see><br/>
	/// Sends a whisper message to the specified user.<br/>
	/// You may whisper to a maximum of 40 unique recipients per day.<br/>
	/// Within the per day limit, you may whisper a maximum of 3 whispers per second and a maximum of 100 whispers per minute.<br/>
	/// Required Scopes: user:manage:whispers<br/>
	/// </summary>
	public async Task SendWhisper(SendWhisperRequestParameters parameters, SendWhisperRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "whispers";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //


public class SendWhisperRequestParameters
{
	///<summary>
	///The ID of the user sending the whisper.<br/>
	///This user must have a verified phone number.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("from_user_id")]
	public string SendingUserId { get; set; } = null!;

	///<summary>
	///The ID of the user to receive the whisper.
	///</summary>
	[JsonPropertyName("to_user_id")]
	public string RecievingUserId { get; set; } = null!;
}

// == ⚫ REQUEST BODY == //

public class SendWhisperRequestBody
{
	///<summary>
	///The whisper message to send. The message must not be empty.<br/>
	///Limits:<br/>
	///500 characters if the user you're sending the message to hasn't whispered you before.<br/>
	///10,000 characters if the user you're sending the message to has whispered you before.<br/>
	///Messages that exceed the maximum length are truncated.
	///</summary>
	[JsonPropertyName("message")]
	public string Message { get; set; } = null!;
}