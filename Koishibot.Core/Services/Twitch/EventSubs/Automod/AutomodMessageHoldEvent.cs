using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Automod;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#automodmessagehold-v2">Twitch Documentation</see><br/>
/// Notifies a user if a message was caught by automod for review. Only public blocked terms trigger notifications, not private ones.<br/>
/// Required Scopes: moderator:manage:automod<br/>
/// </summary>
public class AutomodMessageHoldEvent
{
	///<summary>
	///The ID of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The login of the broadcaster specified in the request. (Lowercase)
	///</summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	///The username of the broadcaster specified in the request.
	///</summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	///<summary>
	///The message sender’s user ID.
	///</summary>
	[JsonPropertyName("user_id")]
	public string ViewerId { get; set; } = string.Empty;

	///<summary>
	///The message sender’s login name. (Lowercase)
	///</summary>
	[JsonPropertyName("user_login")]
	public string ViewerLogin { get; set; } = string.Empty;

	///<summary>
	///The message sender’s display name.
	///</summary>
	[JsonPropertyName("user_name")]
	public string Viewername { get; set; } = string.Empty;

	///<summary>
	///The ID of the message that was flagged by automod.
	///</summary>
	[JsonPropertyName("message_id")]
	public string MessageId { get; set; } = string.Empty;

	///<summary>
	///The body of the message.
	///</summary>
	[JsonPropertyName("message")]
	public List<Message> Message { get; set; } = null!;
	
	///<summary>
	///The timestamp of when automod saved the message.<br/>
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("held_at")]
	public DateTimeOffset HeldAt { get; set; }
	
	///<summary>
	///Reason the message was held.
	///</summary>
	[JsonPropertyName("reason")]
	public string Reason { get; set; } = string.Empty;

	///<summary>
	///Optional. If the message was caught by automod, this will be populated.
	///</summary>
	[JsonPropertyName("automod")]
	public Automod? Automod { get; set; }
	
	///<summary>
	///Optional. If the message was caught due to a blocked term, this will be populated.
	///</summary>
	[JsonPropertyName("blocked_term")]
	public BlockedTerm? BlockedTerm { get; set; }
	
}

public class Automod
{
	///<summary>
	///The category of the message.
	///</summary>
	[JsonPropertyName("category")]
	public string Category { get; set; } = string.Empty;
	
	///<summary>
	///The level of severity. Measured between 1 to 4.
	///</summary>
	[JsonPropertyName("level")]
	public int Level { get; set; }
	
	///<summary>
	///The bounds of the text that caused the message to be caught.
	///</summary>
	[JsonPropertyName("boundaries")]
	public List<Boundary> Boundaries { get; set; }
}

public class Boundary
{
	///<summary>
	///Index in the message for the start of the problem (0 indexed, inclusive).
	///</summary>
	[JsonPropertyName("start_pos")]
	public int StartingPosition { get; set; } 
	
	///<summary>
	///Index in the message for the end of the problem (0 indexed, inclusive).
	///</summary>
	[JsonPropertyName("end_pos")]
	public int EndingPosition { get; set; }
}

public class BlockedTerm
{
	///<summary>
	///The list of blocked terms found in the message.
	///</summary>
	[JsonPropertyName("terms_found")]
	public List<Term> FoundTerms { get; set; }
}

public class Term
{	
	///<summary>
	///The id of the blocked term found.
	///</summary>
	[JsonPropertyName("term_id")]
	public string  Id { get; set; }
	
	///<summary>
	///The bounds of the text that caused the message to be caught.
	///</summary>
	[JsonPropertyName("boundaries")]
	public List<Boundary> Boundaries { get; set; }
	
	///<summary>
	///The id of the broadcaster that owns the blocked term.
	///</summary>
	[JsonPropertyName("owner_broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	///<summary>
	///The login of the broadcaster that owns the blocked term. (Lowercase)
	///</summary>
	[JsonPropertyName("owner_broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	///<summary>
	///The username of the broadcaster that owns the blocked term.
	///</summary>
	[JsonPropertyName("owner_broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;
}