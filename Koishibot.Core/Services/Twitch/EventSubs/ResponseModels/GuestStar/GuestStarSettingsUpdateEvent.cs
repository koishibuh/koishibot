using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.GuestStar.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.GuestStar;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelguest_star_settingsupdate">Twitch Documentation</see><br/>
/// When the host preferences for Guest Star have been updated.<br/>
/// Required Scopes: channel:read:guest_star, channel:manage:guest_star, moderator:read:guest_star or moderator:manage:guest_star<br/>
/// </summary>
public class GuestStarSettingsUpdateEvent
{
    ///<summary>
    ///User ID of the host channel.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterUserId { get; set; }

    ///<summary>
    ///The broadcaster display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string BroadcasterUserName { get; set; }

    ///<summary>
    ///The broadcaster login.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterUserLogin { get; set; }

    ///<summary>
    ///Flag determining if Guest Star moderators have access to control whether a guest is live once assigned to a slot.
    ///</summary>
    [JsonPropertyName("is_moderator_send_live_enabled")]
    public bool IsModeratorSendLiveEnabled { get; set; }

    ///<summary>
    ///Number of slots the Guest Star call interface will allow the host to add to a call.
    ///</summary>
    [JsonPropertyName("slot_count")]
    public int SlotCount { get; set; }

    ///<summary>
    ///Flag determining if browser sources subscribed to sessions on this channel should output audio
    ///</summary>
    [JsonPropertyName("is_browser_source_audio_enabled")]
    public bool IsBrowserSourceAudioEnabled { get; set; }

    ///<summary>
    ///This setting determines how the guests within a session should be laid out within a group browser source.
    ///</summary>
    [JsonPropertyName("group_layout")]
    public GroupLayout GroupLayout { get; set; }
}
