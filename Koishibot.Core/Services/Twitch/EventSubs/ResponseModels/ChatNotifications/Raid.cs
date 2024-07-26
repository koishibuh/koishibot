using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;
public class Raid
{
    /// <summary>
    /// The user ID of the broadcaster raiding this channel.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    /// <summary>
    /// The user name of the broadcaster raiding this channel.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    /// <summary>
    /// The login name of the broadcaster raiding this channel.
    /// </summary> 
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    /// <summary>
    /// The number of viewers raiding this channel from the broadcaster’s channel.
    /// </summary>
    [JsonPropertyName("viewer_count")]
    public int ViewerCount { get; set; }

    /// <summary>
    /// Profile image URL of the broadcaster raiding this channel.
    /// </summary>
    [JsonPropertyName("profile_image_url")]
    public string ProfileImageUrl { get; set; }
}