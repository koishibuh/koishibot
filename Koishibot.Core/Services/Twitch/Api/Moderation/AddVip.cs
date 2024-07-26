using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#add-channel-vip">Twitch Documentation</see><br/>
	/// Adds the specified user as a VIP in the broadcaster’s channel.<br/>
	/// Ratelimit of 10 VIPs within a 10 second window.<br/>
	/// Required Scopes: channel:manage:vips<br/>
	/// </summary>
	public async Task AddVip(AddVipRequestParameters parameters)
	{
		var method = HttpMethod.Post;
		var url = "channels/vips";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class AddVipRequestParameters
{
	///<summary>
	///The ID of the user to give VIP status to.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The ID of the broadcaster that’s adding the user as a VIP.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}