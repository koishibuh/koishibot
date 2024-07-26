using Koishibot.Core.Services.TwitchEventSubNew.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class Metadata
{
    [JsonPropertyName("message_id")]
    public string MessageId { get; set; } = string.Empty;

    [JsonPropertyName("message_type")]
    [JsonConverter(typeof(MessageTypeEnumConverter))]
    public MessageType MessageType { get; set; }

    [JsonPropertyName("message_timestamp")]
    public DateTime MessageTimestamp { get; set; }

    [JsonPropertyName("subscription_type")]
    [JsonConverter(typeof(SubscriptionTypeEnumConverter))]
    public SubscriptionType SubscriptionType { get; set; }

    [JsonPropertyName("subscription_version")]
    public string SubscriptionVersion { get; set; } = string.Empty;
}

// Welcome: MessageId, MessageType, MessageTimestamp
// KeepAlive: MessageId, MessageType, MessageTimestamp
// ReconnectMessage: MessageId, MessageType, MessageTimestamp
// Notification: MessageId, MessageType, MessageTimestamp, SubscriptionType, SubscriptionVersion
// Revoke: MessageId, MessageType, MessageTimestamp, SubscriptionType, SubscriptionVersion
