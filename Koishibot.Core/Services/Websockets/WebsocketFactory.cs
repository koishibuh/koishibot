using System.Net.WebSockets;
using Koishibot.Core.Exceptions;

namespace Koishibot.Core.Services.Websockets;

/*══════════════════【 FACTORY 】══════════════════*/
public class WebSocketFactory : IWebSocketFactory
{
	private WebSocketHandler? ActiveClient { get; set; }

	public async Task<WebSocketHandler> Create(
	string url,
	byte maxReconnectAttempts,
	Func<WebSocketMessage, Task> onError,
	Func<WebSocketMessage, Task> onMessageReceived
	//Func<Task> OnConnected
	)
	{
		if (ActiveClient is not null)
			return ActiveClient;

		var client = new ClientWebSocket();
		var uri = new Uri(url);
		await client.ConnectAsync(uri, default);

		ActiveClient = new WebSocketHandler(client, onError, onMessageReceived);
		_ = Task.Run(ActiveClient.StartListening);

		return ActiveClient;
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IWebSocketFactory
{
	Task<WebSocketHandler> Create(string url, byte maxReconnectAttempts,
	Func<WebSocketMessage, Task> onError, Func<WebSocketMessage, Task> onMessageReceived);
}