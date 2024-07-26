using Koishibot.Core.Services.TwitchEventSubNew.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.RequestModels;

public class Subscription
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(SubscriptionTypeEnumConverter))]
    public SubscriptionType Type { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(SubscriptionStatusTypeEnumConverter))]
    public SubscriptionStatusType Status { get; set; }

    [JsonPropertyName("cost")]
    public byte Cost { get; set; }

    [JsonPropertyName("condition")]
    public Condition? Condition { get; set; }

    [JsonPropertyName("transport")]
    public Transport? Transport { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}