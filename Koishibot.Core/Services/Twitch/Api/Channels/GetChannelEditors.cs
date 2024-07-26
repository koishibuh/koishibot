using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
/// <summary>
 /// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-editors">Twitch Documentation</see><br/>
 /// Gets the broadcaster’s list editors.<br/>
 /// Required Scopes: channel:read:editors<br/>
 /// </summary>
	public async Task GetChannelEditors(GetChannelEditorsRequestParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "channels/editors";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class GetChannelEditorsRequestParameters
{
	///<summary>
	/// The ID of the broadcaster that owns the channel.<br/.
	/// This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;
}

// == ⚫ RESPONSE BODY == //

/// <summary>
/// List of users that are editors for the specified broadcaster.<br/>
/// The list is empty if the broadcaster doesn’t have editors.
/// </summary>
public class GetChannelEditorsResponse
{
	///<summary>
	///An ID that uniquely identifies a user with editor permissions.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string UserName { get; set; }

	///<summary>
	///The timestamp of when the user became one of the broadcaster’s editors.
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	public DateTimeOffset CreatedAt { get; set; }
}