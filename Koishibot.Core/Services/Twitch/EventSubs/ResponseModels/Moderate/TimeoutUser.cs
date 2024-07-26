using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Moderate;
public class TimeoutUser
{
    ///<summary>
    ///The ID of the user being timed out.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The login of the user being timed out.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user name of the user being timed out.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    ///<summary>
    ///Optional. The reason given for the timeout.
    ///</summary>
    [JsonPropertyName("reason")]
    public string Reason { get; set; }

    ///<summary>
    ///The time at which the timeout ends.</br>
    ///(Converted to DateTimeOffset)
    ///</summary>
    [JsonPropertyName("expires_at")]
    public DateTimeOffset ExpiresAt { get; set; }
}
