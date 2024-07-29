using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.User;

/// <summary>
/// <see href="">Twitch Documentation</see><br/>
/// When user updates their account.<br/>
/// Required Scopes: No authorization required. If you have the user:read:email scope, the notification will include email field.
/// </summary> 
public class UserUpdatedEvent
{
    /// <summary>
    /// The user’s user id.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    /// <summary>
    /// The user’s user login. (Lowercase)
    /// </summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    /// <summary>
    /// The user’s user display name.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    /// <summary>
    /// The user’s email address.<br/>
    /// The event includes the user’s email address only if the app used to request this event type includes the user:read:email scope for the user; otherwise, the field is set to an empty string.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// A Boolean value that determines whether Twitch has verified the user’s email address. Is true if Twitch has verified the email address; otherwise, false.
    /// NOTE: Ignore this field if the email field contains an empty string.
    /// </summary>
    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }

    /// <summary>
    /// The user’s description.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }
}
