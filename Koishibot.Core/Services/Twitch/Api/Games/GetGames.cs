using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-games">Twitch Documentation</see><br/>
	/// Gets information about specified categories or games.<br/>
	/// You may get up to 100 categories or games by specifying their ID or name. You may specify all IDs, all names, or a combination of IDs and names.<br/>
	/// If you specify a combination of IDs and names, the total number of IDs and names must not exceed 100.
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetGames(GetGamesRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "helix/games";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetGamesRequestParameters
{
	///<summary>
	///The ID of the category or game to get.<br/>
	///Include this parameter for each category or game you want to get.<br/>
	///For example, id=1234 id=5678. You may specify a maximum of 100 IDs.<br/>
	///The endpoint ignores duplicate and invalid IDs or IDs that weren’t found.
	///</summary>
	[JsonPropertyName("id")]
	public List<string> GameOrCategoryIds { get; set; }

	///<summary>
	///The name of the category or game to get.<br/>
	///The name must exactly match the category’s or game’s title.>br/>
	///Include this parameter for each category or game you want to get.<br/>
	///For example, name=foo name=bar. You may specify a maximum of 100 names.<br/>
	///The endpoint ignores duplicate names and names that weren’t found.
	///</summary>
	[JsonPropertyName("name")]
	public List<string> GameOrCategoryNames { get; set; }

	///<summary>
	///The IGDB ID of the game to get.<br/>
	///Include this parameter for each game you want to get.<br/>
	///For example, igdb_id=1234 igdb_id=5678. You may specify a maximum of 100 IDs.<br/>
	///The endpoint ignores duplicate and invalid IDs or IDs that weren’t found.
	///</summary>
	[JsonPropertyName("igdb_id")]
	public List<string> InternetGameDatabaseIds { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetGamesRequestResponse
{
	///<summary>
	///The list of categories and games.<br/>
	///The list is empty if the specified categories and games weren’t found.
	///</summary>
	[JsonPropertyName("data")]
	public List<GameCategoryData> Data { get; set; }
}