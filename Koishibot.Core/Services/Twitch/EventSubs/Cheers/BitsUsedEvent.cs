using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Cheers;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelbitsuse">Twitch Documentation</see><br/>
/// Sends a notification whenever Bits are used on a channel.<br/>
/// Currently supports when a viewer cheers in a channel, uses a powerup (not the streamer), or sends combo.<br/>
/// Bit transactions via Twitch Extensions are not counted
/// Required Scopes: bits:read
/// </summary>
public class BitsUsedEvent
{

	/// <summary>
	///	The User ID of the channel where the Bits were redeemed.
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	/// <summary>
	/// The login of the channel where the Bits were used. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The display name of the channel where the Bits were used.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterName { get; set; } = string.Empty;

	/// <summary>
	///The User ID of the redeeming user.
	/// </summary>
	[JsonPropertyName("user_id")]
	public string CheererId { get; set; } = string.Empty;

	/// <summary>
	/// The login name of the redeeming user. (Lowercase)
	/// </summary>
	[JsonPropertyName("user_login")]
	public string CheererLogin { get; set; } = string.Empty;

	/// <summary>
	/// The display name of the redeeming user. 
	/// </summary>
	[JsonPropertyName("user_name")]
	public string CheererName { get; set; } = string.Empty;

	/// <summary>
	/// The number of bits used.
	/// </summary>
	[JsonPropertyName("bits")]
	public int BitAmount { get; set; }

	/// <summary>
	/// Type of event, cheer or powerup
	/// </summary>
	[JsonPropertyName("type")]
	public BitType Type { get; set; }

	/// <summary>
	/// Optional. An object that contains the user message and emote information needed to recreate the message.
	/// </summary>
	[JsonPropertyName("message")]
	public Message? Message { get; set; }

	/// <summary>
	/// Optional. Data about Power-up.
	/// </summary>
	[JsonPropertyName("message")]
	public PowerUp? PowerUpData { get; set; }
}

public class PowerUp
{
	/// <summary>
	/// The ID that uniquely identifies this emote.
	/// </summary>
	[JsonPropertyName("type")]
	public PowerUpType Type { get; set; }

	/// <summary>
	/// The ID that uniquely identifies this emote.
	/// </summary>
	[JsonPropertyName("emote")]
	public PowerUpEmote? Emote { get; set; }

	/// <summary>
	/// Optional. The ID of the message effect.
	/// </summary>
	[JsonPropertyName("message_effect_id")]
	public string? MessageEffectId { get; set; }
}

public class PowerUpEmote
{
	/// <summary>
	/// The ID that uniquely identifies this emote.
	/// </summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>
	///The human readable emote token.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }
}