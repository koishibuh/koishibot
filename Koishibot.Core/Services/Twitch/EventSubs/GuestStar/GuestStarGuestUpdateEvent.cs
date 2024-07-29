using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.GuestStar;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_guestupdate">Twitch Documentation</see><br/>
/// When a guest or a slot is updated in an active Guest Star session.<br/>
/// Required Scopes: channel:read:guest_star, channel:manage:guest_star, moderator:read:guest_star or moderator:manage:guest_star<br/>
/// </summary>
public class GuestStarGuestUpdateEvent
{
    ///<summary>
    ///The non-host broadcaster user ID.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string? BroadcasterId { get; set; }

    ///<summary>
    ///The non-host broadcaster display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string? BroadcasterName { get; set; }

    ///<summary>
    ///The non-host broadcaster login.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string? BroadcasterLogin { get; set; }

    ///<summary>
    ///ID representing the unique session that was started.
    ///</summary>
    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    ///<summary>
    ///The user ID of the moderator who updated the guest's state (could be the host). null if the update was performed by the guest.
    ///</summary>
    [JsonPropertyName("moderator_user_id")]
    public string? ModeratorUserId { get; set; }

    ///<summary>
    ///The moderator display name. null if the update was performed by the guest.
    ///</summary>
    [JsonPropertyName("moderator_user_name")]
    public string? ModeratorUserName { get; set; }

    ///<summary>
    ///The moderator login. null if the update was performed by the guest.
    ///</summary>
    [JsonPropertyName("moderator_user_login")]
    public string? ModeratorUserLogin { get; set; }

    ///<summary>
    ///The user ID of the guest who transitioned states in the session. null if the slot is now empty.
    ///</summary>
    [JsonPropertyName("guest_user_id")]
    public string? GuestUserId { get; set; }

    ///<summary>
    ///The guest display name. null if the slot is now empty.
    ///</summary>
    [JsonPropertyName("guest_user_name")]
    public string? GuestUserName { get; set; }

    ///<summary>
    ///The guest login. null if the slot is now empty.
    ///</summary>
    [JsonPropertyName("guest_user_login")]
    public string? GuestUserLogin { get; set; }

    ///<summary>
    ///The ID of the slot assignment the guest is assigned to. null if the guest is in the INVITED, REMOVED, READY, or ACCEPTED state.
    ///</summary>
    [JsonPropertyName("slot_id")]
    public string? SlotId { get; set; }

    ///<summary>
    ///The current state of the user after the update has taken place. null if the slot is now empty. 
    ///</summary>
    [JsonPropertyName("state")]
    public GuestState State { get; set; }

    ///<summary>
    ///User ID of the host channel.
    ///</summary>
    [JsonPropertyName("host_user_id")]
    public string? HostUserId { get; set; }

    ///<summary>
    ///The host display name.
    ///</summary>
    [JsonPropertyName("host_user_name")]
    public string? HostUserName { get; set; }

    ///<summary>
    ///The host login.
    ///</summary>
    [JsonPropertyName("host_user_login")]
    public string? HostUserLogin { get; set; }

    ///<summary>
    ///Flag that signals whether the host is allowing the slot's video to be seen by participants within the session. null if the guest is not slotted.
    ///</summary>
    [JsonPropertyName("host_video_enabled")]
    public bool HostVideoEnabled { get; set; }

    ///<summary>
    ///Flag that signals whether the host is allowing the slot's audio to be heard by participants within the session. null if the guest is not slotted.
    ///</summary>
    [JsonPropertyName("host_audio_enabled")]
    public bool HostAudioEnabled { get; set; }

    ///<summary>
    ///Value between 0-100 that represents the slot's audio level as heard by participants within the session. null if the guest is not slotted.
    ///</summary>
    [JsonPropertyName("host_volume")]
    public int HostVolume { get; set; }
}
