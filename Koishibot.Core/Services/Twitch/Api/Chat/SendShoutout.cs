using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;
namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-a-shoutout">Twitch Documentation</see><br/>
	/// Sends a Shoutout to the specified broadcaster.<br/>
	/// Rate Limits The broadcaster may send a Shoutout once every 2 minutes.<br/>
	/// They may send the same broadcaster a Shoutout once every 60 minutes.
	/// Required Scopes: moderator:manage:shoutouts
	/// </summary>
	public async Task SendShoutout(SendShoutoutParameters parameters)
	{
		var method = HttpMethod.Post;
		var url = "chat/shoutouts";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class SendShoutoutParameters
{
	///<summary>
	///The ID of the broadcaster that’s sending the Shoutout.
	///</summary>
	[JsonPropertyName("from_broadcaster_id")]
	public string FromBroadcasterId { get; set; }

	///<summary>
	///The ID of the broadcaster that’s receiving the Shoutout.
	///</summary>
	[JsonPropertyName("to_broadcaster_id")]
	public string ToBroadcasterId { get; set; }

	///<summary>
	///The ID of the broadcaster or a user that is one of the broadcaster’s moderators.<br/>
	///This ID must match the user ID in the access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }
}