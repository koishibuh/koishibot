using Koishibot.Core.Exceptions;
using Koishibot.Core.Features.Common;
using System.Net.WebSockets;

namespace Koishibot.Core.Services.Websockets;

/*══════════════════【 FACTORY 】══════════════════*/
public class WebSocketFactory : IWebSocketFactory
{
	private WebSocketHandler? ActiveClient { get; set; }
	private int CurrentReconnectAttempts = 0;
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

		while (CurrentReconnectAttempts < maxReconnectAttempts)
		{
			try
			{
				var client = new ClientWebSocket();
				await client.ConnectAsync(new Uri(url), default);
			
				ActiveClient = new WebSocketHandler(client, onMessageReceived, onError, onClosed);
				_ = Task.Run(ActiveClient.StartListening);

				CurrentReconnectAttempts = 0;
				ClientId = Toolbox.ShortGuid(ActiveClient.Id);

				return ActiveClient;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Reconnect attempt number {CurrentReconnectAttempts} failed");
				CurrentReconnectAttempts++;
				
				if (CurrentReconnectAttempts >= maxReconnectAttempts)
					throw new CustomException("Max reconnect attempts exceeded");
				
				await Task.Delay(TimeSpan.FromSeconds(2 * CurrentReconnectAttempts));
				Console.WriteLine($"Attempting to reconnect");
			}
		}
		
		throw new CustomException("Max websocket reconnect attempts exceeded");
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