using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ DELETE == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-custom-reward">Twitch Documentation</see><br/>
	/// Deletes a custom reward that the broadcaster created.<br/>
	/// Required Scopes: channel:manage:redemptions<br/>
	/// </summary>

	public async Task DeleteCustomReward(DeleteCustomRewardRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "channel_points/custom_rewards";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class DeleteCustomRewardRequestParameters
{
	///<summary>
	///The ID of the broadcaster that created the custom reward.<br/>
	///This ID must match the user ID found in the OAuth token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;

	///<summary>
	///The ID of the custom reward to delete.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; } = null!;

}