using Koishibot.Core.Services.StreamElements.Enums;
using Koishibot.Core.Services.StreamElements.Models;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Websockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using Timer = System.Timers.Timer;
namespace Koishibot.Core.Services.StreamElements;

public class StreamElementsWebSocket : WebSocketHandlerBase
{
	public StreamElementsWebSocket(
		string url,
		IOptions<Settings> settings,
		CancellationToken cancel,
		byte maxReconnectAttempts
	) : base(url, cancel, maxReconnectAttempts)
	{
		_settings = settings;
		MessageReceived += OnMessageReceived;
	}
	private IOptions<Settings> _settings;

	private Timer _keepaliveTimer;
	private int _keepaliveTimeoutSeconds = 20;

	private readonly LimitedSizeHashSet<StreamElementsEvent, string> _eventSet
		= new(10, x => x.Id);

	public Action<StreamElementsEvent>? OnTipReceived { get; set; }
	public Action? OnAuthenticated { get; set; }
	public Action? OnUnauthorized { get; set; }

	public async Task ConnectWebSocket()
	{
		await Task.Run(async () => await Connect());
	}

	private async void OnMessageReceived(string message)
	{
		if (MessageIsPing(message))
		{
			await SendPong();
			return;
		}

		if (MessageIsConnected(message))
		{
			await Authenticate();
			return;
		}

		if (MessageIsAuthenticated(message))
		{
			OnAuthenticated?.Invoke();
			return;
		}

		if (MessageIsUnauthorized(message))
		{
			OnUnauthorized?.Invoke();
			return;
		}

		if (MessageIsEvent(message))
		{
			if (message.Contains("event:update")) { return; }

			var response = ParseResponse(message);
			var eventData = JsonSerializer.Deserialize<StreamElementsEvent>(response);

			if (eventData == null) { return; }

			if (!_eventSet.Contains(eventData.Id))
			{
				_eventSet.Add(eventData);
			}

			if (eventData.Type == EventType.Tip)
			{
				OnTipReceived?.Invoke(eventData);
			}
		}
	}

	public bool MessageIsPing(string message)
		=> message.StartsWith("0{\"sid\"");

	public async Task SendPong()
		=> await SendMessage("2");

	public bool MessageIsConnected(string message)
		=> message.StartsWith("40");  // Message Connect


	public bool MessageIsAuthenticated(string message)
		=> message.Contains("authenticated");

	public bool MessageIsUnauthorized(string message)
		=> message.Contains("unauthorized");

	public bool MessageIsEvent(string message)
		=> message.StartsWith("42");

	public string ParseResponse(string message)
	{
		var trimmed = Regex.Replace(message, @"^\d+", "");
		var doc = JsonDocument.Parse(trimmed);
		var root = doc.RootElement;
		return root[1].GetRawText();
	}

	// AUTHENTICATE

	public async Task Authenticate()
	{
		var jwt = _settings.Value.StreamElementsJwtToken;
		var message = new AuthenticationRequest(jwt);

		await SendMessage(message.ConvertToString());
	}

	// TIMER

	private void StartKeepaliveTimer()
	{
		_keepaliveTimer = new Timer(TimeSpan.FromSeconds(_keepaliveTimeoutSeconds));
		_keepaliveTimer.Elapsed += async (_, _) =>
		{
			var rightNow = DateTimeOffset.UtcNow;
			var lastEvent = _eventSet.LastItem();
			if (rightNow.Subtract(lastEvent.CreatedAt).Seconds < _keepaliveTimeoutSeconds - 3)
			{
				return;
			}
			await Disconnect();
		};
		_keepaliveTimer.Start();
	}
}