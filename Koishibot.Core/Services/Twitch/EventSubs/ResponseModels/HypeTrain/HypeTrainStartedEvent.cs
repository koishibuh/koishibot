using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainbegin">Twitch Documentation</see><br/>
/// When a Hype Train begins on the specified channel.<br/>
/// Required Scopes: channel:read:hype_train
/// </summary>
public class HypeTrainStartedEvent
{
    /// <summary>
    /// The Hype Train ID.
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
    public List<Contributor> TopContributions { get; set; }

    /// <summary>
    /// The most recent contribution.
    /// </summary>
    [JsonPropertyName("last_contribution")]
    public Contributor LastContribution { get; set; }

    /// <summary>
    /// The starting level of the Hype Train.
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }

    /// <summary>
    /// The time when the Hype Train started<br/>
    /// (Converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }


    /// <summary>
    /// The time when the Hype Train expires.<br/>
    /// (Converted to DateTimeOffset)<br/>
    /// The expiration is extended when the Hype Train reaches a new level.
    /// </summary>
    [JsonPropertyName("expires_at")]
    public DateTimeOffset ExpiresAt { get; set; }
}