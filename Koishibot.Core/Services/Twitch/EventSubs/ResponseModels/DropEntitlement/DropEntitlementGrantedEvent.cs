using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DropEntitlement;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#dropentitlementgrant">Twitch Documentation</see><br/>
/// When an entitlement for a Drop is granted to a user.<br/>
/// NOTE This subscription type is only supported by webhooks, and cannot be used with WebSockets. <br/>
/// Required Scopes: App access token required. The client ID associated with the access token must be owned by a user who is part of the specified organization.<br/>
/// </summary>
public class UserAuthorizationGrantedEvent
{
    ///<summary>
    ///Individual event ID, as assigned by EventSub. Use this for de-duplicating messages.
    ///</summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    ///<summary>
    ///Entitlement object.
    ///</summary>
    [JsonPropertyName("data")]
    public EntitlementData Data { get; set; }
}