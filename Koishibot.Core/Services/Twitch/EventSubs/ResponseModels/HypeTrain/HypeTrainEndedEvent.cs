using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.HypeTrain;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelhype_trainend">Twitch Documentation</see><br/>
/// When a Hype Train ends on the specified channel. <br/>
/// Required Scopes: channel:read:hype_train
/// </summary>
public class HypeTrainEndedEvent
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
    /// The final level of the Hype Train.
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }

    /// <summary>
    /// Total points contributed to the Hype Train.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    /// The contributors with the most points contributed.
    /// </summary>
    [JsonPropertyName("top_contributions")]
    public List<Contributor> TopContributions { get; set; }

    /// <summary>
    /// The time when the Hype Train started<br/>
    /// (Converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset StartedAt { get; set; }


    /// <summary>
    /// The time when the Hype Train expires<br/>
    /// (Converted to DateTimeOffset)
    /// The expiration is extended when the Hype Train reaches a new level.
    /// </summary>
    [JsonPropertyName("started_at")]
    public DateTimeOffset ExpiresAt { get; set; }


    /// <summary>
    /// The time when the Hype Train cooldown ends so that the next Hype Train can start.<br/>
    /// (Converted to DateTimeOffset)
    /// </summary>
    [JsonPropertyName("cooldown_ends_at")]
    public DateTimeOffset CooldownEndsAt { get; set; }
}