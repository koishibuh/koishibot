using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-global-emotes">Twitch Documentation</see><br/>
	/// Gets the list of global emotes. Global emotes are Twitch-created emotes that users can use in any Twitch chat.<br/>
	/// Required Scopes: chat/emotes/global<br/>
	/// </summary>
	public async Task GetGlobalEmotes()
	{
		var method = HttpMethod.Get;
		var url = "chat/emotes/global";

		var response = await TwitchApiClient.SendRequest(method, url);
	}
}

// == ⚫ RESPONSE BODY == //

public class GetGlobalEmotesResponse
{

	///<summary>
	///The list of global emotes.
	///</summary>
	[JsonPropertyName("data")]
	public List<GetGlobalEmotesData>? Data { get; set; }
}

public class GetGlobalEmotesData
{
	///<summary>
	///An ID that identifies this emote.
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
	public ImageSizes? Images { get; set; }

	///<summary>
	///The formats that the emote is available in.<br/>
	///For example, if the emote is available only as a static PNG, the array contains only static.
	///If the emote is available as a static PNG and an animated GIF, the array contains static and animated.<br/>
	///</summary>
	[JsonPropertyName("format")]
	public List<EmoteFormat>? Format { get; set; }

	///<summary>
	///The sizes that the emote is available in.</br>
	///</summary>
	[JsonPropertyName("scale")]
	public List<ImageScale>? Scale { get; set; }

	///<summary>
	///The background themes that the emote is available in. 
	///</summary>
	[JsonPropertyName("theme_mode")]
	public List<ThemeMode>? ThemeMode { get; set; }

	///<summary>
	///A templated URL.<br/>
	///Use the values from the id, format, scale, and theme_mode fields to replace the like-named placeholder string?s in the templated URL to create a CDN (content delivery network) URL that you use to fetch the emote.<br/>
	///For information about what the template looks like and how to use it to fetch emotes, see Emote CDN URL format.<br/>
	///You should use this template instead of using the URLs in the images object.
	///</summary>
	[JsonPropertyName("template")]
	public string? Template { get; set; }
}