using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#choices">Twitch Documentation</see><br/>
/// An array of the choices for a particular poll. <br/>
/// Each poll’s event payload includes a choices array. <br/>
/// The choices array contains an object that describes each choice and, if applicable, the votes for that choice.
/// </summary>
public class Choice
{
    /// <summary>
    /// ID for the choice.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Text displayed for the choice.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// Not used; will be set to 0.
    /// </summary>
    [JsonPropertyName("bits_votes")]
    public int BitsVotes { get; set; }

    /// <summary>
    /// Number of votes received via Channel Points.
    /// </summary>
    [JsonPropertyName("channel_points_votes")]
    public int ChannelPointsVotes { get; set; }

    /// <summary>
    /// Total number of votes received for the choice across all methods of voting.
    /// </summary>
    [JsonPropertyName("votes")]
    public int Votes { get; set; }
}