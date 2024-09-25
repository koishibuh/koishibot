using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.TwitchApi.Models;

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#ban-user">Twitch Documentation</see><br/>
	/// Bans a user from participating in the specified broadcaster’s chat room or puts them in a timeout.<br/>
	/// If the user is currently in a timeout, you can call this endpoint to change the duration of the timeout or ban them altogether.<br/>
	/// If the user is currently banned, you cannot call this method to put them in a timeout instead.
	/// Required Scopes: moderator:manage:banned_users<br/>
	/// </summary>
	public async Task BanUser(BanUserRequestParameters parameters, BanUserRequestBody requestBody)
	{
		var method = HttpMethod.Post;
		const string url = "moderation/bans";
		var query = parameters.ObjectQueryFormatter();
		var body = TwitchApiHelper.ConvertToStringContent(requestBody);

		var response = await TwitchApiClient.SendRequest(method, url, query, body);
	}
}

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
public class BanUserRequestParameters
{
	///<summary>
	///The ID of the broadcaster whose chat room the user is being banned from.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; } = null!;

	///<summary>
	///The ID of the broadcaster or a user that has permission to moderate the broadcaster’s chat room.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; } = null!;
}

/*════════════════【 REQUEST BODY 】════════════════*/
public class BanUserRequestBody
{
	///<summary>
	///Identifies the user and type of ban.
	///</summary>
	[JsonPropertyName("data")]
	public BanRequestData? BanRequestData { get; set; }
}

public class BanRequestData
{
	///<summary>
	///The ID of the user to ban or put in a timeout.
	///</summary>
	[JsonPropertyName("user_id")]
	public string? UserId { get; set; }

	///<summary>
	///To ban a user indefinitely, don’t include this field.<br/>
	///To put a user in a timeout, include this field and specify the timeout period, in seconds.<br/>
	///The minimum timeout is 1 second and the maximum is 1,209,600 seconds(2 weeks).<br/>
	///To end a user’s timeout early, set this field to 1, or use the Unban user endpoint.
	///</summary>
	[JsonPropertyName("duration")]
	public int TimeoutDurationInSeconds { get; set; }

	///<summary>
	///The reason the you’re banning the user or putting them in a timeout. <br/>
	///The text is user defined and is limited to a maximum of 500 characters.
	///</summary>
	[JsonPropertyName("reason")]
	public string? Reason { get; set; }
}


/*══════════════════【 RESPONSE 】══════════════════*/
public class BanUserResponse
{
	///<summary>
	///A list that contains the user you successfully banned or put in a timeout.
	///</summary>
	[JsonPropertyName("data")]
	public List<BannedUserData>? Data { get; set; }

}

public class BannedUserData
{
	///<summary>
	///The broadcaster whose chat room the user was banned from chatting in.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string? BroadcasterId { get; set; }

	///<summary>
	///The moderator that banned or put the user in the timeout.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string? ModeratorId { get; set; }

	///<summary>
	///The user that was banned or put in a timeout.
	///</summary>
	[JsonPropertyName("user_id")]
	public string? DisciplinedUser { get; set; }

	///<summary>
	///The timestamp when the ban or timeout was placed.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	public DateTimeOffset CreatedAt { get; set; }

	///<summary>
	///The timestamp when the timeout will end.<br/>
	///Is null if the user was banned instead of being put in a timeout.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("end_time")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? EndingAt { get; set; }
}