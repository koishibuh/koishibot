using System.Net.WebSockets;

namespace Koishibot.Core.Services.Websockets;

/*══════════════════【 FACTORY 】══════════════════*/
public class WebSocketFactory : IWebSocketFactory
{
	private WebSocketClient? ActiveClient { get; set; }

	public async Task<WebSocketClient> Create(
	string url,
	byte maxReconnectAttempts,
	Func<WebSocketMessage, Task> onError,
	Func<WebSocketMessage, Task> onMessageReceived
	//Func<Task> OnConnected
	)
	{
		if (ActiveClient is not null) { return ActiveClient; }

		var client = new ClientWebSocket();
		try
		{
			await client.ConnectAsync(new Uri(url), default);
		}
		catch (WebSocketException)
		{
			client.Dispose();
			await onError.Invoke(new WebSocketMessage($"Failed to connect to the websocket"));
			throw new WebSocketException($"Failed to connect to the websocket at {url}.");
		}

		ActiveClient = new WebSocketClient(client, onError, onMessageReceived);
		_ = Task.Run(ActiveClient.StartListening);

		return ActiveClient;
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IWebSocketFactory
{
	Task<WebSocketClient> Create(string url, byte maxReconnectAttempts,
	Func<WebSocketMessage, Task> onError, Func<WebSocketMessage, Task> onMessageReceived);
}