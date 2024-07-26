using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-hype-train-events">Twitch Documentation</see><br/>
	/// Gets information about the broadcaster’s current or most recent Hype Train event.<br/>
	/// Required Scopes: channel:read:hype_train<br/>
	/// </summary>
	public async Task GetHypeTrainEvents(GetHypeTrainEventsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "hypetrain/events";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetHypeTrainEventsRequestParameters
{

	///<summary>
	///The ID of the broadcaster that’s running the Hype Train.<br/>
	///This ID must match the User ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page. The default is 1.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }


	// == ⚫ RESPONSE BODY == //


	public class GetHypeTrainEventsResponse
	{
		///<summary>
		///The list of Hype Train events.<br/>
		///The list is empty if the broadcaster hasn’t run a Hype Train within the last 5 days.
		///</summary>
		[JsonPropertyName("data")]
		public HypeTrainEventData Data { get; set; }

		///<summary>
		///The information used to page through the list of results.<br/>
		///The object is empty if there are no more pages left to page through. 
		///</summary>
		[JsonPropertyName("pagination")]
		public Pagination Pagination { get; set; }
	}

	public class HypeTrainEventData
	{
		///<summary>
		///An ID that identifies this event.
		///</summary>
		[JsonPropertyName("id")]
		public string HypeTrainEventId { get; set; }

		///<summary>
		///The type of event.<br/>
		///The string is in the form, hypetrain.{event_name}.<br/>
		///The request returns only progress event types (i.e., hypetrain.progression).
		///</summary>
		[JsonPropertyName("event_type")]
		public string EventType { get; set; }

		///<summary>
		///The timestamp when the event occurred.<br/>
		///(RFC3339 format converted to DateTimeOffset)
		///</summary>
		[JsonPropertyName("event_timestamp")]
		[JsonConverter(typeof(DateTimeOffsetConverter))]
		public DateTimeOffset EventTimestamp { get; set; }

		///<summary>
		///The version number of the definition of the event’s data.<br/>
		///For example, the value is 1 if the data in event_data uses the first definition of the event’s data.
		///</summary>
		[JsonPropertyName("version")]
		public string Version { get; set; }
		///<summary>
		///The event’s data.
		///</summary>
		[JsonPropertyName("event_data")]
		public EventData EventData { get; set; }

		///<summary>
		///The information used to page through the list of results.<br/>
		///The object is empty if there are no more pages left to page through. 
		///</summary>
		[JsonPropertyName("pagination")]
		public Pagination Pagination { get; set; }

	}

	public class EventData
	{
		///<summary>
		///The ID of the broadcaster that’s running the Hype Train.
		///</summary>
		[JsonPropertyName("broadcaster_id")]
		public string BroadcasterId { get; set; }

		///<summary>
		///The timestamp of when another Hype Train can start.<br/>
		///(RFC3339 format converted to DateTimeOffset)
		///</summary>
		[JsonPropertyName("cooldown_end_time")]
		[JsonConverter(typeof(DateTimeOffsetConverter))]
		public DateTimeOffset CooldownEndTime { get; set; }

		///<summary>
		///The timestamp of when the Hype Train ends.<br/>
		///(RFC3339 format converted to DateTimeOffset)
		///</summary>
		[JsonPropertyName("expires_at")]
		[JsonConverter(typeof(DateTimeOffsetConverter))]
		public DateTimeOffset ExpiresAt { get; set; }

		///<summary>
		///The value needed to reach the next level.
		///</summary>
		[JsonPropertyName("goal")]
		public int GoalToReachNextLevel { get; set; }

		///<summary>
		///An ID that identifies this Hype Train.
		///</summary>
		[JsonPropertyName("id")]
		public string HypeTrainId { get; set; }

		///<summary>
		///The most recent contribution towards the Hype Train’s goal.
		///</summary>
		[JsonPropertyName("last_contribution")]
		public HypeTrainContributor LastContribution { get; set; }

		///<summary>
		///The highest level that the Hype Train reached (the levels are 1 through 5).
		///</summary>
		[JsonPropertyName("level")]
		public int HypeTrainLevel { get; set; }

		///<summary>
		///The timestamp when this Hype Train started.<br/>
		///(RFC3339 format converted to DateTimeOffset)
		///</summary>
		[JsonPropertyName("started_at")]
		[JsonConverter(typeof(DateTimeOffsetConverter))]
		public DateTimeOffset StartedAt { get; set; }

		///<summary>
		///The top contributors for each contribution type.<br/>
		///For example, the top contributor using BITS (by aggregate) and the top contributor using SUBS (by count).
		///</summary>
		[JsonPropertyName("top_contributions")]
		public List<HypeTrainContributor> TopContributions { get; set; }

		///<summary>
		///The current total amount raised.
		///</summary>
		[JsonPropertyName("total")]
		public int TotalAmountRaised { get; set; }
	}
}

public class HypeTrainContributor
{
	///<summary>
	///The total amount contributed.<br/>
	///If type is BITS, total represents the amount of Bits used.<br/>
	///If type is SUBS, total is 500, 1000, or 2500 to represent tier 1, 2, or 3 subscriptions, respectively.
	///</summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }

	///<summary>
	///The contribution method used.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(ContributionApiTypeEnumConverter))]
	public ContributionApiType Type { get; set; }

	///<summary>
	///The ID of the user that made the contribution.
	///</summary>
	[JsonPropertyName("user")]
	public string User { get; set; }
}