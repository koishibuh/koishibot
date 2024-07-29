using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>^/// <see href="https://dev.twitch.tv/docs/api/reference/#check-user-subscription">Twitch Documentation</see><br/>
	/// Checks whether the user subscribes to the broadcaster’s channel.<br/>
	/// Required Scopes: user:read:subscriptions<br/>
	/// </summary>
	public async Task CheckUserSubscription
		(CheckUserSubscriptionRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "subscriptions/user";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class CheckUserSubscriptionRequestParameters
{
	///<summary>
	///The ID of a partner or affiliate broadcaster.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the user that you’re checking to see whether they subscribe to the broadcaster in broadcaster_id.<br/>
	///This ID must match the user ID in the access Token.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }
}


// == ⚫ RESPONSE BODY == //

public class CheckUserSubscriptionResponse
{
	///<summary>
	///A list that contains a single object with information about the user’s subscription.
	///</summary>
	[JsonPropertyName("data")]
	public List<UserSubscriptionData> Data { get; set; }
}

public class UserSubscriptionData
{
	///<summary>
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
	///The ID of the user that gifted the subscription.<br/>
	///The object includes this field only if is_gift is true.
	///</summary>
	[JsonPropertyName("gifter_id")]
	public string GifterId { get; set; }

	///<summary>
	///The gifter’s login name. The object includes this field only if is_gift is true.
	///</summary>
	[JsonPropertyName("gifter_login")]
	public string GifterLogin { get; set; }

	///<summary>
	///The gifter’s display name.<br/>
	///The object includes this field only if is_gift is true.
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
	///The type of subscription.
	///</summary>
	[JsonPropertyName("tier")]
	[JsonConverter(typeof(SubTierEnumConverter))]
	public SubTier SubTier { get; set; }
}