using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#top-predictors">Twitch Documentation</see><br/>
/// An array of up to 10 objects that describe users who participated in a Channel Points Prediction.<br/>
/// </summary>
public class Predictor
{
    /// <summary>
    ///	The ID of the user.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The login of the user. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the user.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The number of Channel Points won. <br/>
    /// This value is always null in the event payload for Prediction progress and Prediction lock.<br/>
    /// This value is 0 if the outcome did not win or if the Prediction was canceled and Channel Points were refunded.
    /// </summary>
    [JsonPropertyName("channel_points_won")]
    public int ChannelPointsWon { get; set; }

    /// <summary>
    /// The number of Channel Points used to participate in the Prediction.
    /// </summary>
    [JsonPropertyName("channel_points_used")]
    public int ChannelPointsUsed { get; set; }
}