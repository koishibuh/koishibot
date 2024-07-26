using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcheer">Twitch Documentation</see><br/>
/// When a user cheers on the specified channel.<br/>
/// Required Scopes: bits:read
/// </summary>
public class CheerReceivedEvent
{
    /// <summary>
    /// Whether the user cheered anonymously or not. (No longer in use)
    /// </summary>
    [JsonPropertyName("is_anonymous")]
    public bool IsAnonymous { get; set; }

    /// <summary>
    /// The user ID for the user who cheered on the specified channel.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string CheererUserId { get; set; } = string.Empty;

    /// <summary>
    /// The user login for the user who cheered on the specified channel. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string CheererUserLogin { get; set; } = string.Empty;

    /// <summary>
    /// The user display name for the user who cheered on the specified channel. 
    /// </summary>
    [JsonPropertyName("user_name")]
    public string CheererUserName { get; set; } = string.Empty;

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
    /// The message sent with the cheer.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// The number of bits cheered.
    /// </summary>
    [JsonPropertyName("bits")]
    public int Bits { get; set; }
}
