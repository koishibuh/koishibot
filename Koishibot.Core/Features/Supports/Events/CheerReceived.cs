using Koishibot.Core.Features.Supports.Extensions;
using Koishibot.Core.Features.Supports.Interfaces;
using Koishibot.Core.Features.Supports.Models;
using Koishibot.Core.Features.TwitchUsers.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.TwitchEventSub.Extensions;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
namespace Koishibot.Core.Features.Supports.Events;


// == ⚫ EVENT SUB == //

public class CheerReceived(
	IOptions<Settings> Settings,
	EventSubWebsocketClient EventSubClient,
	ITwitchAPI TwitchApi,
	IServiceScopeFactory ScopeFactory
	) : ICheerReceived
{
	public async Task SetupMethod()
	{
		EventSubClient.ChannelCheer += OnCheerReceived;
		await SubToEvent();
	}
	public async Task SubToEvent()
	{
		await TwitchApi.CreateEventSubBroadcaster
			("channel.cheer", "1", Settings);
	}

	private async Task OnCheerReceived(object sender, ChannelCheerArgs args)
	{
		using var scope = ScopeFactory.CreateScope();
		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

		await mediatr.Send(new CheerReceivedCommand(args));
	}
}

// == ⚫ COMMAND == //

public record CheerReceivedCommand(ChannelCheerArgs args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelcheer">Channel Cheer EventSub Documentation</see></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record CheerReceivedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ITwitchUserHub TwitchUserHub,
	KoishibotDbContext Database,
	ILogger<CheerReceivedHandler> Log
	) : IRequestHandler<CheerReceivedCommand>
{
	public async Task Handle
		(CheerReceivedCommand command, CancellationToken cancel)
	{
		Log.LogInformation(command.args.Notification.Payload.Event.Bits.ToString());
		var e = command.args.ConvertToDto();
		var amount = command.args.Notification.Payload.Event.Bits;
		var message = command.args.Notification.Payload.Event.Message;
		var user = await TwitchUserHub.Start(e);

		var cheer = new TwitchCheer().Initialize(user.Id, amount, message);
		await Database.AddCheer(cheer);

		var cheerVm = cheer.ConvertToVm(user.Name);
		await Signalr.SendStreamEvent(cheerVm);
	}
}