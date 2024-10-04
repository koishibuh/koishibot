using System.Net.WebSockets;
using System.Text;
namespace Koishibot.Core.Services.Websockets;

/*══════════════════【 CLIENT 】══════════════════*/
public sealed class WebSocketHandler(
ClientWebSocket client,
Func<WebSocketMessage, Task> onMessageReceived,
Func<WebSocketMessage, Task> onError,
Func<WebSocketMessage, Task> onClosed
) : IDisposable
{
	public Guid Id { get; } = Guid.NewGuid();
	public bool IsDisposed { get; private set; }
	private CancellationTokenSource CancelSource = new();

	public async Task StartListening()
	{
		try
		{
			using var memoryStream = new MemoryStream();
			while (client.State == WebSocketState.Open && !CancelSource.Token.IsCancellationRequested)
			{
				WebSocketReceiveResult result;
				do
				{
					var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
					result = await client.ReceiveAsync(messageBuffer, default);

					await memoryStream.WriteAsync(
					messageBuffer.Array.AsMemory(messageBuffer.Offset, result.Count), default);
				} while (!result.EndOfMessage);

				switch (result)
				{
					case { MessageType: WebSocketMessageType.Text }:
					{
						var message = Encoding.UTF8.GetString(memoryStream.ToArray());
						await onMessageReceived.Invoke(new WebSocketMessage(Id, message));
						break;
					}
					case { MessageType: WebSocketMessageType.Close }:
					{
						var message = $"{result.CloseStatus}: {result.CloseStatusDescription}";
						await onClosed.Invoke(new WebSocketMessage(Id, message));
						return;
					}
				}

				memoryStream.Seek(0, SeekOrigin.Begin);
				memoryStream.Position = 0;
				memoryStream.SetLength(0);
			}
		}
		catch (WebSocketException e)
		{
			var error = e.WebSocketErrorCode.ToString();
			await onError.Invoke(new WebSocketMessage(Id, error));
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception e)
		{
			await onError.Invoke(new WebSocketMessage(Id, $"Unexpected error: {e.Message}"));
		}
	}

	public async Task SendMessage(string message, CancellationToken cancel = default)
	{
		if (client.State != WebSocketState.Open)
			throw new InvalidOperationException("WebSocket is not open");

		var bytes = Encoding.UTF8.GetBytes(message);
		await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancel);
	}

	public async Task Disconnect()
	{
		if (client.State is WebSocketState.Open or WebSocketState.CloseReceived)
		{
			await CancelSource.CancelAsync();
			await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
			Dispose();
		}
	}

	public void Dispose()
	{
		if (IsDisposed) return;

		IsDisposed = true;
		client.Dispose();
		CancelSource.Dispose();
	}
}