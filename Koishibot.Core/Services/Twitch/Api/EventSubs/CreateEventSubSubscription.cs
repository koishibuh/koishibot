using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json;
namespace Koishibot.Core.Services.TwitchApi.Models;


/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#create-eventsub-subscription">Twitch Documentation</see><br/>
/// Creates an EventSub subscription.<br/>
/// For Websockets:<br/>
/// the request must specify a user access token. The request will fail if you use an app access token.<br/>
/// If the subscription type requires user authorization, the token must include the required scope.<br/>
/// However, if the subscription type doesn’t include user authorization, the token may include any scopes or no scopes.
/// </summary>
public partial record TwitchApiRequest : ITwitchApiRequest
{
	public async Task CreateEventSubSubscription(List<CreateEventSubSubscriptionRequestBody> requestBody)
	{
		var url = "eventsub/subscriptions";

		var response = await TwitchApiClient.SendRequest(url, requestBody);

		foreach (var item in response)
		{
			var result = JsonSerializer.Deserialize<CreateEventSubSubscriptionResponse>(item);
		}

		Log.LogInformation($"Subscribed to {response.Count} events.");
	} 
}

// == ⚫ REQUEST BODY == //

public class CreateEventSubSubscriptionRequestBody
{
	///<summary>
	///The type of subscription to create.<br/>
	///For a list of subscription types: <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#subscription-types">EventSubs</see><br/>
	///Set this field to the value in the Name column of the Subscription Types table.<br/>
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(SubscriptionTypeEnumConverter))]
	public EventSubSubscriptionType SubscriptionType { get; set; }

	///<summary>
	///The timestamp of when the subscription was created.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset CreatedAt { get; set; }

	///<summary>
	///The version number that identifies the definition of the subscription type that you want the response to use.
	///</summary>
	[JsonPropertyName("version")]
	public string Version { get; set; } = null!;

	///<summary>
	///A JSON object that contains the parameter values that are specific to the specified subscription type.<br/>
	///For the object’s required and optional fields, see the subscription type’s documentation.
	///</summary>
	[JsonPropertyName("condition")]
	public Dictionary<string, string> Condition { get; set; } = null!;

	///<summary>
	///The transport details that you want Twitch to use when sending you notifications.
	///</summary>
	[JsonPropertyName("transport")]

	public TransportMethod Transport { get; set; } = null!;
}



// == ⚫ RESPONSE BODY == //

public class CreateEventSubSubscriptionResponse
{
	///<summary>
	///A list that contains the single subscription that you created.
	///</summary>
	[JsonPropertyName("data")]
	public List<EventSubSubscriptionData> Data { get; set; }

	///<summary>
	///The total number of subscriptions you’ve created.
	///</summary>
	[JsonPropertyName("total")]
	public int TotalSubscriptions { get; set; }

	///<summary>
	///The sum of all of your subscription costs. Learn More
	///</summary>
	[JsonPropertyName("total_cost")]
	public int TotalCost { get; set; }

	///<summary>
	///The maximum total cost that you’re allowed to incur for all subscriptions you create.
	///</summary>
	[JsonPropertyName("max_total_cost")]
	public int MaxTotalCost { get; set; }
}

public class EventSubSubscriptionData
{
	///<summary>
	///An ID that identifies the subscription.
	///</summary>
	[JsonPropertyName("id")]
	public string SubscriptionId { get; set; }

	///<summary>
	///The subscription’s status.<br/>
	///The subscriber receives events only for enabled subscriptions.<br/>
	///</summary>
	[JsonPropertyName("status")]
	public string Status { get; set; }

	///<summary>
	///The subscription’s type. See Subscription Types.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(SubscriptionTypeEnumConverter))]
	public EventSubSubscriptionType Type { get; init; }

	///<summary>
	///The version number that identifies this definition of the subscription’s data.
	///</summary>
	[JsonPropertyName("version")]
	public string Version { get; set; }

	///<summary>
	///The subscription’s parameter values.<br/>
	///This is a string-encoded JSON object whose contents are determined by the subscription type.
	///</summary>
	[JsonPropertyName("condition")]
	public Dictionary<string, string> Condition { get; set; } = null!;

	///<summary>
	///The transport details used to send the notifications.
	///</summary>
	[JsonPropertyName("transport")]
	public TransportMethod Transport { get; set; }

	///<summary>
	///The timestamp the WebSocket connection was established.
	///</summary>
	[JsonPropertyName("connected_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset ConnectedAt { get; set; }

	/////<summary>
	/////An ID that identifies the conduit to send notifications to.<br/>
	/////Included only if method is set to conduit.
	/////</summary>
	//[JsonPropertyName("conduit_id")]
	//public string ConduitId { get; set; }

	///<summary>
	///The amount that the subscription counts against your limit.
	///</summary>
	[JsonPropertyName("cost")]
	public int Cost { get; set; }
}