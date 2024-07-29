using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#update-chat-settings">Twitch Documentation</see><br/>
	/// Updates the broadcaster’s chat settings.<br/>
	/// Required Scopes: moderator:manage:chat_settings<br/>
	/// </summary>
	public async Task UpdateChatSettings
		(UpdateChatSettingsRequestParamaters parameters, UpdateChatSettingsRequestBody requestBody )
	{
		var method = HttpMethod.Patch;
		var url = "chat/settings";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //
public class UpdateChatSettingsRequestParamaters
{
	///<summary>
	///The ID of the broadcaster whose chat settings you want to update.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of a user that has permission to moderate the broadcaster’s chat room, or the broadcaster’s ID if they’re making the update.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }
}

// == ⚫ REQUEST BODY == //

public class UpdateChatSettingsRequestBody
{
	///<summary>
	///A Boolean value that determines whether chat messages must contain only emotes.<br/>
	//Set to true if only emotes are allowed; otherwise, false. The default is false.
	///</summary>
	[JsonPropertyName("emote_mode")]
	public bool EmoteOnlyMode { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster restricts the chat room to followers only.<br/>
	///Set to true if the broadcaster restricts the chat room to followers only; otherwise, false. The default is true.<br/>
	///To specify how long users must follow the broadcaster before being able to participate in the chat room, see the follower_mode_duration field.
	///</summary>
	[JsonPropertyName("follower_mode")]
	public bool FollowerOnlyMode { get; set; }

	///<summary>
	///The length of time, in minutes, that users must follow the broadcaster before being able to participate in the chat room.<br/>
	///Set only if follower_mode is true. Possible values are: 0 (no restriction) through 129600 (3 months). The default is 0.
	///</summary>
	[JsonPropertyName("follower_mode_duration")]
	public int FollowerModeDurationInMinutes { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster adds a short delay before chat messages appear in the chat room.<br/>
	///This gives chat moderators and bots a chance to remove them before viewers can see the message.<br/>
	///Set to true if the broadcaster applies a delay; otherwise, false. The default is false.<br/>
	///To specify the length of the delay, see the non_moderator_chat_delay_duration field.
	///</summary>
	[JsonPropertyName("non_moderator_chat_delay")]
	public bool NonModeratorChatDelay { get; set; }

	///<summary>
	///The amount of time, in seconds, that messages are delayed before appearing in chat.<br/>
	///Set only if non_moderator_chat_delay is true.
	///</summary>
	[JsonPropertyName("non_moderator_chat_delay_duration")]
	[JsonConverter(typeof(ChatDelayEnumConverter))]
	public ChatDelay NonModeratorChatDelayDuration { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster limits how often users in the chat room are allowed to send messages.<br/>
	///Set to true if the broadcaster applies a wait period between messages; otherwise, false.<br/>
	///The default is false.<br/>
	///To specify the delay, see the slow_mode_wait_time field.
	///</summary>
	[JsonPropertyName("slow_mode")]
	public bool SlowMode { get; set; }

	///<summary>
	///The amount of time, in seconds, that users must wait between sending messages.<br/>
	///Set only if slow_mode is true.<br/>
	///Possible values are: 3 (3 second delay) through 120 (2 minute delay).<br/>
	///The default is 30 seconds.
	///</summary>
	[JsonPropertyName("slow_mode_wait_time")]
	public int SlowModeWaitTime { get; set; }

	///<summary>
	///A Boolean value that determines whether only users that subscribe to the broadcaster’s channel may talk in the chat room.<br/>
	///Set to true if the broadcaster restricts the chat room to subscribers only; otherwise, false.<br/>
	///The default is false.
	///</summary>
	[JsonPropertyName("subscriber_mode")]
	public bool SubscriberMode { get; set; }

	///<summary>
	///A Boolean value that determines whether the broadcaster requires users to post only unique messages in the chat room.<br/>
	///Set to true if the broadcaster allows only unique messages; otherwise, false.<br/>
	///The default is false.
	///</summary>
	[JsonPropertyName("unique_chat_mode")]
	public bool UniqueChatMode { get; set; }
}

// == ⚫ RESPONSE BODY == //


public class UpdateChatSettingsResponse
{
	///<summary>
	///The list of chat settings. The list contains a single object with all the settings.
	///</summary>
	[JsonPropertyName("data")]
	public List<ChatSettingsData> Data { get; set; }
}
