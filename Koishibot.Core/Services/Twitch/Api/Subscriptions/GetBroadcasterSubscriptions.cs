using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-broadcaster-subscriptions">Twitch Documentation</see><br/>
	/// Gets a list of users that subscribe to the specified broadcaster.<br/>
	/// Required Scopes: channel:read:subscriptions<br/>
	/// </summary>
	public async Task GetBroadcasterSubscriptions(GetBroadcasterSubscriptionsParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "subscriptions";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //


public class GetBroadcasterSubscriptionsParameters
{

	///<summary>
	///The broadcaster’s ID. This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///Filters the list to include only the specified subscribers.<br/>
	///To specify more than one subscriber, include this parameter for each subscriber.<br/>
	///For example, user_id=1234 user_id=5678. You may specify a maximum of 100 subscribers.
	///</summary>
	[JsonPropertyName("user_id")]
	public List<string>? SubscriberUserId { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/.
	///The minimum page size is 1 item per page and the maximum is 100 items per page.<br/>
	///The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public string? First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///Do not specify if you set the user_id query parameter.<br>
	///</br> The Pagination object in the response contains the cursor’s value. Read More
	///</summary>
	[JsonPropertyName("after")]
	public string? After { get; set; }

	///<summary>
	///The cursor used to get the previous page of results.<br/>
	///Do not specify if you set the user_id query parameter.<br/>
	///The Pagination object in the response contains the cursor’s value. Read More
	///</summary>
	[JsonPropertyName("before")]
	public string? Before { get; set; }
}


// == ⚫ RESPONSE BODY == //

public class GetBroadcasterSubscriptionsResponse
{
	///<summary>
	///The list of users that subscribe to the broadcaster.</br/>
	///The list is empty if the broadcaster has no subscribers.
	///</summary>
	[JsonPropertyName("data")]
	public List<SubscriptionData> Subscribers { get; set; }

	///<summary>
	///Contains the information used to page through the list of results. The object is empty if there are no more pages left to page through. Read More
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }


	///<summary>
	///The current number of subscriber points earned by this broadcaster. Points are based on the subscription tier of each user that subscribes to this broadcaster. For example, a Tier 1 subscription is worth 1 point, Tier 2 is worth 2 points, and Tier 3 is worth 6 points. The number of points determines the number of emote slots that are unlocked for the broadcaster (see Subscriber Emote Slots).
	///</summary>
	[JsonPropertyName("points")]
	public int Points { get; set; }

	///<summary>
	///The total number of users that subscribe to this broadcaster.
	///</summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }
}

public class SubscriptionData
{
	//<summary>
	///An ID that identifies the broadcaster.
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
	///The ID of the user that gifted the subscription to the user.<br/>
	///Is an empty string if is_gift is false.
	///</summary>
	[JsonPropertyName("gifter_id")]
	public string GifterId { get; set; }

	///<summary>
	///The gifter’s login name. Is an empty string if is_gift is false.
	///</summary>
	[JsonPropertyName("gifter_login")]
	public string GifterLogin { get; set; }

	///<summary>
	///The gifter’s display name. Is an empty string if is_gift is false.
	///</summary>
	[JsonPropertyName("gifter_name")]
	public string GifterName { get; set; }

	///<summary>
	///A Boolean value that determines whether the subscription is a gift subscription.<br/>
	///Is true if the subscription was gifted.
	///</summary>
	[JsonPropertyName("is_gift")]
	public bool IsGift { get; set; }

	///<summary>
	///The name of the subscription.
	///</summary>
	[JsonPropertyName("plan_name")]
	public string PlanName { get; set; }

	///<summary>
	///The type of subscription.
	///</summary>
	[JsonPropertyName("tier")]
	[JsonConverter(typeof(SubTierEnumConverter))]
	public SubTier Tier { get; set; }

	///<summary>
	///An ID that identifies the subscribing user.
	///</summary>
	[JsonPropertyName("user_id")]
	public string SubscriberUserId { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string SubscriberUsername { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string SubscriberUserLogin { get; set; }
}