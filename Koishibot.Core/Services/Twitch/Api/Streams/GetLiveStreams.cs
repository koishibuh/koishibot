using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json;
namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-streams">Twitch Documentation</see><br/>
	/// Gets a list of all streams.<br/>
	/// The list is in descending order by the number of viewers watching the stream.<br/>
	/// Because viewers come and go during a stream, it’s possible to find duplicate or missing streams in the list as you page through the results.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task<GetLiveStreamsResponse> GetLiveStreams(GetLiveStreamsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		const string url = "streams";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<GetLiveStreamsResponse>(response)
			?? throw new Exception("Failed to deserialize response");

		return result;
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //
public class GetLiveStreamsRequestParameters
{
	///<summary>
	///A user ID used to filter the list of streams.<br/>
	///Returns only the streams of those users that are broadcasting.<br/>
	///You may specify a maximum of 100 IDs. To specify multiple IDs, include the user_id parameter for each user.<br/>
	///For example, user_id=1234 user_id=5678.
	///</summary>
	[JsonPropertyName("user_id")]
	public List<string>? UserIds { get; set; }

	///<summary>
	///A user login name used to filter the list of streams.<br/>
	///Returns only the streams of those users that are broadcasting.<br/>
	///You may specify a maximum of 100 login names. To specify multiple names, include the user_login parameter for each user.<br/>
	///For example, user_login=foo user_login=bar.
	///</summary>
	[JsonPropertyName("user_login")]
	public List<string>? UserLogins { get; set; }

	///<summary>
	///A game (category) ID used to filter the list of streams.<br/>
	///Returns only the streams that are broadcasting the game (category).<br/>
	///You may specify a maximum of 100 IDs. To specify multiple IDs, include the game_id parameter for each game.<br/>
	///For example, game_id=9876 game_id=5432.
	///</summary>
	[JsonPropertyName("game_id")]
	public string? GameId { get; set; }

	///<summary>
	///The type of stream to filter the list of streams by.<br/>
	///Values are all or live, default is all.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(StreamTypeEnumConverter))]
	public StreamType? Type { get; set; }

	///<summary>
	///A language code used to filter the list of streams.<br/>
	///Returns only streams that broadcast in the specified language.<br/>
	///Specify the language using an ISO 639-1 two-letter language code or other if the broadcast uses a language not in the list of supported stream languages.
	///You may specify a maximum of 100 language codes. To specify multiple languages, include the language parameter for each language.<br/>
	///For example, language= de  language = fr.
	///</summary>
	[JsonPropertyName("language")]
	public List<string>? Language { get; set; }

	///<summary>
	///The maximum number of items to return per page in the response.<br/>
	///The minimum page size is 1 item per page and the maximum is 100 items per page. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the previous page of results.<br/>
	///The Pagination object in the response contains the cursor’s value. 
	///</summary>
	[JsonPropertyName("before")]
	public string? Before { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value.
	///</summary>
	[JsonPropertyName("after")]
	public string? After { get; set; }
}


// == ⚫ RESPONSE BODY == //
public class GetLiveStreamsResponse
{
	///<summary>
	///The list of streams.
	///</summary>
	[JsonPropertyName("data")]
	public List<LivestreamData>? Data { get; set; }

	///<summary>
	///The information used to page through the list of results.<br/>
	///The object is empty if there are no more pages left to page through.
	///</summary>
	[JsonPropertyName("pagination")]
	public Pagination? Pagination { get; set; }
}