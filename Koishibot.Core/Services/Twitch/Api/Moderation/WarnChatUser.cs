using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#warn-chat-user">Twitch Documentation</see><br/>
	/// Warns a user in the specified broadcaster’s chat room, preventing them from chat interaction until the warning is acknowledged. New warnings can be issued to a user when they already have a warning in the channel (new warning will replace old warning).<br/>
	/// Required Scopes: moderator:manage:warnings<br/>
	/// </summary>
	public async Task WarnChatUser(WarnChatUserRequestParameters parameters,
		WarnChatUserRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "moderation/warnings";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class WarnChatUserRequestParameters
{
	///<summary>
	///The ID of the channel in which the warning will take effect.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the twitch user who requested the warning.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class WarnChatUserRequestBody
{
	///<summary>
	///A list that contains information about the warning.
	///</summary>
	[JsonPropertyName("data")]
	public List<WarnRequestData> Data { get; set; }
}

public class WarnRequestData
{
	///<summary>
	///The ID of the twitch user to be warned.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///A custom reason for the warning. Max 500 chars.
	///</summary>
	[JsonPropertyName("reason")]
	public string Reason { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class WarnChatUserResponse
{
	///<summary>
	///A list that contains information about the warning.
	///</summary>
	[JsonPropertyName("data")]
	public List<WarningData> Data { get; set; }
}

public class WarningData
{
	///<summary>
	///The ID of the channel in which the warning will take effect.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the warned user.
	///</summary>
	[JsonPropertyName("user_id")]
	public string WarnedUserId { get; set; }

	///<summary>
	///The ID of the user who applied the warning.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }

	///<summary>
	///The reason provided for warning.
	///</summary>
	[JsonPropertyName("reason")]
	public string Reason { get; set; }
}