namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.User;

/// <summary>
/// <see href="">Twitch Documentation</see><br/>
/// When a user receives a whisper. Event Triggers - Anyone whispers the specified user.<br/>
/// Required Scopes: user:read:whispers OR user:manage:whispers
/// </summary>
public class WhisperReceivedEvent
{
    /// <summary>
    /// The ID of the user sending the whisper message.
    /// </summary>
    [JsonPropertyName("from_user_id")]
    public string FromUserId { get; set; }

    /// <summary>
    /// The login of the user sending the  whisper  message.
    /// </summary>
    [JsonPropertyName("from_user_login")]
    public string FromUserLogin { get; set; }

    /// <summary>
    /// The name of the user sending the whisper message.
    /// </summary>
    [JsonPropertyName("from_user_name")]
    public string FromUserName { get; set; }

    /// <summary>
    /// The ID of the user receiving the whisper message.
    /// </summary>
    [JsonPropertyName("to_user_id")]
    public string ToUserId { get; set; }

    /// <summary>
    /// The login of the user receiving the whisper message.
    /// </summary>
    [JsonPropertyName("to_user_login")]
    public string ToUserLogin { get; set; }

    /// <summary>
    /// The name of the user receiving the whisper message.
    /// </summary>
    [JsonPropertyName("to_user_name")]
    public string ToUserName { get; set; }

    /// <summary>
    /// The whisper ID.
    /// </summary>
    [JsonPropertyName("whisper_id")]
    public string WhisperId { get; set; }

    /// <summary>
    /// Object containing whisper information.
    /// </summary>
    [JsonPropertyName("whisper")]
    public Whisper Whisper { get; set; }
}


public class Whisper
{
	/// <summary>
	/// The body of the whisper message.
	/// </summary>
	[JsonPropertyName("text")]
	public string? Text { get; set; }
}