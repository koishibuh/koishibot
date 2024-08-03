using Koishibot.Core.Services.StreamElements.Enums;
using Koishibot.Core.Services.Twitch.Converters;

namespace Koishibot.Core.Services.StreamElements.Models;

public class StreamElementsResponse
{
	[JsonPropertyName("event")]
	public string EventType { get; set; }

	//[JsonPropertyName("ts")]
	//public long Timestamp { get; set; }

	//[JsonPropertyName("nonce")]
	//public string Nonce { get; set; }
}

public class StreamElementsEvent
{
	[JsonPropertyName("channel")]
	public string Channel { get; set; } = string.Empty;

	/// <summary>
	/// Service alert is from: Twitch, Facebook, Youtube, Trovo?
	/// </summary>
	[JsonPropertyName("provider")]
	public string Provider { get; set; } = string.Empty;

	[JsonPropertyName("type")]
	public EventType Type { get; set; }

	[JsonPropertyName("createdAt")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset CreatedAt { get; set; }

	//[JsonPropertyName("isMock")]
	//public bool IsMock { get; set; }

	[JsonPropertyName("data")]
	public Data Data { get; set; }

	[JsonPropertyName("updatedAt")]
	[JsonConverter(typeof(RFCToDateTimeOffsetConverter))]
	public DateTimeOffset UpdatedAt { get; set; }

	[JsonPropertyName("_id")]
	public string Id { get; set; }

	[JsonPropertyName("activityId")]
	public string ActivityId { get; set; }

	[JsonPropertyName("sessionEventsCount")]
	public int SessionEventsCount { get; set; }
}


public class Data
{
	/// <summary>
	/// Merch
	/// </summary>
	//[JsonPropertyName("items")]
	//public Items Items { get; set; }

	[JsonPropertyName("amount")]
	public int Amount { get; set; }

	[JsonPropertyName("avatar")]
	public string AvatarUrl { get; set; }

	[JsonPropertyName("displayName")]
	public string UserName { get; set; }

	[JsonPropertyName("username")]
	public string UserLogin { get; set; }

	/// <summary>
	/// Provider is the service (ie: Twitch, Facebook, Youtube, Trovo)
	/// </summary>
	[JsonPropertyName("providerId")]
	public string UserId { get; set; }

	/// <summary>
	/// Gifted Sub
	/// </summary>
	[JsonPropertyName("sender")]
	public string Sender { get; set; }

	/// <summary>
	/// Sub
	/// </summary>
	[JsonPropertyName("gifted")]
	public bool Gifted { get; set; }

	/// <summary>
	/// Cheer
	/// </summary>
	[JsonPropertyName("message")]
	public string Message { get; set; }
}