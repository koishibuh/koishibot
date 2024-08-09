using System.Net.WebSockets;

namespace Koishibot.Core.Services.Websockets;

public class WebSocketFactory : IWebSocketFactory
{
	public WebSocketClient? ActiveClient { get; set; }	

	public async Task<WebSocketClient> Create(
		string url,
		byte maxReconnectAttempts,
		Func<WebSocketMessage, Task> OnError,
		Func<WebSocketMessage, Task> OnMessageReceived
		//Func<Task> OnConnected
		)
	{
		if (ActiveClient is not null)
		{
			return ActiveClient;
		}

		var client = new ClientWebSocket();

		try
		{
			await client.ConnectAsync(new Uri(url), default);
		}
		catch (WebSocketException)
		{
			await OnError.Invoke(new WebSocketMessage($"Failed to connect to the websocket at {url}."));
			throw new WebSocketException($"Failed to connect to the websocket at {url}.");
		}

		//var retryCount = 0;
		//while (client.State != WebSocketState.Open
		//			&& retryCount < maxReconnectAttempts)
		//{
		//	try
		//	{
		//		await client.ConnectAsync(new Uri(url), default);
		//		retryCount = 0;
		//	}
		//	catch (WebSocketException)
		//	{
		//		var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
		//		await Task.Delay(delay);
		//		retryCount++;
		//	}
		//}
		//if (retryCount >= maxReconnectAttempts)
		//{
		//	throw new WebSocketException($"Failed to connect to the websocket at {url}.");
		//}

		var webSocketClient = new WebSocketClient(client, OnError, OnMessageReceived);
		_ = Task.Run(webSocketClient.StartListening);
		//await OnConnected.Invoke();

		ActiveClient = webSocketClient;

		return webSocketClient;
	}

	public async void Remove(string name)
	{
		ActiveClient?.Dispose();
	}
}


// == ⚫ INTERFACE == //

public interface IWebSocketFactory
{
	Task<WebSocketClient> Create(string url, byte maxReconnectAttempts,
		Func<WebSocketMessage, Task> OnError, Func<WebSocketMessage, Task> OnMessageReceived);
	void Remove(string name);
}