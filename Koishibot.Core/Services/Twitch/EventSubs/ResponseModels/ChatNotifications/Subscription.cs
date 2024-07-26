using Koishibot.Core.Services.Twitch.Enums;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class Subscription
{
    /// <summary>
    /// The type of subscription plan being used.
    /// </summary>
    [JsonPropertyName("sub_tier")]
    public SubTier SubTier { get; set; }

    /// <summary>
    /// Indicates if the subscription was obtained through Amazon Prime.
    /// </summary>
    [JsonPropertyName("is_prime")]
    public bool IsPrime { get; set; }

    /// <summary>
    /// The number of months the subscription is for.
    /// </summary>
    [JsonPropertyName("duration_months")]
    public int DurationMonths { get; set; }
}
