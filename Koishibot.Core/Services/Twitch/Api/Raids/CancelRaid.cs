using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ DELETE == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#cancel-a-raid">Twitch Documentation</see><br/>
	/// Cancel a pending raid.<br/>
	/// The limit is 10 requests within a 10-minute window.<br/>
	/// Required Scopes: channel:manage:raids<br/>
	/// </summary>
	public async Task CancelRaid(CancelRaidRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "raids";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class CancelRaidRequestParameters
{
	///<summary>
	///The ID of the broadcaster that initiated the raid.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}