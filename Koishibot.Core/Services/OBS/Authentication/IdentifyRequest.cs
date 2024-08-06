using Koishibot.Core.Services.OBS.Enums;
namespace Koishibot.Core.Services.OBS.Authentication;


public class IdentifyRequest
{
	public OpCodeType? Op { get; set; }

	[JsonPropertyName("d")]
	public IdentifyData Data { get; set; } = null!;
}


/// <summary>
/// Should contain authentication string if authentication is required.<br/>
/// Along with PubSub subscriptions and other session parameters.
/// </summary>
public class IdentifyData
{
	/// <summary>
	/// The version number that the client would like the obs-websocket server to use.
	/// </summary>
	public int RpcVersion { get; set; }

	/// <summary>
	/// The authenticated string from the Challenge and Salt
	/// </summary>
	public string? Authentication { get; set; }

	/// <summary>
	/// A bitmask of EventSubscriptions items to subscribe to events and event categories at will.<br/>
	/// By default, all event categories are subscribed, except for events marked as high volume.<br/>
	/// High volume events must be explicitly subscribed to.
	/// </summary>
	public int EventSubscriptions { get; set; }
}