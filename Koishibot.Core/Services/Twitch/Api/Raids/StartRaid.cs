using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.TwitchApi.Models;

// == ⚫ POST == //

public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#start-a-raid">Twitch Documentation</see><br/>
	/// Raid another channel by sending the broadcaster’s viewers to the targeted channel.<br/>
	/// Raid occurs after 90 second countdown expires or when Raid Now button is clicked.<br/>
	/// The limit is 10 requests within a 10-minute window.<br/>
	/// Required Scopes: channel:manage:raids<br/>
	/// </summary>
	public async Task StartRaid(StartRaidRequestParameters parameters)
	{
		var method = HttpMethod.Post;
		var url = "raids";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

// == ⚫ REQUEST QUERY PARAMETERS == //

public class StartRaidRequestParameters
{
	///<summary>
	///The ID of the broadcaster that’s sending the raiding party.<br/>
	///This ID must match the user ID in the user access token.
	///</summary>
	[JsonPropertyName("from_broadcaster_id")]
	public string SendingBroadcasterId { get; set; } = null!;

	///<summary>
	///The ID of the broadcaster to raid.
	///</summary>
	[JsonPropertyName("to_broadcaster_id")]
	public string ReceivingBroadcasterId { get; set; } = null!;
}

// == ⚫ RESPONSE BODY == //

public class StartRaidResponse
{
	///<summary>
	///A list that contains a single object with information about the pending raid.
	///</summary>
	[JsonPropertyName("data")]
	public List<RaidData> Data { get; set; }
}

public class RaidData
{
	///<summary>
	///The timestamp of when the raid was requested.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	public string CreatedAt { get; set; }

	///<summary>
	///A Boolean value that indicates whether the channel being raided contains mature content.
	///</summary>
	[JsonPropertyName("is_mature")]
	public bool IsMature { get; set; }
}


