using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatSettings;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelchat_settingsupdate">Twitch Documentation</see><br/>
/// When a broadcaster’s chat settings are updated.<br/>
/// Required Scopes: user:read:chat<br/>
/// </summary>
public class ChatSettingsUpdatedEvent
{
    ///<summary>
    ///The ID of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The login of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster specified in the request.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///A Boolean value that determines whether chat messages must contain only emotes. True if only messages that are 100% emotes are allowed; otherwise false.
    ///</summary>
    [JsonPropertyName("emote_mode")]
    public bool EmoteOnlyMode { get; set; }

    ///<summary>
    ///A Boolean value that determines whether the broadcaster restricts the chat room to followers only, based on how long they’ve followed.
    /// True if the broadcaster restricts the chat room to followers only; otherwise false.
    /// See follower_mode_duration_minutes for how long the followers must have followed the broadcaster to participate in the chat room.
    ///</summary>
    [JsonPropertyName("follower_mode")]
    public bool FollowerOnlyMode { get; set; }

    ///<summary>
    ///The length of time, in minutes, that the followers must have followed the broadcaster to participate in the chat room. See follower_mode.
    /// Null if follower_mode is false.
    ///</summary>
    [JsonPropertyName("follower_mode_duration_minutes")]
    public int FollowerModeDurationMinutes { get; set; }

    ///<summary>
    ///A Boolean value that determines whether the broadcaster limits how often users in the chat room are allowed to send messages.
    /// Is true, if the broadcaster applies a delay; otherwise, false.
    // See slow_mode_wait_time_seconds for the delay.
    ///</summary>
    [JsonPropertyName("slow_mode")]
    public bool SlowChatMode { get; set; }

    ///<summary>
    ///The amount of time, in seconds, that users need to wait between sending messages. See slow_mode.
    ///Null if slow_mode is false.
    ///</summary>
    [JsonPropertyName("slow_mode_wait_time_seconds")]
    public int SlowModeWaitTimeSeconds { get; set; }

    ///<summary>
    ///A Boolean value that determines whether only users that subscribe to the broadcaster’s channel can talk in the chat room.
    //True if the broadcaster restricts the chat room to subscribers only; otherwise false.
    ///</summary>
    [JsonPropertyName("subscriber_mode")]
    public bool SubscriberOnlyMode { get; set; }

    ///<summary>
    ///A Boolean value that determines whether the broadcaster requires users to post only unique messages in the chat room.
    //True if the broadcaster requires unique messages only; otherwise false.
    ///</summary>
    [JsonPropertyName("unique_chat_mode")]
    public bool UniqueChatMode { get; set; }
}