using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#get-game-analytics">Twitch Documentation</see><br/>
/// Gets an analytics report for one or more games. The response contains the URLs used to download the reports (CSV files).<br/>
/// Required Scopes: analytics:read:games
/// </summary>
public partial record TwitchApiRequest : ITwitchApiRequest
{
	public async Task GetGameAnalytics(GetGameAnalyticsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "analytics/games";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetGameAnalyticsRequestParameters
{
	///<summary>
	///The game’s client ID.<br/>
	///If specified, the response contains a report for the specified game.<br/>
	///If not specified, the response includes a report for each of the authenticated user’s games.
	///</summary>
	[JsonPropertyName("game_id")]
	public string? GameId { get; set; }

	///<summary>
	///The type of analytics report to get. Possible values are:overview_v2
	///</summary>
	[JsonPropertyName("type")]
	public AnalyticsReportType? Type { get; set; }

	///<summary>
	///The reporting window’s start date, in RFC3339 format.<br/>
	///Set the time portion to zeroes (for example, 2021-10-22T00:00:00Z).<br/>
	///If you specify a start date, you must specify an end date.<br/>
	///The start date must be within one year of today’s date. If you specify an earlier date, the API ignores it and uses a date that’s one year prior to today’s date.<br/>
	///If you don’t specify a start and end date, the report includes all available data for the last 365 days from today.<br/>
	///The report contains one row of data for each day in the reporting window.
	///</summary>
	[JsonPropertyName("started_at")]
	public string? StartedAt { get; set; }

	///<summary>
	///The reporting window’s end date, in RFC3339 format.<br/>
	///Set the time portion to zeroes (for example, 2021-10-22T00:00:00Z).<br/>
	///The report is inclusive of the end date. Specify an end date only if you provide a start date.<br/>
	///Because it can take up to two days for the data to be available, you must specify an end date that’s earlier than today minus one to two days.If not, the API ignores your end date and uses an end date that is today minus one to two days.
	///</summary>
	[JsonPropertyName("ended_at")]
	public string? EndedAt { get; set; }

	///<summary>
	///The maximum number of report URLs to return per page in the response.<br/>
	///The minimum page size is 1 URL per page and the maximum is 100 URLs per page. The default is 20.<br/>
	///NOTE: While you may specify a maximum value of 100, the response will contain at most 20 URLs per page.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.<br/>
	///This parameter is ignored if game_id parameter is set.
	///</summary>
	[JsonPropertyName("after")]
	public string? After { get; set; }
}

// == ⚫ RESPONSE BODY == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#get-game-analytics">Twitch Documentation</see><br/>
/// A list of reports. The reports are returned in no particular order; however, the data within each report is in ascending order by date (newest first).<br/>
/// The report contains one row of data per day of the reporting window; the report contains rows for only those days that the game was used.<br/>
/// A report is available only if the game was broadcast for at least 5 hours over the reporting period.<br/>
/// The array is empty if there are no reports.<br/>
/// </summary>
public class GetGameAnalyticsResponse
{
	///<summary>
	///An ID that identifies the game that the report was generated for.
	///</summary>
	[JsonPropertyName("game_id")]
	public string GameId { get; set; }

	///<summary>
	///The URL that you use to download the report. The URL is valid for 5 minutes.
	///</summary>
	[JsonPropertyName("URL")]
	public string Url { get; set; }

	///<summary>
	///The type of report.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(AnalyticReportTypeEnumConverter))]
	public AnalyticsReportType Type { get; set; }

	///<summary>
	///The reporting window’s start and end dates, in RFC3339 format.
	///</summary>
	[JsonPropertyName("date_range")]
	public DateRange DateRange { get; set; }


	///<summary>
	///Contains the information used to page through the list of results. The object is empty if there are no more pages left to page through. Read More
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}