using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChatNotifications;

public class CharityDonation
{
    /// <summary>
    /// Name of the charity.
    /// </summary>
    [JsonPropertyName("charity_name")]
    public string CharityName { get; set; }

    /// <summary>
    /// An object that contains the amount of money that the user paid.
    /// </summary>
    [JsonPropertyName("amount")]
    public Amount Amount { get; set; }

}