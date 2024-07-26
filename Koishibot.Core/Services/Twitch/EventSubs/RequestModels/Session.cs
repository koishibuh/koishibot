using Koishibot.Core.Services.TwitchEventSubNew.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class Session
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(SessionStatusTypeEnumConverter))]
    public SessionStatusType SessionStatus { get; set; }

    [JsonPropertyName("connected_at")]
    public DateTime ConnectedAt { get; set; }

    [JsonPropertyName("keepalive_timeout_seconds")]
    public int KeepAliveTimeoutSeconds { get; set; }

    [JsonPropertyName("reconnect_url")]
    public string? ReconnectUrl { get; set; }
}