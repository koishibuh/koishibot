using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;


/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionend">Twitch Documentation</see><br/>
/// When a Prediction ends on the specified channel.<br/>
/// Required Scopes: channel:read:predictions OR channel:manage:predictions
/// </summary>
public class PredictionEndedEvent
{
    /// <summary>
    /// Channel Points Prediction ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

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
    /// Title for the Channel Points Prediction.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// ID of the winning outcome.
    /// </summary>
    [JsonPropertyName("winning_outcome_id")]
    public string WinningOutcomeId { get; set; }

    /// <summary>
    /// An array of outcomes for the Channel Points Prediction.
    /// </summary>
    [JsonPropertyName("outcomes")]
    public List<Outcome> Outcomes { get; set; }

    /// <summary>
    /// The status of the Channel Points Prediction.
    /// </summary>
    [JsonPropertyName("status")]
    public PredictionEventSubStatus Status { get; set; }

    /// <summary>
    /// The time the Channel Points Prediction started.<br/>
    /// (RFC3339 format converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }


    /// <summary>
    /// TThe time the Channel Points Prediction ended - converted to DateTimeOffset.
    /// </summary>
    [JsonPropertyName("ended_at")]
    public DateTimeOffset EndedAt { get; set; }
}