using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.UserAuthorization;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#userauthorizationgrant">Twitch Documentation</see><br/>
/// When a user’s authorization has been granted to your client id.<br/>
/// NOTE: This subscription type is only supported by webhooks, and cannot be used with WebSockets.<br/>
/// Required Scopes: Provided client_id must match the client id in the application access token.<br/>
/// </summary>
public class UserAuthorizationGrantedEvent
{
    ///<summary>
    ///The client_id of the application that was granted user access.
    ///</summary>
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    ///<summary>
    ///The user id for the user who has granted authorization for your client id.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    ///<summary>
    ///The user login for the user who has granted authorization for your client id.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string UserLogin { get; set; }

    ///<summary>
    ///The user display name for the user who has granted authorization for your client id.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
}