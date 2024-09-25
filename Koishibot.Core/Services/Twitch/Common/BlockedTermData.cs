using Koishibot.Core.Services.Twitch.Converters;
using System.Text.Json.Serialization;

namespace Koishibot.Core.Services.Twitch.Common;

public class BlockedTermData
{
	///<summary>
	///The broadcaster that owns the list of blocked terms.
	///</summary>
	[JsonPropertyName("broadcaster_id")]
	public string BroadcasterId { get; set; }

	///<summary>
	///The moderator that blocked the word or phrase from being used in the broadcaster’s chat room.
	///</summary>
	[JsonPropertyName("moderator_id")]
	public string ModeratorId { get; set; }

	///<summary>
	///An ID that identifies this blocked term.
	///</summary>
	[JsonPropertyName("id")]
	public string BlockedTermId { get; set; }

	///<summary>
	///The blocked word or phrase.
	///</summary>
	[JsonPropertyName("text")]
	public string BlockedTerm { get; set; }

	///<summary>
	///The timestamp that the term was blocked.<br/>
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("created_at")]
	public DateTimeOffset CreatedAt { get; set; }

	///<summary>
	///The timestamp that the term was updated.<br/>
	///When the term is added, this timestamp is the same as created_at.<br/>
	///The timestamp changes as AutoMod continues to deny the term.
	///(RFC3339 format converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("updated_at")]
	public DateTimeOffset LastDeniedAt { get; set; }

	///<summary>
	///The UTC date and time (in RFC3339 format) that the blocked term is set to expire.<br/>
	///After the block expires, users may use the term in the broadcaster’s chat room.<br/>
	///This field is null if the term was added manually or was permanently blocked by AutoMod.
	///</summary>
	[JsonPropertyName("expires_at")]
	public DateTimeOffset? ExpiresAt { get; set; }

}