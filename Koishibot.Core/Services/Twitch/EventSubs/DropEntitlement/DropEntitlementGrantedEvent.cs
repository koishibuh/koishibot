using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.DropEntitlement;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#dropentitlementgrant">Twitch Documentation</see><br/>
/// When an entitlement for a Drop is granted to a user.<br/>
/// NOTE This subscription type is only supported by webhooks, and cannot be used with WebSockets. <br/>
/// Required Scopes: App access token required. The client ID associated with the access token must be owned by a user who is part of the specified organization.<br/>
/// </summary>
public class UserAuthorizationGrantedEvent
{
    ///<summary>
    ///Individual event ID, as assigned by EventSub. Use this for de-duplicating messages.
    ///</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    ///<summary>
    ///Entitlement object.
    ///</summary>
    [JsonPropertyName("data")]
    public EntitlementData? Data { get; set; }
}

// == ⚫ == //

public class EntitlementData
{
	///<summary>
	///The ID of the organization that owns the game that has Drops enabled.
	///</summary>
	[JsonPropertyName("organization_id")]
	public string? OrganizationId { get; set; }

	///<summary>
	///Twitch category ID of the game that was being played when this benefit was entitled.
	///</summary>
	[JsonPropertyName("category_id")]
	public string? CategoryId { get; set; }

	///<summary>
	///The category name.
	///</summary>
	[JsonPropertyName("category_name")]
	public string? CategoryName { get; set; }

	///<summary>
	///The campaign this entitlement is associated with.
	///</summary>
	[JsonPropertyName("campaign_id")]
	public string? CampaignId { get; set; }

	///<summary>
	///Twitch user ID of the user who was granted the entitlement.
	///</summary>
	[JsonPropertyName("user_id")]
	public string? UserId { get; set; }

	///<summary>
	///The user display name of the user who was granted the entitlement.
	///</summary>
	[JsonPropertyName("user_name")]
	public string? UserName { get; set; }

	///<summary>
	///The user login of the user who was granted the entitlement.
	///</summary>
	[JsonPropertyName("user_login")]
	public string? UserLogin { get; set; }

	///<summary>
	///Unique identifier of the entitlement. Use this to de-duplicate entitlements.
	///</summary>
	[JsonPropertyName("entitlement_id")]
	public string? EntitlementId { get; set; }

	///<summary>
	///Identifier of the Benefit.
	///</summary>
	[JsonPropertyName("benefit_id")]
	public string? BenefitId { get; set; }

	///<summary>
	///The timestamp when this entitlement was granted on Twitch.<br/>s
	///(ISO format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	[JsonConverter(typeof(DateTimeOffsetRFC3339Converter))]
	public DateTimeOffset CreatedAt { get; set; }
}