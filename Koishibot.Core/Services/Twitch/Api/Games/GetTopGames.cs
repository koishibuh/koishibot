using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-top-games">Twitch Documentation</see><br/>
	/// Gets information about all broadcasts on Twitch.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetTopGames(GetTopGamesRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "games/top";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetTopGamesRequestParameters
{
	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page.<br/>
	///The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int MaxPerPageCount { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }

	///<summary>
	///The cursor used to get the previous page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("before")]
	public string Before { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetTopGamesResponse
{
	///<summary>
	///The list of broadcasts.<br/>
	///The broadcasts are sorted by the number of viewers, with the most popular first.
	///</summary>
	[JsonPropertyName("data")]
	public List<GameCategoryData> Data { get; set; }

	///<summary>
	///Contains the information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination Pagination { get; set; }
}
