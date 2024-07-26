using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;

public class Raid
{
    ///<summary>
    ///The ID of the user being raided.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The login of the user being raided.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user name of the user raided.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///The viewer count.
    ///</summary>
    [JsonPropertyName("viewer_count")]
    public int ViewerCount { get; set; }
}
