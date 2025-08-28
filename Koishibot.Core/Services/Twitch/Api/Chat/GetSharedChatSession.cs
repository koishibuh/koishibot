using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-shared-chat-session">Twitch Documentation</see><br/>
	/// Retrieves the active shared chat session for a channel.<br/>
	/// Requires an app access token or user access token.<br/>
	/// </summary>
	public async Task GetSharedChatSession(GetSharedChatSessionParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "shared_chat/session";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
public class GetSharedChatSessionParameters
{
	///<summary>
	/// The User ID of the channel broadcaster.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetSharedChatSessionResponse
{
	///<summary>
	/// The host channel and list of participant channels.
	///</summary>
	[JsonPropertyName("data")]
	public List<SharedChatSessionData> Data { get; set; }
}

public class SharedChatSessionData
{
	///<summary>
	/// The unique identifier for the shared chat session.
	///</summary>
	[JsonPropertyName("session_id")]
	public string SessionId { get; set; }
	
	///<summary>
	/// The User ID of the host channel.
	///</summary>
	[JsonPropertyName("host_broadcaster_id")]
	public string HostBroadcasterId { get; set; }
	
	///<summary>
	///The timestamp of when the session was created.<br/> 
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	public DateTimeOffset CreatedAt { get; set; }
	
	///<summary>
	///The timestamp of when the session was last updated.<br/> 
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("updated_at")]
	public DateTimeOffset UpdatedAt { get; set; }
}

public class Participants
{
	///<summary>
	///	The User ID of the participating channel.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}