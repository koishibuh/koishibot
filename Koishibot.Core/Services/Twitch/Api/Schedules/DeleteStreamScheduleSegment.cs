using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch;

namespace Koishibot.Core.Services.TwitchApi.Models;


public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#delete-channel-stream-schedule-segment">Twitch Documentation</see><br/>
	/// Removes a broadcast segment from the broadcaster’s streaming schedule.<br/>
	/// For recurring segments, removing a segment removes all segments in the recurring schedule.<br/>
	/// Required Scopes: channel:manage:schedule<br/>
	/// </summary>
	public async Task DeleteStreamScheduleSegment(DeleteStreamScheduleSegmentRequestParameters parameters)
	{
		var method = HttpMethod.Delete;
		var url = "schedule/segment";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class DeleteStreamScheduleSegmentRequestParameters
{
	///<summary>
	///The ID of the broadcaster that owns the streaming schedule.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the broadcast segment to remove.
	///</summary>
	[JsonPropertyName("id")]
	public string StreamScheduleSegmentId { get; set; }
}

