using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-shield-mode-status">Twitch Documentation</see><br/>
	/// ctivates or deactivates the broadcaster’s Shield Mode.<br/>
	/// Required Scopes: moderator:manage:shield_mode<br/>
	/// </summary>
	public async Task UpdateShieldModeStatus(UpdateShieldModeStatusRequestParameters parameters,
		UpdateShieldModeStatusRequestBody requestBody)
	{
		var method = HttpMethod.Put;
		var url = "moderation/shield_mode";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class UpdateShieldModeStatusRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose Shield Mode you want to activate or deactivate.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the broadcaster or a user that is one of the broadcaster’s moderators.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class UpdateShieldModeStatusRequestBody
{
	///<summary>
	///A Boolean value that determines whether to activate Shield Mode.<br/>
	///Set to true to activate Shield Mode; otherwise, false to deactivate Shield Mode.
	///</summary>
	[JsonPropertyName("is_active")]
	public bool ShieldModeStatus { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class UpdateShieldModeStatusResponse
{
	///<summary>
	///A list that contains a single object with the broadcaster’s Shield Mode status.
	///</summary>
	[JsonPropertyName("data")]
	public List<ShieldModeData> Data { get; set; }
}
