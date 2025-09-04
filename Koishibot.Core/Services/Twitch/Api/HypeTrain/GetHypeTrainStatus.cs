using Koishibot.Core.Services.Twitch;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.TwitchApi.Models;

/*════════════════【 API REQUEST 】════════════════*/
public partial record TwitchApiRequest : ITwitchApiRequest
{
	/// <summary>
	/// <see href="https://dev.twitch.tv/docs/api/reference/#get-hype-train-status">Twitch Documentation</see><br/>
	/// Get the status of a Hype Train for the specified broadcaster.<br/>
	/// Required Scopes: channel:read:hype_train<br/>
	/// Requires that broadcaster_id and user_id match in the User-Access token.<br/>
	/// </summary>
	public async Task GetHypeTrainStatus(GetHypeTrainStatusParameters parameters)
	{
		var method = HttpMethod.Get;
		const string url = "hypetrain/status";
		var query = parameters.ObjectQueryFormatter();

		var response = await TwitchApiClient.SendRequest(method, url, query);
	}
}

/*═════════════【 REQUEST PARAMETERS 】═════════════*/
public class GetHypeTrainStatusParameters
{

	///<summary>
	///The User ID of the channel broadcaster.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }
}

/*══════════════════【 RESPONSE 】══════════════════*/
public class GetHypeTrainStatusResponse
{
	///<summary>
	///A list that contains information related to the channel’s Hype Train.
	///</summary>
	[JsonPropertyName("data")]
	public List<HypeTrainStatusData> Data { get; set; }
}


public class HypeTrainStatusData
{
	///<summary>
	///An object describing the current Hype Train. Null if a Hype Train is not active.
	///</summary>
	[JsonPropertyName("id")]
	public CurrentHypeTrainData? CurrentHypeTrainData { get; set; }
}

public class CurrentHypeTrainData
{
	///<summary>
	///The Hype Train ID.
	///</summary>
	[JsonPropertyName("id")]
	public string HypeTrainEventId { get; set; }
	
	///<summary>
	///The ID of the broadcaster that’s running the Hype Train.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; }
	
	///<summary>
	///The login of the broadcaster that’s running the Hype Train.
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; }
	
	///<summary>
	///The username of the broadcaster that’s running the Hype Train.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterUsername { get; set; }
	
	///<summary>
	///The current level of the Hype Train.
	///</summary>
	[JsonPropertyName("level")]
	public int HypeTrainLevel { get; set; }
	
	///<summary>
	///Total points contributed to the Hype Train.
	///</summary>
	[JsonPropertyName("total")]
	public int TotalPoints { get; set; }
	
	///<summary>
	///The number of points contributed to the Hype Train at the current level.
	///</summary>
	[JsonPropertyName("progress")]
	public int Progress { get; set; }
	
	///<summary>
	///The number of points required to reach the next level.
	///</summary>
	[JsonPropertyName("goal")]
	public int NextLevelGoal { get; set; }
	
	///<summary>
	///The contributors with the most points contributed.
	///</summary>
	[JsonPropertyName("top_contributions")]
	public List<TopHypeTrainContributor> TopHypeTrainContributors { get; set; }
	
	///<summary>
	///A list containing the broadcasters participating in the shared Hype Train.<br/>
	///Null if the Hype Train is not shared.
	///</summary>
	[JsonPropertyName("shared_train_participants")]
	public List<SharedTrainParticipant> SharedTrainParticipants { get; set; }
	
	///<summary>
	///The timestamp when this Hype Train started.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset StartedAt { get; set; }
	
	///<summary>
	///The time when the Hype Train expires.<br/>
	///The expiration is extended when the Hype Train reaches a new level.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("expires_at")]
	public DateTimeOffset ExpiresAt { get; set; }
	
	///<summary>
	///The number of points required to reach the next level.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(HypeTrainTypeConverter))]
	public string? HypeTrainType { get; set; }
	
	///<summary>
	///Indicates if the Hype Train is shared. <br/>
	///When true, shared_train_participants will contain the list of broadcasters the train is shared with.
	///</summary>
	[JsonPropertyName("is_shared_train")]
	public bool IsSharedTrain { get; set; }

	///<summary>
	///An object with information about the channel’s Hype Train records.<br/>
	///Null if a Hype Train has not occurred.
	///</summary>
	public HypeTrainRecord? AllTimeHighRecord { get; set; }
	
	///<summary>
	///An object with information about the channel’s shared Hype Train records.<br/>
	///Null if a Hype Train has not occurred.
	///</summary>
	public HypeTrainRecord? SharedAllTimeHighRecord { get; set; }
	
}

public class TopHypeTrainContributor
{
	///<summary>
	///The ID of the user that made the contribution.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The user’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; }
	
	///<summary>
	///The user’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string Username { get; set; }
	
	///<summary>
	///The contribution method used.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(ContributionApiTypeEnumConverter))]
	public ContributionApiType Type { get; set; }
	
	///<summary>
	///The total number of points contributed for the type.
	///</summary>
	[JsonPropertyName("total")]
	public int TotalPoints { get; set; }

}


public class HypeTrainRecord
{
	///<summary>
	///The level of the record Hype Train.
	///</summary>
	[JsonPropertyName("level")]
	public int Level { get; set; }
	
	///<summary>
	///Total points contributed to the record Hype Train.
	///</summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }
	
	///<summary>
	///The time when the record was achieved.
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("achieved_at")]
	public DateTimeOffset AchievedAt { get; set; }
}