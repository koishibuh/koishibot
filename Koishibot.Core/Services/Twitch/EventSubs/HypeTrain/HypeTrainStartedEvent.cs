using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.TwitchApi.Models;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainbegin-v2">Twitch Documentation</see><br/>
/// When a Hype Train begins on the specified channel.<br/>
/// Required Scopes: channel:read:hype_train<br/><br/>
/// </summary>
public class HypeTrainStartedEvent
{
	/// <summary>
	/// The Hype Train ID.
	/// </summary>
	[JsonPropertyName("id")]
	public string? Id { get; set; }

	/// <summary>
	///	The broadcaster’s user ID.
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user login. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user display name.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterUsername { get; set; } = string.Empty;

	/// <summary>
	/// Total points contributed to the Hype Train.
	/// </summary>
	[JsonPropertyName("total")]
	public int Total { get; set; }

	/// <summary>
	/// The number of points contributed to the Hype Train at the current level.
	/// </summary>
	[JsonPropertyName("progress")]
	public int Progress { get; set; }

	/// <summary>
	/// The number of points required to reach the next level.
	/// </summary>
	[JsonPropertyName("goal")]
	public int Goal { get; set; }
	
	/// <summary>
	/// The contributors with the most points contributed.
	/// </summary>
	[JsonPropertyName("top_contributions")]
	public List<Contributor>? TopContributions { get; set; }
	
	/// <summary>
	/// The starting level of the Hype Train.
	/// </summary>
	[JsonPropertyName("level")]
	public int Level { get; set; }
	
	/// <summary>
	/// The all-time high level this type of Hype Train has reached for this broadcaster.
	/// </summary>
	[JsonPropertyName("all_time_high_level")]
	public int AllTimeHighLevel { get; set; }	
	
	/// <summary>
	/// The all-time high total this type of Hype Train has reached for this broadcaster.
	/// </summary>
	[JsonPropertyName("all_time_high_total")]
	public int AllTimeHighTotal { get; set; }

	///<summary>
	///A list containing the broadcasters participating in the shared Hype Train.<br/>
	///Null if the Hype Train is not shared.
	///</summary>
	[JsonPropertyName("shared_train_participants")]
	public List<SharedTrainParticipant>? SharedTrainParticipants { get; set; }

	/// <summary>
	/// The time when the Hype Train started<br/>
	/// (Converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("started_at")]
	// [JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset StartedAt { get; set; }

	/// <summary>
	/// The time when the Hype Train expires.<br/>
	/// (Converted to DateTimeOffset)<br/>
	/// The expiration is extended when the Hype Train reaches a new level.
	/// </summary>
	[JsonPropertyName("expires_at")]
	// [JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset ExpiresAt { get; set; }

	///<summary>
	/// The number of points required to reach the next level.
	///</summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(HypeTrainTypeConverter))]
	public string? HypeTrainType { get; set; }
	
	///<summary>
	/// Indicates if the Hype Train is shared.<br/>
	/// When true, shared_train_participants will contain the list of broadcasters the train is shared with.
	///</summary>
	[JsonPropertyName("is_shared_train")]
	public bool IsSharedTrain { get; set; }
}