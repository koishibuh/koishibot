using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-information">Twitch Documentation</see><br/>
	/// Gets information about one or more channels.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task<List<ChannelInfoData>> GetChannelInfo(GetChannelInfoQueryParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "channels";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);

		var result = JsonSerializer.Deserialize<GetChannelInfoResponse>(response)
			?? throw new Exception("Failed to deserialize response");

		return result.ChannelInfos;
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetChannelInfoQueryParameters
{
	///<summary>
	///The ID of the broadcaster whose channel you want to get.<br/>
	///To specify more than one ID, include this parameter for each broadcaster you want to get.<br/>
	///For example, broadcaster_id=1234 broadcaster_id=5678. You may specify a maximum of 100 IDs.<br/>
	///The API ignores duplicate IDs and IDs that are not found.<br/>
	///REQUIRED.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public List<string> BroadcasterIds { get; set; } = null!;
}


// == ⚫ RESPONSE BODY == //

public class GetChannelInfoResponse
{
	///<summary>
	/// A list that contains information about the specified channels.<br/>
	/// The list is empty if the specified channels weren’t found.
	///</summary>
	[JsonPropertyName("data")]
	public List<ChannelInfoData> ChannelInfos { get; set; }
}

///<summary>
///<see href = "https://dev.twitch.tv/docs/api/reference/#get-channel-information" > Twitch Documentation</see><br/>
///Gets information about one or more channels.<br/>
///</summary>
public class ChannelInfoData
{
	///<summary>
	///An ID that uniquely identifies the broadcaster.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	///<summary>
	///The broadcaster’s preferred language.<br/>
	///The value is an ISO 639-1 two-letter language code (for example, en for English). <br/>
	///The value is set to “other” if the language is not a Twitch supported language.
	///</summary>
	[JsonPropertyName("broadcaster_language")]
	public string BroadcasterLanguage { get; set; } = string.Empty;

	///<summary>
	///The name of the game that the broadcaster is playing or last played.<br/>
	///The value is an empty string if the broadcaster has never played a game.
	///</summary>
	[JsonPropertyName("game_name")]
	public string CategoryName { get; set; } = string.Empty;

	///<summary>
	///An ID that uniquely identifies the game that the broadcaster is playing or last played.<br/>
	///The value is an empty string if the broadcaster has never played a game.
	///</summary>
	[JsonPropertyName("game_id")]
	public string CategoryId { get; set; } = string.Empty;

	///<summary>
	///The title of the stream that the broadcaster is currently streaming or last streamed.<br/>
	///The value is an empty string if the broadcaster has never streamed.
	///</summary>
	[JsonPropertyName("title")]
	public string StreamTitle { get; set; } = string.Empty;

	///<summary>
	///The value of the broadcaster’s stream delay setting, in seconds.<br/>
	///This field’s value defaults to zero unless:<br/>
	///1) the request specifies a user access token<br/>
	///2) the ID in the broadcaster_id query parameter matches the user ID in the access token<br/>
	///and 3) the broadcaster has partner status and they set a non-zero stream delay value.
	///</summary>
	[JsonPropertyName("delay")]
	public int DelayInSeconds { get; set; }

	///<summary>
	///The tags applied to the channel.
	///</summary>
	[JsonPropertyName("tags")]
	public List<string> Tags { get; set; }

	///<summary>
	///The CCLs applied to the channel.
	///</summary>
	[JsonPropertyName("content_classification_labels")]
	public List<ContentClassificationLabel> ContentClassificationLabels { get; set; }

	///<summary>
	///Boolean flag indicating if the channel has branded content.
	///</summary>
	[JsonPropertyName("is_branded_content")]
	public bool IsBrandedContent { get; set; }
}