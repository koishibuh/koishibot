namespace Koishibot.Core.Services.Twitch.Common;

public record TransportMethod
{
	/// <summary>
	/// The transport method.
	/// </summary>
	[JsonPropertyName("method")]
	public string Method => "websocket";

	/// <summary>
	/// An ID that identifies the WebSocket to send notifications to.<br/>
	/// When you connect to EventSub using WebSockets, the server returns the ID in the Welcome message. 
	/// </summary>
	[JsonPropertyName("session_id")]
	public string SessionId { get; set; } = null!;
};
