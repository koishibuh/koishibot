using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Supports.Events;
using Koishibot.Core.Services.StreamElements.Models;
namespace Koishibot.Core.Services.StreamElements;

public record StreamElementsService(
	ILogger<StreamElementsService> Log,
	IOptions<Settings> Settings,
	IAppCache Cache,
	IServiceScopeFactory ScopeFactory,
	ISignalrService Signalr) : IStreamElementsService
{
	public CancellationToken? Cancel { get; set; }
	public StreamElementsWebSocket? StreamElements { get; set; }

	public async Task CreateWebSocket(CancellationToken cancel)
	{
		Cancel ??= cancel;

		StreamElements ??= new StreamElementsWebSocket(
			"wss://realtime.streamelements.com/socket.io/?cluster=main&EIO=3&transport=websocket",
			Settings,	Cancel.Value, 3);

		StreamElements.OnAuthenticated += async () => await OnAuthenticated();
		StreamElements.OnUnauthorized += async () => await OnUnauthorized();
		StreamElements.OnDisconnectError += async () => await OnDisconnected();

		StreamElements.OnTipReceived += async message => await OnTipReceived(message);

		await StreamElements.ConnectWebSocket();
	}

	public async Task OnAuthenticated() => await Signalr.SendLog
		(new LogVm("StreamElements Websocket Authenticated", "Info"));

	public async Task OnUnauthorized() => await Signalr.SendLog
		(new LogVm("StreamElements Websocket Unauthorized", "Info"));

	private async Task OnDisconnected() => await Signalr.SendLog
		(new LogVm("StreamElements Websocket Disconnected", "Info"));

	public async Task OnTipReceived(StreamElementsEvent e)
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(new SETipReceivedCommand(e));
	}
}

// == ⚫ INTERFACE == //

public interface IStreamElementsService
{
	public Task CreateWebSocket(CancellationToken cancel);
}