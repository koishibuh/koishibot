﻿namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.StreamStatus;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#streamoffline">Twitch Documentation</see><br/>
/// When the specified broadcaster stops a stream.<br/>
/// Required Scopes: No authorization required.
/// </summary>
public class StreamOfflineEvent
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
	public string BroadcasterName { get; set; } = string.Empty;
}