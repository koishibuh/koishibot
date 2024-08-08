using System.Net.WebSockets;
using System.Text;
namespace Koishibot.Core.Services.Websockets;

public sealed class WebSocketClient(
	ClientWebSocket Client,
	Func<WebSocketMessage, Task> OnError,
	Func<WebSocketMessage, Task> OnMessageReceived
	) : IDisposable
{
	public bool IsDisposed { get; private set; }
	public CancellationTokenSource CancelSource = new();

	// == ⚫ == //

	public async Task StartListening()
	{
		try
		{
			using var memoryStream = new MemoryStream();
			while (Client.State == WebSocketState.Open)
			{
				WebSocketReceiveResult result;
				do
				{
					var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
					result = await Client.ReceiveAsync(messageBuffer, default);

					await memoryStream.WriteAsync(
						messageBuffer.Array.AsMemory(messageBuffer.Offset, result.Count),	default);
				} while (!result.EndOfMessage);

				if (result is { MessageType: WebSocketMessageType.Text })
				{
					var message = Encoding.UTF8.GetString(memoryStream.ToArray());
					await OnMessageReceived.Invoke(new WebSocketMessage(message));
				}
				else if (result is { MessageType: WebSocketMessageType.Close })
				{
					var error = $"{result.CloseStatus}: {result.CloseStatusDescription}";
					await OnError.Invoke(new WebSocketMessage(error));
					await Disconnect();
				}
				memoryStream.Seek(0, SeekOrigin.Begin);
				memoryStream.Position = 0;
				memoryStream.SetLength(0);
			}
		}
		catch (WebSocketException e)
		{
			var error = e.WebSocketErrorCode.ToString();
			await OnError.Invoke(new WebSocketMessage(error));
		}
	}

	public async Task SendMessage(string message, CancellationToken cancel = default)
	{
		var bytes = Encoding.UTF8.GetBytes(message);
		await Client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancel);
	}

	public async Task Disconnect()
	{
		await Client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", default);
		CancelSource.Cancel();

		Dispose();
	}

	public void Dispose()
	{
		if (IsDisposed) { return; }
		IsDisposed = true;

		Client.Dispose();
	}
}
