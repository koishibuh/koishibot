using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-charity-campaign">Twitch Documentation</see><br/>
	/// Gets information about the charity campaign that a broadcaster is running.<br/>
	/// For example, the campaign’s fundraising goal and the current amount of donations.<br/>
	/// Required Scopes: channel:read:charity<br/>
	/// </summary>
	public async Task GetCharityCampaign(GetCharityCampaignRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "charity/campaigns";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetCharityCampaignRequestParameters
{
	///<summary>
	///The ID of the broadcaster that’s currently running a charity campaign.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetCharityCampaignResponse
{
	///<summary>
	///A list that contains the charity campaign that the broadcaster is currently running.<br/>
	///The list is empty if the broadcaster is not running a charity campaign; the campaign information is not available after the campaign ends.
	///</summary>
	[JsonPropertyName("data")]
	public List<CharityCampaignData> Data { get; set; }
}

public class CharityCampaignData
{
	///<summary>
	///An ID that identifies the charity campaign.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///An ID that identifies the broadcaster that’s running the campaign.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The charity’s name.
	///</summary>
	[JsonPropertyName("charity_name")]
	public string CharityName { get; set; }

	///<summary>
	///A description of the charity.
	///</summary>
	[JsonPropertyName("charity_description")]
	public string CharityDescription { get; set; }

	///<summary>
	///A URL to an image of the charity’s logo.<br/>
	///The image’s type is PNG and its size is 100px X 100px.
	///</summary>
	[JsonPropertyName("charity_logo")]
	public string CharityLogo { get; set; }

	///<summary>
	///A URL to the charity’s website.
	///</summary>
	[JsonPropertyName("charity_website")]
	public string CharityWebsite { get; set; }

	///<summary>
	///The current amount of donations that the campaign has received.
	///</summary>
	[JsonPropertyName("current_amount")]
	public Amount CurrentAmount { get; set; }

	///<summary>
	///The campaign’s fundraising goal. This field is null if the broadcaster has not defined a fundraising goal.
	///</summary>
	[JsonPropertyName("target_amount")]
	public Amount TargetAmount { get; set; }
}