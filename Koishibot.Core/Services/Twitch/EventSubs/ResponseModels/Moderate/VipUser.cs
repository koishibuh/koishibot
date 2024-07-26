using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;
public class VipUser
{
    ///<summary>
    ///The ID of the user gaining or losing VIP status.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The login of the user gaining or losing VIP status.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user name of the user gaining or losing VIP status.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
}
