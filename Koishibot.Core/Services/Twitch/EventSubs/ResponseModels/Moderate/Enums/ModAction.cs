using System.Text.Json;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate.Enums;

public enum ModAction
{
    Ban = 1,
    Timeout,
    Unban,
    Untimeout,
    ClearChat,
    EmoteOnlyChat,
    EmoteOnlyChatOff,
    FollowersOnlyChat,
    FollowersOnlyChatOff,
    UniqueChat,
    UniqueChatOff,
    SlowChat,
    SlowChatOff,
    SubscribersOnlyChat,
    SubscribersOnlyChatOff,
    CancelRaid,
    DeleteChatMessage,
    UnVip,
    Vip,
    Raid,
    AddBlockedTerm,
    AddPermittedTerm,
    RemoveBlockedTerm,
    RemovePermittedTerm,
    AddMod,
    Unmod,
    ApproveUnbanRequest,
    DenyUnbanRequest,
    WarnUser
}


// == ⚫ == //

public class ModActionEnumConverter : JsonConverter<ModAction>
{
    public override ModAction Read
        (ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "ban" => ModAction.Ban,
            "timeout" => ModAction.Timeout,
            "unban" => ModAction.Unban,
            "untimeout" => ModAction.Untimeout,
            "clear" => ModAction.ClearChat,
            "emoteonly" => ModAction.EmoteOnlyChat,
            "emoteonlyoff" => ModAction.EmoteOnlyChatOff,
            "followers" => ModAction.FollowersOnlyChat,
            "followersoff" => ModAction.FollowersOnlyChatOff,
            "uniquechat" => ModAction.UniqueChat,
            "uniquechatoff" => ModAction.UniqueChatOff,
            "slow" => ModAction.SlowChat,
            "slowoff" => ModAction.SlowChatOff,
            "subscribers" => ModAction.SubscribersOnlyChat,
            "subscribersoff" => ModAction.SubscribersOnlyChatOff,
            "unraid" => ModAction.CancelRaid,
            "delete" => ModAction.DeleteChatMessage,
            "unvip" => ModAction.UnVip,
            "vip" => ModAction.Vip,
            "raid" => ModAction.Raid,
            "add_blocked_term" => ModAction.AddBlockedTerm,
            "add_permitted_term" => ModAction.AddPermittedTerm,
            "remove_blocked_term" => ModAction.RemoveBlockedTerm,
            "remove_permitted_term" => ModAction.RemovePermittedTerm,
            "mod" => ModAction.AddMod,
            "unmod" => ModAction.Unmod,
            "approve_unban_request" => ModAction.ApproveUnbanRequest,
            "deny_unban_request" => ModAction.DenyUnbanRequest,
            "warn" => ModAction.WarnUser,
            _ => throw new JsonException()
        };
    }

    public override void Write
        (Utf8JsonWriter writer, ModAction value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}