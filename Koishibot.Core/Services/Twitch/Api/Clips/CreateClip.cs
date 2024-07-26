using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#create-clip">Twitch Documentation</see><br/>
	/// Creates a clip from the broadcaster’s stream.<br/>
	/// Captures up to 90 seconds of the broadcaster’s stream. Check docs for more info.
	/// Required Scopes: clips:edit<br/>
	/// </summary>
	public async Task CreateClip(CreateClipRequestParameters parameters)
	{
		var method = HttpMethod.Post;
		var url = "clips";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class CreateClipRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose stream you want to create a clip from.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///A Boolean value that determines whether the API captures the clip at the moment the viewer requests it or after a delay.<br/>
	///If false (default), Twitch captures the clip at the moment the viewer requests it (this is the same clip experience as the Twitch UX).<br/>
	///If true, Twitch adds a delay before capturing the clip (this basically shifts the capture window to the right slightly).
	///</summary>
	[JsonPropertyName("has_delay")]
	public bool HasDelay { get; set; }
}

// == ⚫ RESPONSE BODY == //

public class CreateClipResponse
{
	///<summary>
	///A URL that you can use to edit the clip’s title, identify the part of the clip to publish, and publish the clip.
	///The URL is valid for up to 24 hours or until the clip is published, whichever comes first.
	///</summary>
	[JsonPropertyName("edit_url")]
	public string EditUrl { get; set; }

	///<summary>
	///An ID that uniquely identifies the clip.
	///</summary>
	[JsonPropertyName("id")]
	public string ClipId { get; set; }
}
