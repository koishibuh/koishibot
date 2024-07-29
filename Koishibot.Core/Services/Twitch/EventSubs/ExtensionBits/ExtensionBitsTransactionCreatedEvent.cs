namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ExtensionBits;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#extensionbits_transactioncreate">Twitch Documentation</see><br/>
/// NOTE This subscription type is only supported by webhooks, and cannot be used with WebSockets.<br/>
/// When a new transaction is created for a Twitch Extension.<br/>
/// Required Scopes: The OAuth token client ID must match the Extension client ID.<br/>
/// </summary>
public class ExtensionBitsTransactionCreatedEvent
{
    ///<summary>
    ///Client ID of the extension.
    ///</summary>
    [JsonPropertyName("extension_client_id")]
    public string? ExtensionClientId { get; set; }

    ///<summary>
    ///Transaction ID.
    ///</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    ///<summary>
    ///The transaction’s broadcaster ID.
    ///</summary>
    [JsonPropertyName("broadcaster_user_id")]
    public string? BroadcasterId { get; set; }

    ///<summary>
    ///The transaction’s broadcaster login.
    ///</summary>
    [JsonPropertyName("broadcaster_user_login")]
    public string? BroadcasterLogin { get; set; }

    ///<summary>
    ///The transaction’s broadcaster display name.
    ///</summary>
    [JsonPropertyName("broadcaster_user_name")]
    public string? BroadcasterName { get; set; }

    ///<summary>
    ///The transaction’s user ID.
    ///</summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    ///<summary>
    ///The transaction’s user login.
    ///</summary>
    [JsonPropertyName("user_login")]
    public string? UserLogin { get; set; }

    ///<summary>
    ///The transaction’s user display name.
    ///</summary>
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    ///<summary>
    ///Additional extension product information.
    ///</summary>
    [JsonPropertyName("product")]
    public Product? Product { get; set; }
}

// == ⚫ == //

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
	public string? Name { get; set; }

	///<summary>
	///Bits involved in the transaction.
	///</summary>
	[JsonPropertyName("bits")]
	public int Bits { get; set; }

	///<summary>
	///Unique identifier for the product acquired.
	///</summary>
	[JsonPropertyName("sku")]
	public string? Sku { get; set; }

	///<summary>
	///Flag indicating if the product is in development. If in_development is true, bits will be 0.
	///</summary>
	[JsonPropertyName("in_development")]
	public bool InDevelopment { get; set; }
}