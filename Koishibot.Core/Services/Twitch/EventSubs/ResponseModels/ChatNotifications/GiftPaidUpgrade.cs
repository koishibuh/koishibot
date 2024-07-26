using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class GiftPaidUpgrade
{
    /// <summary>
    /// Whether the gift was given anonymously.
    /// </summary>
    [JsonPropertyName("gifter_is_anonymous")]
    public bool GifterIsAnonymous { get; set; }

    /// <summary>
    /// Optional. The user ID of the user who gifted the subscription. Null if anonymous.
    /// </summary>
    [JsonPropertyName("gifter_user_id")]
    public string? GifterUserId { get; set; }

    /// <summary>
    /// Optional. The user name of the user who gifted the subscription. Null if anonymous.
    /// </summary>
    [JsonPropertyName("gifter_user_name")]
    public string? GifterUserName { get; set; }

    /// <summary>
    /// Optional. The user login of the user who gifted the subscription. Null if anonymous.
    /// </summary>
    [JsonPropertyName("gifter_user_login")]
    public string? GifterUserLogin { get; set; }
}