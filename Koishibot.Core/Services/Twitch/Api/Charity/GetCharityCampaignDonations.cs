using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-charity-campaign-donations">Twitch Documentation</see><br/>
	/// Gets the list of donations that users have made to the broadcaster’s active charity campaign.<br/>
	/// Required Scopes: channel:read:charity<br/>
	/// </summary>
	public async Task GetCharityCampaignDonations(GetCharityCampaignDonationsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "charity/donations";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetCharityCampaignDonationsRequestParameters
{
	///<summary>
	///The ID of the broadcaster that’s currently running a charity campaign.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int? First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value. Read More
	///</summary>
	[JsonPropertyName("after")]
	public string? After { get; set; }
}

// == ⚫ RESPONSE BODY == //


public class GetCharityCampaignDonationsResponse
{
	///<summary>
	///A list that contains the donations that users have made to the broadcaster’s charity campaign.<br/>
	///The list is empty if the broadcaster is not currently running a charity campaign; the donation information is not available after the campaign ends.
	///</summary>
	[JsonPropertyName("data")]
	public List<CharityCampaignDonationData> Data { get; set; }
}

public class CharityCampaignDonationData
{
	///<summary>
	///An ID that identifies the donation. The ID is unique across campaigns.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///An ID that identifies the charity campaign that the donation applies to.
	///</summary>
	[JsonPropertyName("campaign_id")]
	public string CampaignId { get; set; }

	///<summary>
	///An ID that identifies a user that donated money to the campaign.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string UserName { get; set; }

	///<summary>
	///An object that contains the amount of money that the user donated.
	///</summary>
	[JsonPropertyName("amount")]
	public Amount Amount { get; set; }

	///<summary>
	///An object that contains the information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}
