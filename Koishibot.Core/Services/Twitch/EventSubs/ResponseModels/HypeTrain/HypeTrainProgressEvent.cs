using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainprogress">Twitch Documentation</see><br/>
/// When a Hype Train makes progress on the specified channel. <br/>
/// When a Hype Train starts, one channel.hype_train.progress event will be sent for each contribution that caused the Hype Train to begin (in addition to the channel.hype_train.begin event).<br/>
/// After a Hype Train begins, any additional cheers or subscriptions on the channel will cause channel.hype_train.progress notifications to be sent.<br/>
/// Required Scopes: channel:read:hype_train
/// </summary>
public class HypeTrainProgressEvent
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
    public string BroadcasterUserId { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user login. (Lowercase)
    /// </summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The broadcaster’s user display name.
    /// </summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; } = string.Empty;

    /// <summary>
    /// The current level of the Hype Train.
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }

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
    /// The time when the Hype Train started.<br/>
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