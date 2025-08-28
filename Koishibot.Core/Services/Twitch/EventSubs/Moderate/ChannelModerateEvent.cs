using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelmoderate-v2">Twitch Documentation</see><br/>
/// When a moderator performs a moderation action in a channel.<br/>
/// Required Scopes: Check documenation for full list<br/>
/// </summary>
public class ModerateEvent
{
    ///<summary>
    ///The ID of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterId { get; set; }

    ///<summary>
    ///The login of the broadcaster. (Lowercase)
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string BroadcasterLogin { get; set; }

    ///<summary>
    ///The user name of the broadcaster.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string BroadcasterName { get; set; }
    
    ///<summary>
    ///The channel in which the action originally occurred. Is the same as the broadcaster_user_id if not in shared chat.
    ///</summary>
    [JsonPropertyName("source_broadcaster_user_id")]
    public string SourceBroadcasterId { get; set; }

    ///<summary>
    ///The channel in which the action originally occurred. Is the same as the broadcaster_user_login if not in shared chat. (Lowercase)
    ///</summary>
    [JsonPropertyName("source_broadcaster_user_login")]
    public string SourceBroadcasterLogin { get; set; }

    ///<summary>
    ///The channel in which the action originally occurred. Is null when the moderator action happens in the same channel as the broadcaster.<br/>
    ///Is not null when in a shared chat session, and the action happens in the channel of a participant other than the broadcaster.
    ///</summary>
    [JsonPropertyName("source_broadcaster_user_name")]
    public string SourceBroadcasterName { get; set; }

    ///<summary>
    ///The ID of the moderator who performed the action.
    ///</summary>
    [JsonPropertyName("moderator_user_id")]
    public string ModeratorUserId { get; set; }

    ///<summary>
    ///The login of the moderator. (Lowercase)
    ///</summary>
    [JsonPropertyName("moderator_user_login")]
    public string ModeratorUserLogin { get; set; }

    ///<summary>
    ///The user name of the moderator.
    ///</summary>
    [JsonPropertyName("moderator_user_name")]
    public string ModeratorUserName { get; set; }

    ///<summary>
    ///The action performed.
    ///</summary>
    [JsonPropertyName("action")]
    public ModAction Action { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the followers command.
    ///</summary>
    [JsonPropertyName("followers")]
    public FollowersOnlyChat? Followers { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the slow command.
    ///</summary>
    [JsonPropertyName("slow")]
    public SlowChat? Slow { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the vip command.
    ///</summary>
    [JsonPropertyName("vip")]
    public VipUser? Vip { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the vip command.
    ///</summary>
    [JsonPropertyName("unvip")]
    public VipUser? Unvip { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the mod command.
    ///</summary>
    [JsonPropertyName("mod")]
    public ModUser? Mod { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the unmod command.
    ///</summary>
    [JsonPropertyName("unmod")]
    public ModUser? Unmod { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the ban command.
    ///</summary>
    [JsonPropertyName("ban")]
    public BannedUserEvent? Ban { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the unban command.
    ///</summary>
    [JsonPropertyName("unban")]
    public UnbanUser? Unban { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the timeout command.
    ///</summary>
    [JsonPropertyName("timeout")]
    public TimeoutUser? Timeout { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the untimeout command.
    ///</summary>
    [JsonPropertyName("untimeout")]
    public UntimeoutUser? Untimeout { get; set; }
    
    ///<summary>
    ///Optional. Metadata associated with the raid command.
    ///</summary>
    [JsonPropertyName("raid")]
    public Raid? Raid { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the unraid command.
    ///</summary>
    [JsonPropertyName("unraid")]
    public Unraid? Unraid { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the delete command.
    ///</summary>
    [JsonPropertyName("delete")]
    public DeletedMessage? DeletedMessage { get; set; }
    
    ///<summary>
    ///Optional. Metadata associated with the automod terms changes.
    ///</summary>
    [JsonPropertyName("automod_terms")]
    public AutomodTerms? AutomodTerms { get; set; }
    
    ///<summary>
    ///Optional. Metadata associated with an unban request.
    ///</summary>
    [JsonPropertyName("unban_request")]
    public UnbanRequest? UnbanRequest { get; set; }

    ///<summary>
    ///Optional. Metadata associated with the warn command.
    ///</summary>
    [JsonPropertyName("warn")]
    public Warn? Warn { get; set; }
    
    ///<summary>
    ///Optional. Information about the shared_chat_ban event. Is null if action is not shared_chat_ban.<br/>
    ///This field has the same information as the ban field but for a action that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
    ///</summary>
    [JsonPropertyName("shared_chat_ban")]
    public BannedUserEvent? SharedChatBan { get; set; }

    ///<summary>
    ///Optional. Information about the shared_chat_unban event. Is null if action is not shared_chat_unban.<br/>
    ///This field has the same information as the unban field but for a action that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
    ///</summary>
    [JsonPropertyName("shared_chat_unban")]
    public UnbanUser? SharedChatUnban { get; set; }

    ///<summary>
    ///Optional. Information about the shared_chat_timeout event. Is null if action is not shared_chat_timeout.<br/>
    ///This field has the same information as the timeout field but for a action that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
    ///</summary>
    [JsonPropertyName("shared_chat_timeout")]
    public TimeoutUser? SharedChatTimeout { get; set; }

    ///<summary>
    ///Optional. Information about the shared_chat_untimeout event. Is null if action is not shared_chat_untimeout.<br/>
    ///This field has the same information as the untimeout field but for a action that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
    ///</summary>
    [JsonPropertyName("shared_chat_untimeout")]
    public UntimeoutUser? SharedChatUntimeout { get; set; }
    
    ///<summary>
    ///Optional. Information about the shared_chat_delete event. Is null if action is not shared_chat_delete.<br/>
    ///This field has the same information as the delete field but for a action that happened for a channel in a shared chat session other than the broadcaster in the subscription condition.
    ///</summary>
    [JsonPropertyName("shared_chat_delete")]
    public DeletedMessage? SharedChatDeletedMessage { get; set; }

}

