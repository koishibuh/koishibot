using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#search-channels">Twitch Documentation</see><br/>
	/// Gets the channels that match the specified query and have streamed content within the past 6 months.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task SearchChannels(SearchChannelsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		const string url = "search/channels";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

/*════════════════【 REQUEST BODY 】════════════════*/
public class SearchChannelsRequestParameters
{
	///<summary>
	/// The URI-encoded search string.<br/>
	/// For example, encode search strings like angel of death as angel%20of%20death
	///</summary>
	[JsonPropertyName("query")]
	public string Query { get; set; }

	///<summary>
	///A Boolean value that determines whether the response includes only channels that are currently streaming live.<br/>
	///Set to true to get only channels that are streaming live; otherwise, false to get live and offline channels.<br/>
	///The default is false.
	///</summary>
	[JsonPropertyName("live_only")]
	public bool LiveOnly { get; set; }

	///<summary>
	/// 	The maximum number of items to return per page in the response.<br/>
	/// 	The minimum page size is 1 item per page and the maximum is 100 items per page. The default is 20.
	///</summary>
	[JsonPropertyName("first")]
	public int First { get; set; }

	///<summary>
	///The cursor used to get the next page of results.<br/>
	///The Pagination object in the response contains the cursor’s value
	///</summary>
	[JsonPropertyName("after")]
	public string After { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class SearchChannelsResponse
{
	///<summary>
	///The list of channels that match the query. The list is empty if there are no matches.
	///</summary>
	[JsonPropertyName("data")]
	public List<SearchedChannelData> Data { get; set; }
}

public class SearchedChannelData
{
	///<summary>
	///The ISO 639-1 two-letter language code of the language used by the broadcaster.<br/>
	///For example, en for English. If the broadcaster uses a language not in the list of supported stream languages, the value is other.
	///</summary>
	[JsonPropertyName("broadcaster_language")]
	public string BroadcasterLanguage { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("display_name")]
	public string DisplayName { get; set; }

	///<summary>
	///The ID of the game that the broadcaster is playing or last played.
	///</summary>
	[JsonPropertyName("game_id")]
	public string GameId { get; set; }

	///<summary>
	///The name of the game that the broadcaster is playing or last played.
	///</summary>
	[JsonPropertyName("game_name")]
	public string GameName { get; set; }

	///<summary>
	///An ID that uniquely identifies the channel (this is the broadcaster’s ID).
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster is streaming live.<br/>
	///Is true if the broadcaster is streaming live; otherwise, false.
	///</summary>
	[JsonPropertyName("is_live")]
	public bool IsLive { get; set; }

	///<summary>
	///The tags applied to the channel.
	///</summary>
	[JsonPropertyName("tags")]
	public List<string> Tags { get; set; }

	///<summary>
	///A URL to a thumbnail of the broadcaster’s profile image.
	///</summary>
	[JsonPropertyName("thumbnail_url")]
	public string ThumbnailUrl { get; set; }

	///<summary>
	///The stream’s title. Is an empty string if the broadcaster didn’t set it.
	///</summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }

	///<summary>
	///The timestamp of when the broadcaster started streaming.<br/>
	///The string is empty if the broadcaster is not streaming live.
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset? StartedAt { get; set; }
}