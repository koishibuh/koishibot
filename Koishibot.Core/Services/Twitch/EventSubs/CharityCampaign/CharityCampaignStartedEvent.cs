using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.CharityCampaign;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaignstart">Twitch Documentation</see><br/>
/// When a user donates to the broadcaster’s charity campaign. It’s possible to receive this event after the Progress event.<br/>
/// Required Scopes: channel:read:charity<br/>
/// </summary>
public class CharityCampaignStartedEvent
{
	///<summary>
	///An ID that identifies the charity campaign.
	///</summary>
	[JsonPropertyName("id")]
	public string? Id { get; set; }

	///<summary>
	///An ID that identifies the broadcaster that’s running the campaign.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string? BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string? BroadcasterLogin { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string? BroadcasterName { get; set; }

	///<summary>
	///The charity’s name.
	///</summary>
	[JsonPropertyName("charity_name")]
	public string? CharityName { get; set; }

	///<summary>
	///A description of the charity.
	///</summary>
	[JsonPropertyName("charity_description")]
	public string? CharityDescription { get; set; }

	///<summary>
	///A URL to an image of the charity’s logo. The image’s type is PNG and its size is 100px X 100px.
	///</summary>
	[JsonPropertyName("charity_logo")]
	public string? CharityLogo { get; set; }

	///<summary>
	///A URL to the charity’s website.
	///</summary>
	[JsonPropertyName("charity_website")]
	public string? CharityWebsite { get; set; }

	///<summary>
	///An object that contains the current amount of donations that the campaign has received.
	///</summary>
	[JsonPropertyName("current_amount")]
	public Amount? CurrentAmount { get; set; }

	///<summary>
	///An object that contains the campaign’s target fundraising goal.
	///</summary>
	[JsonPropertyName("target_amount")]
	public Amount? TargetAmount { get; set; }

	///<summary>
	///The timestamp of when the broadcaster started the campaign.<br/>
	///(RFC3339 converted to DateTimeOffset) 
	///</summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset StartedAt { get; set; }
}