using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ConduitShard;

public class Transport
{
    ///<summary>
    ///websocket or webhook
    ///</summary>
    [JsonPropertyName("method")]
    public string Method { get; set; }

    ///<summary>
    ///Optional. Webhook callback URL. Null if method is set to websocket.
    ///</summary>
    [JsonPropertyName("callback")]
    public string? Callback { get; set; }

    ///<summary>
    ///Optional. WebSocket session ID. Null if method is set to webhook.
    ///</summary>
    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    ///<summary>
    ///Optional. Time that the WebSocket session connected. Null if method is set to webhook.
    ///(Converted to DateTimeOffset)
    ///</summary>
    [JsonPropertyName("connected_at")]
    public DateTimeOffset? ConnectedAt { get; set; }

    ///<summary>
    ///Optional. Time that the WebSocket session disconnected. Null if method is set to webhook. <br/>
    ///(Converted to DateTimeOffset)
    ///</summary>
    [JsonPropertyName("disconnected_at")]
    public DateTimeOffset? DisconnectedAt { get; set; }
}
