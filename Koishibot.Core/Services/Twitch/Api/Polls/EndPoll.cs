using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ PATCH  == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#end-poll">Twitch Documentation</see><br/>
	/// Ends an active poll. You have the option to end it or end it and archive it.<br/>
	/// Required Scopes: channel:manage:polls<br/>
	/// </summary>
	public async Task EndPoll(EndPollRequestBody requestBody)
	{
		var method = HttpMethod.Patch;
		var url = "polls";
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, body);
	}
}

// == ⚫ REQUEST BODY == //

public class EndPollRequestBody
{
	///<summary>
	///The ID of the broadcaster that’s running the poll.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The ID of the poll to update.
	///</summary>
	[JsonPropertyName("id")]
	public string PollId { get; set; }

	///<summary>
	///The status to set the poll to.<br/>
	///Possible case-sensitive values are:<br/>
	///TERMINATED — Ends the poll before the poll is scheduled to end. The poll remains publicly visible.<br/>
	///ARCHIVED — Ends the poll before the poll is scheduled to end, and then archives it so it's no longer publicly visible.
	///</summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(SetPollStatusEnumConverter))]
	public SetPollStatus Status { get; set; }
}

// == ⚫ RESPONSE BODY == //


public class EndPolLResponse
{
	///<summary>
	/// A list that contains the poll that you ended.
	///</summary>
	[JsonPropertyName("data")]
	public List<PollData> Data { get; set; }
}