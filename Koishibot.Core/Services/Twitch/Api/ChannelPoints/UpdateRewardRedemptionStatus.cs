using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{ 
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-redemption-status">Twitch Documentation</see><br/>
	/// Updates a redemption’s status.<br/>
	/// You may update a redemption only if its status is UNFULFILLED.<br/>
	/// The app used to create the reward is the only app that may update the redemption.<br/>
	/// Required Scopes: channel:manage:redemptions<br/>
	/// </summary>
	public async Task UpdateRedemptionStatus
			(UpdateRedemptionStatusRequestParameters parameters, UpdateRedemptionStatusRequestBody requestBody)
	{
		var method = HttpMethod.Patch;
		var url = "channel_points/custom_rewards/redemptions";
		var query = parameters.ObjectQueryFormatter();
		var request = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, request);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class UpdateRedemptionStatusRequestParameters
{
	///<summary>
	///A list of IDs that identify the redemptions to update.<br/>
	///To specify more than one ID, include this parameter for each redemption you want to update.<br/>
	///For example, id=1234 id=5678. You may specify a maximum of 50 IDs.
	///</summary>
	[JsonPropertyName("id")]
	public List<string> Id { get; set; }

	///<summary>
	///The ID of the broadcaster that’s updating the redemption.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID that identifies the reward that’s been redeemed.
	///</summary>
	[JsonPropertyName("reward_id")]
	public string RewardId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class UpdateRedemptionStatusRequestBody
{
	///<summary>
	///The status to set the redemption to.<br/>
	///	Setting the status to CANCELED refunds the user’s channel points.
	///</summary>
	[JsonPropertyName("status")]
	public RewardRedemptionStatus Status { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class UpdateRewardRedemptionStatusResponse
{
	///<summary>
	///The list of redemptions for the specified reward.<br/>
	///The list is empty if there are no redemptions that match the redemption criteria.
	///</summary>
	[JsonPropertyName("data")]
	public List<CustomRewardRedemptionData> CustomRewardRedemptionData { get; set; }
}
