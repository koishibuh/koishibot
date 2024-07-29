using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json;
namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-custom-reward">Twitch Documentation</see><br/>
	/// Gets a list of custom rewards that the specified broadcaster created.<br/>
	/// Required Scopes: channel:read:redemptions or channel:manage:redemptions<br/>
	/// </summary>
	public async Task<List<CustomRewardData>> GetCustomRewards(GetCustomRewardsParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "channel_points/custom_rewards";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<GetCustomRewardsResponse>(response)
			?? throw new Exception("Failed to deserialize response");

		return result.CustomRewardData;
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-custom-reward">Twitch Documentation</see><br/>
public class GetCustomRewardsParameters
{
	///<summary>
	///The ID of the broadcaster whose custom rewards you want to get.<br/>
	///This ID must match the user ID found in the OAuth token.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///A list of IDs to filter the rewards by.<br/>
	///To specify more than one ID, include this parameter for each reward you want to get.<br/>
	///For example, id=1234&id=5678. You may specify a maximum of 50 IDs.<br/>
	///Duplicate IDs are ignored. The response contains only the IDs that were found.<br/>
	///If none of the IDs were found, the response is 404 Not Found.</br>
	///OPTIONAL
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///A Boolean value that determines whether the response contains only the custom rewards that the app may manage (the app is identified by the ID in the Client-Id header).<br/>
	///Set to true to get only the custom rewards that the app may manage.<br/>
	/// OPTIONAL, the default is false.</br>
	///</summary>
	[JsonPropertyName("only_manageable_rewards")]
	public bool OnlyManageableRewards { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetCustomRewardsResponse
{
	///<summary>
	///A list of custom rewards.<br/>
	///The list is in ascending order by id.<br/>
	///If the broadcaster hasn’t created custom rewards, the list is empty.
	///</summary>
	[JsonPropertyName("data")]
	public List<CustomRewardData> CustomRewardData { get; set; }
}