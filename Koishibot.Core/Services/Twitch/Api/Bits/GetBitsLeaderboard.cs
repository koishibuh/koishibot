using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-bits-leaderboard">Twitch Documentation</see><br/>
	/// Gets the Bits leaderboard for the authenticated broadcaster.<br/>
	/// Required Scopes: bits:read
	/// </summary>
	public async Task GetBitsLeaderboard(GetBitsLeaderboardRequestParamaters parameters)
	{
		var method = HttpMethod.Get;
		var url = "bits/leaderboard";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetBitsLeaderboardRequestParamaters
{
	///<summary>
	///The number of results to return.<br/>
	///The minimum count is 1 and the maximum is 100. The default is 10.
	///</summary>
	[JsonPropertyName("count")]
	public int Count { get; set; }

	///<summary>
	///The time period over which data is aggregated (uses the PST time zone).
	///</summary>
	[JsonPropertyName("period")]
	[JsonConverter(typeof(PeriodEnumConverter))]
	public Period Period { get; set; }

	///<summary>
	///The start date, in RFC3339 format, used for determining the aggregation period.<br/>
	///Specify this parameter only if you specify the period query parameter. The start date is ignored if period is all.<br/>
	///Note that the date is converted to PST before being used, so if you set the start time to 2022-01-01T00:00:00.0Z and period to month, the actual reporting period is December 2021, not January 2022.<br/>
	///If you want the reporting period to be January 2022, you must set the start time to 2022-01-01T08:00:00.0Z or 2022-01-01T00:00:00.0-08:00.<br/>
	/// If your start date uses the ‘+’ offset operator (for example, 2022-01-01T00:00:00.0+05:00), you must URL encode the start date.
	///</summary>
	[JsonPropertyName("started_at")]
	public string StartedAt { get; set; }

	///<summary>
	///An ID that identifies a user that cheered bits in the channel.<br/>
	///If count is greater than 1, the response may include users ranked above and below the specified user.<br/>
	///To get the leaderboard’s top leaders, don’t specify a user ID.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }
}


// == ⚫ RESPONSE BODY == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/api/reference/#get-bits-leaderboard">Twitch Documentation</see><br/>
/// Gets the Bits leaderboard for the authenticated broadcaster.<br/>
/// </summary>
public class GetBitsLeaderboardResponse
{
	///<summary>
	///An ID that identifies a user on the leaderboard.
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
	///The user’s position on the leaderboard.
	///</summary>
	[JsonPropertyName("rank")]
	public int Rank { get; set; }

	///<summary>
	///The number of Bits the user has cheered.
	///</summary>
	[JsonPropertyName("score")]
	public int Score { get; set; }

	///<summary>
	///The reporting window’s start and end dates, in RFC3339 format.<br/>
	///The dates are calculated by using the started_at and period query parameters.<br/>
	///If you don’t specify the started_at query parameter, the fields contain empty strings.
	///</summary>
	[JsonPropertyName("date_range")]
	public DateRange DateRange { get; set; }

	///<summary>
	///The number of ranked users in data.<br/>
	///This is the value in the count query parameter or the total number of entries on the leaderboard, whichever is less.
	///</summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }
}
