using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-emotes">Twitch Documentation</see><br/>
	/// Gets the broadcaster’s list of custom emotes.<br/>
	/// Broadcasters create these custom emotes for users who subscribe to or follow the channel or cheer Bits in the channel’s chat window.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetChannelEmotes(GetChannelEmotesRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "chat/emotes";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetChannelEmotesRequestParameters
{
	///<summary>
	///An ID that identifies the broadcaster whose emotes you want to get
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class GetChannelEmotesResponse
{

	///<summary>
	///The list of emotes that the specified broadcaster created.<br/>
	///If the broadcaster hasn't created custom emotes, the list is empty.
	///</summary>
	[JsonPropertyName("data")]
	public List<GetChannelEmotesData> Data { get; set; }
}

public class GetChannelEmotesData
{
	///<summary>
	///An ID that identifies this emote.
	///</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	///<summary>
	///The name of the emote.<br/>
	///This is the name that viewers type in the chat window to get the emote to appear.
	///</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	///<summary>
	///The image URLs for the emote. <br/>
	///These image URLs always provide a static, non-animated emote image with a light background.<br/>
	///NOTE: You should use the templated URL in the template field to fetch the image instead of using these URLs.
	///</summary>
	[JsonPropertyName("images")]
	public ImageSizes Images { get; set; }

	///<summary>
	///The subscriber tier at which the emote is unlocked. This field contains the tier information only if emote_type is set to subscriptions, otherwise, it's an empty string.
	///</summary>
	[JsonPropertyName("tier")]
	public string Tier { get; set; }

	///<summary>
	///The type of emote.
	///</summary>
	[JsonPropertyName("emote_type")]
	[JsonConverter(typeof(EmoteTypeEnumConverter))]
	public EmoteType EmoteType { get; set; }

	///<summary>
	///An ID that identifies the emote set that the emote belongs to.
	///</summary>
	[JsonPropertyName("emote_set_id")]
	public string EmoteSetId { get; set; }

	///<summary>
	///The formats that the emote is available in.<br/>
	///For example, if the emote is available only as a static PNG, the array contains only static.
	///If the emote is available as a static PNG and an animated GIF, the array contains static and animated.<br/>
	///</summary>
	[JsonPropertyName("format")]
	[JsonConverter(typeof(EmoteFormatEnumConverter))]
	public List<EmoteFormat> Format { get; set; }

	///<summary>
	///The sizes that the emote is available in.</br>
	///</summary>
	[JsonPropertyName("scale")]
	[JsonConverter(typeof(ImageScaleEnumConverter))]
	public List<ImageScale> Scale { get; set; }

	///<summary>
	///The background themes that the emote is available in. 
	///</summary>
	[JsonPropertyName("theme_mode")]
	[JsonConverter(typeof(ThemeModeEnumConverter))]
	public List<ThemeMode> ThemeMode { get; set; }

	///<summary>
	///A templated URL.<br/>
	///Use the values from the id, format, scale, and theme_mode fields to replace the like-named placeholder strings in the templated URL to create a CDN (content delivery network) URL that you use to fetch the emote.<br/>
	///For information about what the template looks like and how to use it to fetch emotes, see Emote CDN URL format.<br/>
	///You should use this template instead of using the URLs in the images object.
	///</summary>
	[JsonPropertyName("template")]
	public string Template { get; set; }
}