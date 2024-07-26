using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-channel-stream-schedule">Twitch Documentation</see><br/>
	/// Updates the broadcaster’s schedule settings, such as scheduling a vacation.<br/>
	/// Required Scopes: channel:manage:schedule<br/>
	/// </summary>
	public async Task UpdateStreamSchedule(UpdateStreamScheduleRequestParameters parameters)
	{
		var method = HttpMethod.Patch;
		var url = "schedule/settings";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class UpdateStreamScheduleRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose schedule settings you want to update.<br/>
	///The ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///A Boolean value that indicates whether the broadcaster has scheduled a vacation.<br/>
	///Set to true to enable Vacation Mode and add vacation dates, or false to cancel a previously scheduled vacation.
	///</summary>
	[JsonPropertyName("is_vacation_enabled")]
	public bool IsVacationEnabled { get; set; }

	///<summary>
	///The UTC date and time of when the broadcaster’s vacation starts.<br/>
	///Specify the date and time in RFC3339 format (for example, 2021-05-16T00:00:00Z).<br/>
	///Required if is_vacation_enabled is true.
	///</summary>
	[JsonPropertyName("vacation_start_time")]
	public string VacationStartTime { get; set; }

	///<summary>
	///The UTC date and time of when the broadcaster’s vacation ends.<br/>
	///Specify the date and time in RFC3339 format (for example, 2021-05-30T23:59:59Z).<br/>
	///Required if is_vacation_enabled is true.
	///</summary>
	[JsonPropertyName("vacation_end_time")]
	public string VacationEndTime { get; set; }

	///<summary>
	///The time zone that the broadcaster broadcasts from.<br/>
	///Specify the time zone using IANA time zone database format (for example, America/New_York).<br/>
	///Required if is_vacation_enabled is true.
	///</summary>
	[JsonPropertyName("timezone")]
	public string Timezone { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value. 
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }
}