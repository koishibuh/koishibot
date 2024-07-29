using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-custom-reward">Twitch Documentation</see><br/>
	/// Gets a list of redemptions for the specified custom reward. The app used to create the reward is the only app that may get the redemptions.<br/>
	/// Required Scopes: channel:read:redemptions or channel:manage:redemptions<br/>
	/// </summary>
	public async Task GetRewardRedemptions(GetRewardRedemptionsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "channel_points/custom_rewards/redemptions";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetRewardRedemptionsRequestParameters
{
	///<summary>
	///The ID of the broadcaster that owns the custom reward. This ID must match the user ID found in the user OAuth token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID that identifies the custom reward whose redemptions you want to get.
	///</summary>
	[JsonPropertyName("reward_id")]
	public string RewardId { get; set; }

	///<summary>
	///The status of the redemptions to return.<br/>
	///	NOTE: This field is required only if you don’t specify the id query parameter.<br/>
	///	NOTE: Canceled and fulfilled redemptions are returned for only a few days after they’re canceled or fulfilled.
	///</summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(RewardRedemptionStatusEnumConverter))]
	public RewardRedemptionStatus Status { get; set; }

	///<summary>
	///A list of IDs to filter the redemptions by.<br/>
	///To specify more than one ID, include this parameter for each redemption you want to get.<br/>
	///For example, id=1234 id=5678. You may specify a maximum of 50 IDs.<br/>
	///Duplicate IDs are ignored.The response contains only the IDs that were found.<br/>
	///If none of the IDs were found, the response is 404 Not Found.
	///</summary>
	[JsonPropertyName("id")]
	public List<string> Id { get; set; }

	///<summary>
	///The order to sort redemptions by. <br/>
	///The default is OLDEST.
	///</summary>
	[JsonPropertyName("sort")]
	[JsonConverter(typeof(RewardRedemptionSortEnumConverter))]
	public RewardRedemptionSort Sort { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value. Read more
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }

	///<summary>
	///The maximum number of redemptions to return per page in the response.<br/>
	///The minimum page size is 1 redemption per page and the maximum is 50. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetCustomRewardRedemptionsResponse
{
	///<summary>
	///The list of redemptions for the specified reward.<br/>
	///The list is empty if there are no redemptions that match the redemption criteria.
	///</summary>
	[JsonPropertyName("data")]
	public List<CustomRewardRedemptionData> CustomRewardData { get; set; }
}


