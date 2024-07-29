using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch.Common;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;


/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpredictionprogress">Twitch Documentation</see><br/>
/// When users participate in a Prediction on the specified channel.<br/>
/// Required Scopes: channel:read:predictions OR channel:manage:predictions
/// </summary>
public class PredictionProgressEvent
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
    public string BroadcasterName { get; set; } = string.Empty;

    /// <summary>
    /// Title for the Channel Points Prediction.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// An array of outcomes for the Channel Points Prediction.
    /// </summary>
    [JsonPropertyName("outcomes")]
    public List<Outcome> Outcomes { get; set; }

    /// <summary>
    /// The time the Channel Points Prediction started.<br/>
    /// (Converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }


    /// <summary>
    /// The time the Channel Points Prediction will automatically lock.<br/>
    /// (Converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("locks_at")]
    public DateTimeOffset LocksAt { get; set; }
}