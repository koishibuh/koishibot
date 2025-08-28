using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-chat-badges">Twitch Documentation</see><br/>
	/// Gets the broadcaster’s list of custom chat badges. The list is empty if the broadcaster hasn’t created custom chat badges<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetChannelChatBadges(GetChannelChatBadgesParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "chat/badges";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
public class GetChannelChatBadgesParameters
{
	///<summary>
	///The ID of the broadcaster whose chat badges you want to get.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetChannelChatBadgesResponse
{
	///<summary>
	///The list of chat badges.<br/>
	///The list is sorted in ascending order by set_id, and within a set, the list is sorted in ascending order by id.
	///</summary>
	[JsonPropertyName("data")]
	public List<ChatBadgeData> Data { get; set; }
}