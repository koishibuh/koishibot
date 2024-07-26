using Koishibot.Core.Services.Twitch.Common;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ GET == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-channel-chat-badges">Twitch Documentation</see><br/>
	/// Gets Twitch’s list of chat badges, which users may use in any channel’s chat room.<br/>
	/// Required Scopes: User Access Token<br/>
	/// </summary>
	public async Task GetGlobalChatBadges(GetChannelChatBadgesParameters parameters)
	{
		var method = HttpMethod.Get;
		var url = "chat/badges/global";

		var response = await TwitchApiClient.SendRequest(method, url);
	}
}

// == ⚫ RESPONSE BODY == //

public class GetGlobalChatBadgesResponse
{
	///<summary>
	///The list of chat badges.<br/>
	///The list is sorted in ascending order by set_id, and within a set, the list is sorted in ascending order by id.
	///</summary>
	[JsonPropertyName("data")]
	public List<ChatBadgeData> Data { get; set; }
}