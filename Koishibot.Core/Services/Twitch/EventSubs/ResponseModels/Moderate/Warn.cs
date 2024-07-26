using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;

public class Warn
{
    ///<summary>
    ///The ID of the user being warned.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The login of the user being warned.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user name of the user being warned.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///Optional. Reason given for the warning.
    ///</summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    ///<summary>
    ///Optional. Chat rules cited for the warning.
    ///</summary>
    [JsonPropertyName("chat_rules_cited")]
    public List<string>? ChatRulesCited { get; set; }
}
