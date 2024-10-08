using Koishibot.Core.Features.Supports.Events;
using Koishibot.Core.Services.StreamElements.Models;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Websockets;
using System.Text.Json;
using Koishibot.Core.Exceptions;
using Koishibot.Core.Persistence.Cache.Enums;
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
	private WebSocketFactory Factory { get; set; } = new();
	public WebSocketHandler? StreamElementsWebSocket { get; set; }

	private Timer _keepaliveTimer { get; } = new(TimeSpan.FromSeconds(25));

	private readonly LimitedSizeHashSet<StreamElementsEvent, string> _eventSet
		= new(10, x => x.Id);

	public async Task CreateWebSocket()
	{
		const string url = "wss://realtime.streamelements.com/socket.io/?cluster=main&EIO=3&transport=websocket";
		try
		{
			StreamElementsWebSocket = await Factory.Create(url, 3, ProcessMessage, Error, Closed);
		}
		catch (Exception e)
		{
			Log.LogError("An error has occured: {e}", e);
			await Signalr.SendError(e.Message);
			throw new CustomException(e.Message);
		}
	}

	private async Task ProcessMessage(WebSocketMessage message)
	{
		try
		{
			if (message.IsPing())
				await SendPong();

			else if (message.IsConnected())
				await Authenticate();

			else if (message.IsAuthenticated())
			{
				_eventSet.Add(new StreamElementsEvent { CreatedAt = DateTimeOffset.UtcNow });
				await Cache.UpdateServiceStatus(ServiceName.StreamElements, Status.Online);
				StartKeepaliveTimer();
			}

			else if (message.IsUnauthorized())
				await OnUnauthorized();

			else if (message.IsEvent())
			{
				if (message.IsEventUpdate()) { return; }

				var response = message.ParseResponse();

				var eventData = JsonSerializer.Deserialize<StreamElementsEvent>(response);
				if (eventData == null)
					throw new NullException("StreamElements EventData was null");

				if (!_eventSet.Contains(eventData.Id))
				{
					_eventSet.Add(eventData);
				}

				if (eventData.Type == "Tip")
				{
					using var scope = ScopeFactory.CreateScope();
					var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

					await mediatr.Send(new SETipReceivedCommand(eventData));
				}
			}
		}
		catch (Exception e)
		{
			Log.LogError("An error has occured: {e}", e);
			await Signalr.SendError(e.Message);
		}
	}

	public async Task Connected() =>
		await Signalr.SendInfo("StreamElements Websocket Authenticated");



	private async Task SendPong()
		=> await StreamElementsWebSocket.SendMessage("2");

	private async Task OnUnauthorized() =>
		await Signalr.SendError("StreamElements Websocket Unauthorized");

	private async Task Error(WebSocketMessage message)
	{
		Log.LogError("Websocket error: {message}", message);
		await Signalr.SendError(message.Message);
		if (StreamElementsWebSocket is not null && StreamElementsWebSocket.IsDisposed is false)
		{
			await Disconnect();
		}
	}

	private async Task Closed(WebSocketMessage message)
	{
		Log.LogInformation($"StreamElements Websocket closed {message.Message}");
		if (StreamElementsWebSocket?.IsDisposed is false)
		{
			await Disconnect();
		}
	}

	private async Task Disconnect()
	{
		_keepaliveTimer.Stop();
		await Factory.Disconnect();
		await Cache.UpdateServiceStatus(ServiceName.StreamElements, Status.Offline);
		await Signalr.SendInfo("StreamElements Websocket Disconnected");
		// TODO: Proper retry
		await CreateWebSocket();

	}

	public void SetCancellationToken(CancellationToken cancel)
		=> Cancel = cancel;

	// AUTHENTICATE

	private async Task Authenticate()
	{
		var jwt = Settings.Value.StreamElementsJwtToken;
		var message = new AuthenticationRequest(jwt);

		await StreamElementsWebSocket.SendMessage(message.ConvertToString());
	}

	private void StartKeepaliveTimer()
	{
		_keepaliveTimer.Elapsed += async (_, _) =>
		{
			await SendPong();
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