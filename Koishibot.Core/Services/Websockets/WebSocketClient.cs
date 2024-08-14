using System.Net.WebSockets;
using System.Text;

namespace Koishibot.Core.Services.Websockets;

/*══════════════════【 CLIENT 】══════════════════*/
public sealed class WebSocketClient(
ClientWebSocket client,
Func<WebSocketMessage, Task> onError,
Func<WebSocketMessage, Task> onMessageReceived
) : IDisposable
{
	public bool IsDisposed { get; private set; }
	private CancellationTokenSource CancelSource = new();

	public async Task StartListening()
	{
		try
		{
			using var memoryStream = new MemoryStream();
			while (client.State == WebSocketState.Open)
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
						await onMessageReceived.Invoke(new WebSocketMessage(message));
						break;
					}
					case { MessageType: WebSocketMessageType.Close }:
					{
						var error = $"{result.CloseStatus}: {result.CloseStatusDescription}";
						await onError.Invoke(new WebSocketMessage(error));
						await Disconnect();
						break;
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
			await onError.Invoke(new WebSocketMessage(error));
		}
	}

	public async Task SendMessage(string message, CancellationToken cancel = default)
	{
		var bytes = Encoding.UTF8.GetBytes(message);
		await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancel);
	}

	public async Task Disconnect()
	{
		await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", default);
		CancelSource.Cancel();

		Dispose();
	}

	public void Dispose()
	{
		if (IsDisposed)
		{
			return;
		}

		IsDisposed = true;
		client.Dispose();
	}
}