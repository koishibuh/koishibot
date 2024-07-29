using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-channel-stream-schedule-segment">Twitch Documentation</see><br/>
	/// Updates a scheduled broadcast segment.<br/>
	/// For recurring segments, updating a segment’s title, category, duration, and timezone, changes all segments in the recurring schedule, not just the specified segment.<br/>
	/// Required Scopes: channel:manage:schedule<br/>
	/// </summary>
	public async Task UpdateStreamScheduleSegment
		(UpdateStreamScheduleSegmentRequestParameters parameters, UpdateStreamScheduleSegmentRequestBody requestBody)
	{
		var method = HttpMethod.Patch;
		var url = "schedule/segment";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class UpdateStreamScheduleSegmentRequestParameters
{
	///<summary>
	///The ID of the broadcaster who owns the broadcast segment to update.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the broadcast segment to update.
	///</summary>
	[JsonPropertyName("id")]
	public string ScheduleSegmentId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class UpdateStreamScheduleSegmentRequestBody
{

	///<summary>
	///The date and time that the broadcast segment starts.<br/>
	///Specify the date and time in RFC3339 format (for example, 2022-08-02T06:00:00Z).<br/>
	///NOTE: Only partners and affiliates may update a broadcast’s start time and only for non-recurring segments.
	//////</summary>
	[JsonPropertyName("start_time")]
	public string StartTime { get; set; }

	///<summary>
	///The length of time, in minutes, that the broadcast is scheduled to run.<br/>
	///The duration must be in the range 30 through 1380 (23 hours).
	///</summary>
	[JsonPropertyName("duration")]
	public string DurationInMinutes { get; set; }

	///<summary>
	///The ID of the category that best represents the broadcast’s content.<br/>
	///To get the category ID, use the Search Categories endpoint.
	///</summary>
	[JsonPropertyName("category_id")]
	public string CategoryId { get; set; }

	///<summary>
	///The broadcast’s title. The title may contain a maximum of 140 characters.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }

	///<summary>
	///A Boolean value that indicates whether the broadcast is canceled.<br/>
	///Set to true to cancel the segment.<br/>
	///NOTE: For recurring segments, the API cancels the first segment after the current UTC date and time and not the specified segment<br/>
	///(unless the specified segment is the next segment after the current UTC date and time).
	///</summary>
	[JsonPropertyName("is_canceled")]
	public bool IsCanceled { get; set; }

	///<summary>
	///The time zone where the broadcast takes place.<br/>
	///Specify the time zone using IANA time zone database format (for example, America/New_York).
	///</summary>
	[JsonPropertyName("timezone")]
	public string Timezone { get; set; }
}

// == ⚫ RESPONSE BODY == //


public class UpdateStreamScheduleSegmentResponse
{
	///<summary>
	///The broadcaster’s streaming schedule.
	///</summary>
	[JsonPropertyName("data")]
	public StreamScheduleData StreamScheduleData { get; set; }
}
