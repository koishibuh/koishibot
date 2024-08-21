using Koishibot.Core.Features.Supports.Events;
using Koishibot.Core.Services.StreamElements.Enums;
using Koishibot.Core.Services.StreamElements.Models;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Websockets;
using System.Text.Json;
using Timer = System.Timers.Timer;
namespace Koishibot.Core.Services.StreamElements;

/*═══════════════════【 SERVICE 】═══════════════════*/
public record StreamElementsService(
	ILogger<StreamElementsService> Log,
	IOptions<Settings> Settings,
	IAppCache Cache,
	ISignalrService Signalr,
	IServiceScopeFactory ScopeFactory
	) : IStreamElementsService
{
	public CancellationToken? Cancel { get; set; }
	public WebSocketHandler StreamElementsHandler { get; set; }

	private Timer _keepaliveTimer;
	private int _keepaliveTimeoutSeconds = 20;

	private readonly LimitedSizeHashSet<StreamElementsEvent, string> _eventSet
		= new(10, x => x.Id);

	public async Task CreateWebSocket()
	{
		const string url = "wss://realtime.streamelements.com/socket.io/?cluster=main&EIO=3&transport=websocket";
		var factory = new WebSocketFactory();

		StreamElementsHandler = await factory.Create(url, 3, Error, ProcessMessage);
	}

	private async Task ProcessMessage(WebSocketMessage message)
	{
		if (message.IsPing())
		{
			await SendPong();
			return;
		}

		if (message.IsConnected())
		{
			await Authenticate();
			return;
		}

		if (message.IsAuthenticated())
		{
			StartKeepaliveTimer();
			return;
		}

		if (message.IsUnauthorized())
		{
			await OnUnauthorized();
			return;
		}

		if (message.IsEvent())
		{
			if (message.IsEventUpdate()) { return; }

			var response = message.ParseResponse();
			var eventData = JsonSerializer.Deserialize<StreamElementsEvent>(response);

			if (eventData == null) { return; }

			if (!_eventSet.Contains(eventData.Id))
			{
				_eventSet.Add(eventData);
			}

			if (eventData.Type == EventType.Tip)
			{
				try
				{
					using var scope = ScopeFactory.CreateScope();
					var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

					await mediatr.Send(new SETipReceivedCommand(eventData));
				}
				catch (Exception ex)
				{
					Log.LogError(ex.Message);
				}
			}
		}
	}


	public async Task Connected() =>
		await Signalr.SendInfo("StreamElements Websocket Authenticated");

	private async Task Error(WebSocketMessage error)
	{
		//await Cache.UpdateServiceStatus(ServiceName.ObsWebsocket, false);
		await StreamElementsHandler.Disconnect();
	}

	private async Task SendPong()
		=> await StreamElementsHandler.SendMessage("2");

	private async Task OnUnauthorized() =>
		await Signalr.SendError("StreamElements Websocket Unauthorized");

	private async Task Disconnect()
	{
		await StreamElementsHandler.Disconnect();
	}


	public void SetCancellationToken(CancellationToken cancel)
		=> Cancel = cancel;

	// AUTHENTICATE

	private async Task Authenticate()
	{
		var jwt = Settings.Value.StreamElementsJwtToken;
		var message = new AuthenticationRequest(jwt);

		await StreamElementsHandler.SendMessage(message.ConvertToString());
	}

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

public class StreamElementsMessage
{
	public string Message { get; set; } = null!;
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IStreamElementsService
{
	void SetCancellationToken(CancellationToken cancel);
	public Task CreateWebSocket();
}