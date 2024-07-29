using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ POST == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#send-chat-announcement">Twitch Documentation</see><br/>
	/// Sends an announcement to the broadcaster’s chat room<br/>
	/// Required Scopes: moderator:manage:announcements<br/>
	/// </summary>
	public async Task SendAnnouncement
			(SendAnnouncementRequestParameters parameters, SendAnnouncementRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		var url = "chat/announcements";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class SendAnnouncementRequestParameters
{
	///<summary>
	///The ID of the broadcaster that owns the chat room to send the announcement to.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of a user who has permission to moderate the broadcaster’s chat room,<br/>
	///or the broadcaster’s ID if they’re sending the announcement.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class SendAnnouncementRequestBody
{
	///<summary>
	///The announcement to make in the broadcaster’s chat room.<br/>
	///Announcements are limited to a maximum of 500 characters; announcements longer than 500 characters are truncated.
	///</summary>
	[JsonPropertyName("message")]
	public string Message { get; set; }

	///<summary>
	///The color used to highlight the announcement. Case-sensitive values.
	///If color is set to primary or is not set, the channel’s accent color is used to highlight the announcement.
	///</summary>
	[JsonPropertyName("color")]
	[JsonConverter(typeof(AnnouncementColorEnumConverter))]
	public AnnouncementColor Color { get; set; }
}
