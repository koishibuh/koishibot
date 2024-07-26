using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatMessage;

public class Mention
{
    ///<summary>
    ///The user ID of the mentioned user.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The user name of the mentioned user.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///The user login of the mentioned user.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }
}
