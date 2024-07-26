using Koishibot.Core.Services.Twitch.Common;

namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ PATCH == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#modify-channel-information">Twitch Documentation</see><br/>
	/// Updates a channel’s properties.<br/>
	/// All fields are optional, but you must specify at least one field.<br/>
	/// Required Scopes: channel:manage:broadcast<br/>
	/// </summary>
	public async Task EditChannelInfo
			(EditChannelInfoRequestParameters parameters, EditChannelInfoRequestBody requestBody)
	{
		var method = HttpMethod.Patch;
		var url = "channels";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class EditChannelInfoRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose channel you want to update.<br/>
	///This ID must match the user ID in the user access token.<br/>
	///REQUIRED
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;
}

// == ⚫ REQUEST BODY == //

/// <summary>
/// All fields are optional, but you must specify at least one field.
/// </summary>
public class EditChannelInfoRequestBody
{
	///<summary>
	///The ID of the game that the user plays.<br/>
	///The game is not updated if the ID isn’t a game ID that Twitch recognizes.<br/>
	///To unset this field, use “0” or “” (an empty string).
	///</summary>
	[JsonPropertyName("game_id")]
	public string? GameId { get; set; }

	///<summary>
	///The user’s preferred language.<br/>
	///Set the value to an ISO 639-1 two-letter language code (for example, en for English).<br/>
	///Set to “other” if the user’s preferred language is not a Twitch supported language.<br/>
	///The language isn’t updated if the language code isn’t a Twitch supported language.
	///</summary>
	[JsonPropertyName("broadcaster_language")]
	public string? BroadcasterLanguage { get; set; }

	///<summary>
	///The title of the user’s stream. You may not set this field to an empty string.<br/>
	///Maximum 140 character length.
	///</summary>
	[JsonPropertyName("title")]
	public string? Title { get; set; }

	///<summary>
	///The number of seconds you want your broadcast buffered before streaming it live.<br/>
	///The delay helps ensure fairness during competitive play.<br/>
	///Only users with Partner status may set this field.<br/>
	///The maximum delay is 900 seconds (15 minutes).
	///</summary>
	[JsonPropertyName("delay")]
	public int? DelayInSeconds { get; set; }

	///<summary>
	///A list of channel-defined tags to apply to the channel.<br/>
	///To remove all tags from the channel, set tags to an empty array.<br/>
	///Tags help identify the content that the channel streams.<br/>
	///Maximum of 10 tags, each tag is limited to a maximum of 25 characters and may not be an empty string, contain spaces or special characters.<br/>
	///Tags are case insensitive.For readability, consider using camelCasing or PascalCasing.
	///</summary>
	[JsonPropertyName("tags")]
	public List<string>? Tags { get; set; }

	///<summary>
	///List of labels that should be set as the Channel’s CCLs.
	///</summary>
	[JsonPropertyName("content_classification_labels")]
	//[JsonConverter(typeof(ContentClassificationLabelEnumListConverter))]
	public List<ContentClassificationLabelModel>? ContentClassificationLabels { get; set; }

	///<summary>
	///Boolean flag indicating if the channel has branded content.
	///</summary>
	[JsonPropertyName("is_branded_content")]
	public bool? IsBrandedContent { get; set; }
}