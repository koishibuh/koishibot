using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-shield-mode-status">Twitch Documentation</see><br/>
	/// Gets the broadcaster’s Shield Mode activation status.<br/>
	/// Required Scopes: moderator:read:shield_mode or moderator:manage:shield_mode<br/>
	/// </summary>
	public async Task GetShieldModeStatus(GetShieldModeStatusRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "moderation/shield_mode";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //


public class GetShieldModeStatusRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose Shield Mode activation status you want to get.
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

// == ⚫ RESPONSE BODY == //

public class GetShieldModeStatusResponse
{
	///<summary>
	///A list that contains a single object with the broadcaster’s Shield Mode status.
	///</summary>
	[JsonPropertyName("data")]
	public List<ShieldModeData> Data { get; set; }
}
