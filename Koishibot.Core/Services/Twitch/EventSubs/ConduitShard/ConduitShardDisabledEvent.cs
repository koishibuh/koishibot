namespace Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ConduitShard;

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#conduitsharddisabled">Twitch Documentation</see><br/>
/// When EventSub disables a shard due to the status of the underlying transport changing.<br/>
/// Required Scopes: Check documentation for list<br/>
/// </summary>
public class ConduitShardDisabledEvent
{
	///<summary>
	///The ID of the conduit.
	///</summary>
	[JsonPropertyName("conduit_id")]
	public string? ConduitId { get; set; }

	///<summary>
	///The ID of the disabled shard.
	///</summary>
	[JsonPropertyName("shard_id")]
	public string? ShardId { get; set; }

	///<summary>
	///The new status of the transport.
	///</summary>
	[JsonPropertyName("status")]
	public string? Status { get; set; }

	///<summary>
	///The disabled transport.
	///</summary>
	[JsonPropertyName("transport")]
	public Transport? Transport { get; set; }
}

// == ⚫ == //

public class Transport
{
	///<summary>
	///websocket or webhook
	///</summary>
	[JsonPropertyName("method")]
	public string? Method { get; set; }

	///<summary>
	///Optional. Webhook callback URL. Null if method is set to websocket.
	///</summary>
	[JsonPropertyName("callback")]
	public string? Callback { get; set; }

	///<summary>
	///Optional. WebSocket session ID. Null if method is set to webhook.
	///</summary>
	[JsonPropertyName("session_id")]
	public string? SessionId { get; set; }

	///<summary>
	///Optional. Time that the WebSocket session connected. Null if method is set to webhook.
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("connected_at")]
	public DateTimeOffset? ConnectedAt { get; set; }

	///<summary>
	///Optional. Time that the WebSocket session disconnected. Null if method is set to webhook. <br/>
	///(Converted to DateTimeOffset)
	///</summary>
	[JsonPropertyName("disconnected_at")]
	public DateTimeOffset? DisconnectedAt { get; set; }
}
