using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	public async Task RemoveVip(RemoveVipRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "channels/vips";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class RemoveVipRequestParameters
{
	///<summary>
	///The ID of the user to remove VIP status from.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The ID of the broadcaster who owns the channel where the user has VIP status.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}