using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	public async Task CreateStreamScheduleSegement
		(CreateStreamScheduleSegmentRequestParameters parameters, CreateStreamScheduleSegementRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "schedule/segment";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#create-channel-stream-schedule-segment">Twitch Documentation</see><br/>
/// Adds a single or recurring broadcast to the broadcaster’s streaming schedule.<br/>
/// Required Scopes: channel:manage:schedule<br/>
/// </summary>
public class CreateStreamScheduleSegmentRequestParameters
{
	///<summary>
	///The ID of the broadcaster that owns the schedule to add the broadcast segment to. This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class CreateStreamScheduleSegementRequestBody
{
	///<summary>
	///The date and time that the broadcast segment starts.<br/>
	///Specify the date and time in RFC3339 format (for example, 2021-07-01T18:00:00Z).
	///</summary>
	[JsonPropertyName("start_time")]
	public string StartTime { get; set; }

	///<summary>
	///The time zone where the broadcast takes place.<br/>
	///Specify the time zone using IANA time zone database format (for example, America/New_York).
	///</summary>
	[JsonPropertyName("timezone")]
	public string Timezone { get; set; }

	///<summary>
	///The length of time, in minutes, that the broadcast is scheduled to run.<br/>
	///The duration must be in the range 30 through 1380 (23 hours).
	///</summary>
	[JsonPropertyName("duration")]
	public string Duration { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcast recurs weekly.<br/>
	///Is true if the broadcast recurs weekly. Only partners and affiliates may add non-recurring broadcasts.
	///</summary>
	[JsonPropertyName("is_recurring")]
	public bool IsRecurring { get; set; }

	///<summary>
	///The ID of the category that best represents the broadcast’s content.<br/>
	///To get the category ID, use the Search Categories endpoint.
	///</summary>
	[JsonPropertyName("category_id")]
	public string CategoryId { get; set; }

	///<summary>
	///The broadcast’s title.<br/>
	///The title may contain a maximum of 140 characters.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }

}

// == ⚫ RESPONSE BODY == //

public class CreateStreamScheduleSegementResponse
{
	///<summary>
	///The broadcaster’s streaming schedule.
	///</summary>
	[JsonPropertyName("data")]
	public StreamScheduleData StreamScheduleData { get; set; }
}
