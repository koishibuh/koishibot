using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;
public class UnbanRequest
{

    ///<summary>
    ///Whether or not the unban request was approved or denied.
    ///</summary>
    [JsonPropertyName("is_approved")]
    public bool IsApproved { get; set; }

    ///<summary>
    ///The ID of the banned user.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The login of the user.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user name of the user.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///The message included by the moderator explaining their approval or denial.
    ///</summary>
    [JsonPropertyName("moderator_message")]
    public string ModeratorMessage { get; set; }
}
