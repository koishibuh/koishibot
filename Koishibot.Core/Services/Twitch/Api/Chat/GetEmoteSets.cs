using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-emote-sets">Twitch Documentation</see><br/>
	/// Gets emotes for one or more specified emote sets.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetEmoteSets(GetEmoteSetsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "chat/emotes/set";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetEmoteSetsRequestParameters
{
	///<summary>
	/// An ID that identifies the emote set to get.<br/>
	/// Include this parameter for each emote set you want to get.<br/>
	/// For example, emote_set_id=1234 emote_set_id=5678. You may specify a maximum of 25 IDs.<br/>
	/// The response contains only the IDs that were found and ignores duplicate IDs.
	///</summary>
	[JsonPropertyName("emote_set_id")]
	public string? EmoteSetId { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetEmoteSetsResponse
{

	///<summary>
	///The list of emotes found in the specified emote sets.<br/>
	///The list is empty if none of the IDs were found.<br/>
	///The list is in the same order as the set IDs specified in the request.<br/>
	///Each set contains one or more emoticons.
	///</summary>
	[JsonPropertyName("data")]
	public List<GetEmoteSetsData>? Data { get; set; }
}

public class GetEmoteSetsData
{

	///<summary>
	///An ID that uniquely identifies this emote.
	///</summary>
	[JsonPropertyName("id")]
	public string? Id { get; set; }

	///<summary>
	///The name of the emote.<br/>
	///This is the name that viewers type in the chat window to get the emote to appear.
	///</summary>
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	///<summary>
	///The image URLs for the emote. <br/>
	///These image URLs always provide a static, non-animated emote image with a light background.<br/>
	///NOTE: You should use the templated URL in the template field to fetch the image instead of using these URLs.
	///</summary>
	[JsonPropertyName("images")]
	public ImageSizes Images { get; set; }

	///<summary>
	///The type of emote.
	///</summary>
	[JsonPropertyName("emote_type")]
	public EmoteType EmoteType { get; set; }

	///<summary>
	///An ID that identifies the emote set that the emote belongs to.
	///</summary>
	[JsonPropertyName("emote_set_id")]
	public string? EmoteSetId { get; set; }

	///<summary>
	///The ID of the broadcaster who owns the emote.
	///</summary>
	[JsonPropertyName("owner_id")]
	public string? OwnerId { get; set; }

	///<summary>
	///The formats that the emote is available in.<br/>
	///For example, if the emote is available only as a static PNG, the array contains only static.
	///If the emote is available as a static PNG and an animated GIF, the array contains static and animated.<br/>
	///</summary>
	[JsonPropertyName("format")]
	//[JsonConverter(typeof(EmoteFormatEnumConverter))]
	//[JsonConverter(typeof(ListJsonConverter<EmoteFormat, EmoteFormatEnumConverter>))]
	//[JsonStringEnum<EmoteFormat>]
	public List<EmoteFormat>? Format { get; set; }

	///<summary>
	///The sizes that the emote is available in.</br>
	///</summary>
	[JsonPropertyName("scale")]
	public List<ImageScale> Scale { get; set; }

	///<summary>
	///The background themes that the emote is available in. 
	///</summary>
	[JsonPropertyName("theme_mode")]
	public List<ThemeMode> ThemeMode { get; set; }

	///<summary>
	///A templated URL.<br/>
	///Use the values from the id, format, scale, and theme_mode fields to replace the like-named placeholder string?s in the templated URL to create a CDN (content delivery network) URL that you use to fetch the emote.<br/>
	///For information about what the template looks like and how to use it to fetch emotes, see Emote CDN URL format.<br/>
	///You should use this template instead of using the URLs in the images object.
	///</summary>
	[JsonPropertyName("template")]
	public string? Template { get; set; }
}
