using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public record EventSubTransportRequest
{
    [JsonPropertyName("method")]
    public string Method => "websocket";

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }
};