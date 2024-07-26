using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{

	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#search-categories">Twitch Documentation</see><br/>
	/// Gets the games or categories that match the specified query.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task SearchCategories(SearchCategoriesRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "search/categories";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class SearchCategoriesRequestParameters
{
	///<summary>
	///The URI-encoded search string.<br/>
	///For example, encode #archery as %23archery and search strings like angel of death as angel%20of%20death.
	///</summary>
	[JsonPropertyName("query")]
	public string Query { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page.<br/>
	///The default is 20.
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

public class SearchCategoriesResponse
{
	///<summary>
	///The list of games or categories that match the query.<br/>
	///The list is empty if there are no matches.
	///</summary>
	[JsonPropertyName("data")]
	public List<CategoryData> Data { get; set; }
}

public class CategoryData
{
	///<summary>
	///A URL to an image of the game’s box art or streaming category.
	///</summary>
	[JsonPropertyName("box_art_url")]
	public string BoxArtUrl { get; set; }

	///<summary>
	///The name of the game or category.
	///</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	///<summary>
	///An ID that uniquely identifies the game or category.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }
}