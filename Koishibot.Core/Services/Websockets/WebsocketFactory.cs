using Koishibot.Core.Features.Common;
using System.Net.WebSockets;

namespace Koishibot.Core.Services.Websockets;

/*══════════════════【 FACTORY 】══════════════════*/
public class WebSocketFactory : IWebSocketFactory
{
	private WebSocketHandler? ActiveClient { get; set; }
	public string ClientId { get; set; }

	public async Task<WebSocketHandler> Create(
	string url,
	byte maxReconnectAttempts,
	Func<WebSocketMessage, Task> onMessageReceived,
	Func<WebSocketMessage, Task> onError,
	Func<WebSocketMessage, Task> onClosed
	)
	{
		if (ActiveClient is not null && ActiveClient.IsDisposed is false)
			return ActiveClient;

		var client = new ClientWebSocket();
		var uri = new Uri(url);
		await client.ConnectAsync(uri, default);

		ActiveClient = new WebSocketHandler(client, onMessageReceived, onError, onClosed);
		_ = Task.Run(ActiveClient.StartListening);

		ClientId = Toolbox.ShortGuid(ActiveClient.Id);

		return ActiveClient;
	}

	public async Task Disconnect()
	{
		if (ActiveClient is null) return;

		await ActiveClient.Disconnect();
		ActiveClient = null;
	}
}


/*══════════════════【 INTERFACE 】══════════════════*/
public interface IWebSocketFactory
{
	Task<WebSocketHandler> Create(string url, byte maxReconnectAttempts,
	Func<WebSocketMessage, Task> onMessageReceived, Func<WebSocketMessage, Task> onError,
	Func<WebSocketMessage, Task> onClosed);
	Task Disconnect();
}