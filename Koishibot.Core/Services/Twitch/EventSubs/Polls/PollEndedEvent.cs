﻿using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Converters;
using Koishibot.Core.Services.Twitch.Enums;

namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;


/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Twitch Documentation</see><br/>
/// When a poll ends on the specified channel.<br/>
/// Required Scopes: channel:read:polls OR channel:manage:polls 
/// </summary>
public class PollEndedEvent
{
	/// <summary>
	/// ID of the poll.
	/// </summary>
	[JsonPropertyName("id")]
	public string PollId { get; set; }

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

	/// <summary>
	/// Question displayed for the poll.
	/// </summary>
	[JsonPropertyName("title")]
	public string Title { get; set; }

	/// <summary>
	/// An array of choices for the poll.
	/// </summary>
	[JsonPropertyName("choices")]
	public List<Choice> Choices { get; set; }

	/// <summary>
	/// Not supported.
	/// </summary>
	[JsonPropertyName("bits_voting")]
	public BitsVoting BitsVoting { get; set; }

	/// <summary>
	/// The Channel Points voting settings for the poll.
	/// </summary>
	[JsonPropertyName("channel_points_voting")]
	public ChannelPointsVoting ChannelPointsVoting { get; set; }

	/// <summary>
	/// The status of the poll.
	/// </summary>
	[JsonPropertyName("status")]
	[JsonConverter(typeof(PollStatusEnumConverter))]
	public PollStatus Status { get; set; }


	/// <summary>
	/// The time the poll started.<br/>
	/// (Converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("started_at")]
	public DateTimeOffset StartedAt { get; set; }


	/// <summary>
	/// The time the poll ended.<br/>
	/// (Converted to DateTimeOffset)
	/// </summary>
	[JsonPropertyName("ended_at")]
	public DateTimeOffset EndedAt { get; set; }

}