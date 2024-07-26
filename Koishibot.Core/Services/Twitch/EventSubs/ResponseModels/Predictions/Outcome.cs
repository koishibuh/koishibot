using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Predictions;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#outcomes">Twitch Documentation</see><br/>
/// An array of the outcomes for a particular Channel Points Prediction. <br/>
/// Each Prediction’s event payload includes an outcomes array. <br/>
/// The outcomes array contains an object that describes each outcome and, if applicable, the number of users who selected that outcome, the number of Channel Points for that outcome, and an array of top_predictors.<br/>
/// </summary>
public class Outcome
{
    /// <summary>
    /// The outcome ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }


    /// <summary>
    /// The outcome title.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// The color for the outcome. Valid values are pink and blue.
    /// </summary>
    [JsonPropertyName("color")]
    public string Color { get; set; }

    /// <summary>
    /// The number of users who used Channel Points on this outcome.
    /// </summary>
    [JsonPropertyName("users")]
    public int Users { get; set; }

    /// <summary>
    /// The total number of Channel Points used on this outcome.
    /// </summary>
    [JsonPropertyName("channel_points")]
    public int ChannelPoints { get; set; }

    /// <summary>
    /// An array of users who used the most Channel Points on this outcome.
    /// </summary>
    [JsonPropertyName("top_predictors")]
    public List<Predictor> TopPredictors { get; set; }
}