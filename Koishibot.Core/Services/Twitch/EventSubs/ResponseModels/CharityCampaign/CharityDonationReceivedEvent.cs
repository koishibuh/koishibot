using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch.Common;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.CharityCampaign;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcharity_campaigndonate">Twitch Documentation</see><br/>
/// When a user donates to the broadcaster’s charity campaign.<br/>
/// Required Scopes: channel:read:charity<br/>
/// </summary>
public class CharityDonationReceivedEvent
{
    ///<summary>
    ///An ID that identifies the donation. The ID is unique across campaigns.
    ///</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    ///<summary>
    ///An ID that identifies the charity campaign.
    ///</summary>
    [JsonPropertyName("campaign_id")]
    public string CampaignId { get; set; }

    ///<summary>
    ///An ID that identifies the broadcaster that’s running the campaign.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The broadcaster’s login name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The broadcaster’s display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///An ID that identifies the user that donated to the campaign.
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
    ///An object that contains the amount of money that the user donated.
    ///</summary>
    [JsonPropertyName("amount")]
    public Amount Amount { get; set; }
}