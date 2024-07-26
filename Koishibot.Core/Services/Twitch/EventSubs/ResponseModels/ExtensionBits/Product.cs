using System.Text.Json.Serialization;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ExtensionBits;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-reference/#product">Twitch Documentation</see><br/>
/// Additional information about a product acquired via a Twitch Extension Bits transaction.<br/>
/// </summary>
public class Product
{
    ///<summary>
    ///Product name.
    ///</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    ///<summary>
    ///Bits involved in the transaction.
    ///</summary>
    [JsonPropertyName("bits")]
    public int Bits { get; set; }

    ///<summary>
    ///Unique identifier for the product acquired.
    ///</summary>
    [JsonPropertyName("sku")]
    public string Sku { get; set; }

    ///<summary>
    ///Flag indicating if the product is in development. If in_development is true, bits will be 0.
    ///</summary>
    [JsonPropertyName("in_development")]
    public bool InDevelopment { get; set; }
}