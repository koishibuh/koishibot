using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-chat-settings">Twitch Documentation</see><br/>
	/// Gets the broadcaster’s chat settings.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetChatSettings(GetChatSettingsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "chat/settings";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetChatSettingsRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose chat settings you want to get.<br/
	///REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the broadcaster or one of the broadcaster’s moderators.
	/// This field is required only if you want to include the non_moderator_chat_delay and non_moderator_chat_delay_duration settings in the response.
	/// If you specify this field, this ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string? ModeratorId { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetChatSettingsResponse
{
	///<summary>
	///The list of chat settings. The list contains a single object with all the settings.
	///</summary>
	[JsonPropertyName("data")]
	public List<ChatSettingsData> Data { get; set; }
}

