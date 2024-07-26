using System.Net.WebSockets;
using System.Text;
namespace Koishibot.Core.Services.Websockets;

public abstract class WebSocketHandlerBase
{
	public WebSocketState State => _socket.State;
	private readonly ClientWebSocket _socket = new();
	private readonly string _url;
	private readonly byte _maxReconnectAttempts;
	protected CancellationToken _cancel;

	public event Func<Task>? Connected;
	public event Action? Reconnecting;
	public event Action? OnDisconnectError;
	public event Action<string>? MessageReceived;

	protected WebSocketHandlerBase(string url, CancellationToken cancel, byte maxReconnectAttempts)
	{
		_url = url;
		_cancel = cancel;
		_maxReconnectAttempts = maxReconnectAttempts;
	}

	public async Task Connect()
	{
		var retryCount = 0;
		while (_socket.State != WebSocketState.Open && retryCount < _maxReconnectAttempts)
		{
			try
			{
				await _socket.ConnectAsync(new Uri(_url), _cancel);
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
			while (_socket.State == WebSocketState.Open)
			{
				WebSocketReceiveResult result;
				do
				{
					var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
					result = await _socket.ReceiveAsync(messageBuffer, _cancel);
					await memoryStream.WriteAsync(messageBuffer.Array.AsMemory(messageBuffer.Offset, result.Count),
							_cancel);
				} while (!result.EndOfMessage);

				if (result is { MessageType: WebSocketMessageType.Text })
				{
					var message = Encoding.UTF8.GetString(memoryStream.ToArray());
					MessageReceived?.Invoke(message);
				}
				memoryStream.Seek(0, SeekOrigin.Begin);
				memoryStream.Position = 0;
				memoryStream.SetLength(0);
			}
		}
		catch (WebSocketException e)
		{
			if (e.Message ==
					"The remote party closed the WebSocket connection without completing the close handshake.")
			{
				OnDisconnectError?.Invoke();
				await Connect();
			}
		}
	}

	public async Task SendMessage(string message)
	{
		var bytes = Encoding.UTF8.GetBytes(message);
		await _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, _cancel);
	}

	public async Task Disconnect()
	{
		if (_socket.State == WebSocketState.Open)
		{
			await _socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Closing", _cancel);
		}
	}

}