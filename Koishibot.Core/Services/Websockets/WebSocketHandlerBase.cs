using System.Net.WebSockets;
using System.Text;
namespace Koishibot.Core.Services.Websockets;

public abstract class WebSocketHandlerBase
{
	public ClientWebSocket? _client = null;
	private readonly string _url;
	private readonly byte _maxReconnectAttempts;
	public CancellationToken _cancel;
	public CancellationTokenSource? _cancelSource = null;

	public event Action? Connected;
	public event Action? Reconnecting;
	public event Action<string>? Error;
	public event Action? Disconnected;
	public event Action<string>? MessageReceived;

	protected WebSocketHandlerBase(string url, CancellationToken cancel, byte maxReconnectAttempts)
	{
		_url = url;
		_cancel = cancel;
		_maxReconnectAttempts = maxReconnectAttempts;
	}

	public async Task Connect()
	{
		_client?.Dispose();

		_client = new ClientWebSocket();
		_cancelSource = new CancellationTokenSource();

		var retryCount = 0;
		while (_client.State != WebSocketState.Open
					&& retryCount < _maxReconnectAttempts)
		{
			try
			{
				await _client.ConnectAsync(new Uri(_url), _cancel);
				retryCount = 0;
			}
			catch (WebSocketException)
			{
				var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
				await Task.Delay(delay, _cancel);
				Reconnecting?.Invoke();
				retryCount++;
			}
		}
		if (retryCount >= _maxReconnectAttempts)
		{
			Error?.Invoke("Maxed Reconnect Attempts");
			throw new WebSocketException($"Failed to connect to the websocket at {_url}.");
		}
		_ = Task.Run(StartListening, _cancel);
		Connected?.Invoke();
	}

	private async Task StartListening()
	{
		try
		{
			using var memoryStream = new MemoryStream();
			while (_client is not null && _client.State == WebSocketState.Open)
			{
				WebSocketReceiveResult result;
				do
				{
					var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
					result = await _client.ReceiveAsync(messageBuffer, _cancel);

					await memoryStream.WriteAsync(
						messageBuffer.Array.AsMemory(messageBuffer.Offset, result.Count),
						_cancel);
				} while (!result.EndOfMessage);

				if (result is { MessageType: WebSocketMessageType.Text })
				{
					var message = Encoding.UTF8.GetString(memoryStream.ToArray());
					MessageReceived?.Invoke(message);
				}
				else if (result is { MessageType: WebSocketMessageType.Close })
				{
					Error?.Invoke($"Close status: {result.CloseStatus} ({result.CloseStatusDescription})");
					await Disconnect();
				}
				memoryStream.Seek(0, SeekOrigin.Begin);
				memoryStream.Position = 0;
				memoryStream.SetLength(0);
			}
		}
		catch (WebSocketException e)
		{
			Error?.Invoke(e.WebSocketErrorCode.ToString());
		}
	}



	public async Task SendMessage(string message)
	{
		var bytes = Encoding.UTF8.GetBytes(message);
		await _client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, _cancel);
	}

	public async Task Disconnect()
	{
		if (_client is null) { return; }

		await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", _cancel);

		_cancelSource?.Cancel();
		_client.Dispose();
		_client = null;

		Disconnected?.Invoke();
	}
}

