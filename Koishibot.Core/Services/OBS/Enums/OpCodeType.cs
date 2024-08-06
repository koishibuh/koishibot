namespace Koishibot.Core.Services.OBS.Enums;

/// <summary>
/// <see href="https://github.com/obsproject/obs-websocket/blob/master/docs/generated/protocol.md#hello-opcode-0"/>Github</see>
/// </summary>
public enum OpCodeType : byte

{/// <summary>
 /// First message sent from the server immediately on client connection.<br/>
 /// Contains authentication information if auth is required.<br/>
 /// Also contains RPC version for version negotiation.
 /// </summary>
	Hello = 0,

	/// <summary>
	/// Response to Hello message, should contain authentication string if authentication is required.<br/>
	/// As well as PubSub subscriptions and other session parameters.
	/// </summary>
	Identify = 1,

	/// <summary>
	/// The identify request was received and validated, and the connection is now ready for normal operation.
	/// </summary>
	Identified = 2,

	/// <summary>
	/// Sent at any time after initial identification to update the provided session parameters.
	/// </summary>
	Reidentify = 3,

	/// <summary>
	/// An event coming from OBS has occured.<br/>
	/// Example: scene switched, source muted.
	/// </summary>
	Event = 5,

	/// <summary>
	/// Client is making a request to obs-websocket.<br/>
	/// Example: get current scene, create source.
	/// </summary>
	Request = 6,

	/// <summary>
	/// Obs-websocket is responding to a request coming from a client.
	/// </summary>
	RequestResponse = 7,

	/// <summary>
	/// Client is making a batch of requests for obs-websocket.<br/>
	/// Requests are processed serially (in order) by the server.
	/// </summary>
	RequestBatch = 8,

	/// <summary>
	/// Obs-websocket is responding to a request batch coming from the client.
	/// </summary>
	RequestBatchResponse = 9
}