using Koishibot.Core.Services.TwitchEventSubNew.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class EventSubRequest
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(SubscriptionTypeEnumConverter))]
    public SubscriptionType Type { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("condition")]
    public Dictionary<string, string> Condition { get; set; } = null!;

    [JsonPropertyName("transport")]
    public EventSubTransportRequest Transport { get; set; } = null!;
}