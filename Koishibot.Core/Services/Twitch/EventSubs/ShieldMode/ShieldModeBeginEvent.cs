﻿using System.Text.Json.Serialization;
using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ShieldMode;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelshield_modebegin">Twitch Documentation</see><br/>
/// When the broadcaster activates Shield Mode.<br/>
/// Required Scopes: moderator:read:shield_mode OR moderator:manage:shield_mode
/// </summary>
public class ShieldModeBeginEvent
{
	/// <summary>
	///	The broadcaster’s user ID.
	/// </summary>
	[JsonPropertyName("broadcaster_user_id")]
	public string BroadcasterId { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user login. (Lowercase)
	/// </summary>
	[JsonPropertyName("broadcaster_user_login")]
	public string BroadcasterLogin { get; set; } = string.Empty;

	/// <summary>
	/// The broadcaster’s user display name.
	/// </summary>
	[JsonPropertyName("broadcaster_user_name")]
	public string BroadcasterUsername { get; set; } = string.Empty;

	/// <summary>
	/// The UserId of the moderator that updated the Shield Mode’s status.<br/>
	/// If the broadcaster updated the status, this ID will be the same as broadcaster_user_id.
	/// </summary>
	[JsonPropertyName("moderator_user_id")]
	public string ModeratorUserId { get; set; } = string.Empty;

	/// <summary>
	/// The Userlogin of the moderator that sent the Shoutout. (Lowercase)
	/// </summary>
	[JsonPropertyName("moderator_user_login")]
	public string ModeratorUserLogin { get; set; } = string.Empty;

	/// <summary>
	/// The Username of the moderator that sent the Shoutout.
	/// </summary>
	[JsonPropertyName("moderator_user_name")]
	public string ModeratorUserName { get; set; } = string.Empty;

	/// <summary>
	/// Thetimestamp of when the moderator activated Shield Mode. <br/>
	/// The object includes this field only for channel.shield_mode.begin events<br/>
	/// (RFC3339 format converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset StartedAt { get; set; }
}