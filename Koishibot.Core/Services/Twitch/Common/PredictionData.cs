using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.Common;
public class PredictionData
{
	///<summary>
	///An ID that identifies this prediction.
	///</summary>
	[JsonPropertyName("id")]
	public string PredictionId { get; set; }

	///<summary>
	///An ID that identifies the broadcaster that created the prediction.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The broadcaster’s display name.
	///</summary>
	[JsonPropertyName("broadcaster_name")]
	public string BroadcasterName { get; set; }

	///<summary>
	///The broadcaster’s login name.
	///</summary>
	[JsonPropertyName("broadcaster_login")]
	public string BroadcasterLogin { get; set; }

	///<summary>
	///The question that the prediction asks. For example, Will I finish this entire pizza?
	///</summary>
	[JsonPropertyName("title")]
	public string PredictionTitle { get; set; }

	///<summary>
	///The ID of the winning outcome. Is null unless status is RESOLVED.
	///</summary>
	[JsonPropertyName("winning_outcome_id")]
	public string? WinningOutcomeId { get; set; }

	///<summary>
	///The list of possible outcomes for the prediction.
	///</summary>
	[JsonPropertyName("outcomes")]
	public List<Outcome> Outcomes { get; set; }

	///<summary>
	///The length of time (in seconds) that the prediction will run for.
	///</summary>
	[JsonPropertyName("prediction_window")]
	public int PredictionWindowInSeconds { get; set; }

	///<summary>
	///The prediction’s status.
	///</summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(PredictionApiStatusEnumConverter))]
	public PredictionApiStatus Status { get; set; }

	///<summary>
	///The timestamp of when the Prediction began.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	public DateTimeOffset CreatedAt { get; set; }

	///<summary>
	///The timestamp of when the Prediction ended.<br/>
	///If status is ACTIVE, this is set to null.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("ended_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? EndedAt { get; set; }

	///<summary>
	///The timestamp of when the Prediction was locked.<br/>
	///If status is not LOCKED, this is set to null.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("locked_at")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset? LockedAt { get; set; }
}

public class Outcome
{
	///<summary>
	///An ID that identifies this outcome.
	///</summary>
	[JsonPropertyName("id")]
	public string OutcomeId { get; set; }

	///<summary>
	///The outcome’s text.
	///</summary>
	[JsonPropertyName("title")]
	public string OutcomeTitle { get; set; }

	///<summary>
	///The number of unique viewers that chose this outcome.
	///</summary>
	[JsonPropertyName("users")]
	public int UserCount { get; set; }

	///<summary>
	///The number of Channel Points spent by viewers on this outcome.
	///</summary>
	[JsonPropertyName("channel_points")]
	public int ChannelPointTotalSpent { get; set; }

	///<summary>
	///A list of viewers who were the top predictors; otherwise, null if none.
	///</summary>
	[JsonPropertyName("top_predictors")]
	public List<TopPredictor> TopPredictors { get; set; }

	///<summary>
	///The color that visually identifies this outcome in the UX.<br/>
	///If the number of outcomes is two, the color is BLUE for the first outcome and PINK for the second outcome.<br/>
	///If there are more than two outcomes, the color is BLUE for all outcomes.
	///</summary>
	[JsonPropertyName("color")]
	[JsonConverter(typeof(PredictionColorEnumConverter))]
	public PredictionColor? Color { get; set; }
}

public class TopPredictor
{
	///<summary>
	///An ID that identifies the viewer.
	///</summary>
	[JsonPropertyName("user_id")]
	public string UserId { get; set; }

	///<summary>
	///The viewer’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string UserName { get; set; }

	///<summary>
	///The viewer’s login name.
	///</summary>
	[JsonPropertyName("user_login")]
	public string UserLogin { get; set; }

	///<summary>
	///The number of Channel Points the viewer spent.
	///</summary>
	[JsonPropertyName("channel_points_used")]
	public int ChannelPointsUsed { get; set; }

	///<summary>
	///The number of Channel Points distributed to the viewer.
	///</summary>
	[JsonPropertyName("channel_points_won")]
	public int? ChannelPointsWon { get; set; }
}