using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

/// This shows as DecimalPlace without the S on the documentation?

public class Amount
{
    /// <summary>
    /// The monetary amount. The amount is specified in the currency’s minor unit. For example, the minor units for USD is cents, so if the amount is $5.50 USD, value is set to 550.
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; set; }

    /// <summary>
    /// The number of decimal places used by the currency. For example, USD uses two decimal places.
    /// </summary>
    [JsonPropertyName("decimal_place")]
    public int DecimalPlace { get; set; }

    /// <summary>
    /// The ISO-4217 three-letter currency code that identifies the type of currency in value.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}