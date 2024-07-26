using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-stream-schedule">Twitch Documentation</see><br/>
	/// Gets the broadcaster’s streaming schedule.<br/>
	/// You can get the entire schedule or specific segments of the schedule.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetStreamSchedule(GetChannelStreamScheduleRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "schedule";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetChannelStreamScheduleRequestParameters
{
	///<summary>
	///The ID of the broadcaster that owns the streaming schedule you want to get.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the scheduled segment to return.<br/>
	///To specify more than one segment, include the ID of each segment you want to get.<br/>
	///For example, id=1234 id=5678. You may specify a maximum of 100 IDs.
	///</summary>
	[JsonPropertyName("id")]
	public string ScheduleSegementId { get; set; }

	///<summary>
	///The timestamp that identifies when in the broadcaster’s schedule to start returning segments.<br/>
	///If not specified, the request returns segments starting after the current UTC date and time.<br/>
	///Specify the date and time in RFC3339 format (for example, 2022-09-01T00:00:00Z).
	///</summary>
	[JsonPropertyName("start_time")]
	public string StartTime { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 25 items per page. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetChannelStreamScheduleResponse
{
	///<summary>
	///The broadcaster’s streaming schedule.
	///</summary>
	[JsonPropertyName("data")]
	public StreamScheduleData StreamScheduleData { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through. 
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}

