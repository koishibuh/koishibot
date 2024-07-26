using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.ChannelUpdate;

namespace Koishibot.Core.Features.StreamInformation.Events;

// == ⚫ EVENT SUB == //

//public record StreamUpdated(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//	) : IStreamUpdated
//{
//	public async Task SetupHandler()
//	{
//		EventSubClient.ChannelUpdate += OnChannelUpdated;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster("channel.update", "2", Settings);
//	}

//	private async Task OnChannelUpdated(object sender, ChannelUpdateArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new StreamUpdatedCommand(args));
//	}
//}

// == ⚫ COMMAND == //

//public record StreamUpdatedCommand
//	(ChannelUpdateArgs args) : IRequest;

//public record StreamUpdatedCommand
//	(EventMessage<ChannelUpdatedEvent> args) : IRequest;

public record StreamUpdatedCommand
	(ChannelUpdatedEvent args) : IRequest;


// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelupdate">Channel Update EventSub Documentation</see></para>
/// <para>Raised when there are changes in title, category, mature flag, broadcast, or language</para>
/// <para>Event sent to <see cref="StreamInfoUpdatedEventHandler"/></para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//public record StreamUpdatedHandler(
//	IAppCache Cache, ISignalrService Signalr,
//	ILogger<StreamUpdatedHandler> Log
//	) : IRequestHandler<StreamUpdatedCommand>
//{
//	public async Task Handle
//		(StreamUpdatedCommand command, CancellationToken cancellationToken)
//	{
//		var e = command.args.ConvertToModel();

//		Cache.UpdateStreamInfo(e);

//		var infoVm = e.ConvertToVm();
//		await Signalr.SendStreamInfo(infoVm);
//	}
//}

public record StreamUpdatedHandler(
	IAppCache Cache, ISignalrService Signalr,
	ILogger<StreamUpdatedHandler> Log
	) : IRequestHandler<StreamUpdatedCommand>
{
	public async Task Handle
		(StreamUpdatedCommand command, CancellationToken cancellationToken)
	{
		var e = command.args.BroadcasterUserId;
		var test = "";
		//var e = command.args.ConvertToModel();

		//Cache.UpdateStreamInfo(e);

		//var infoVm = e.ConvertToVm();
		//await Signalr.SendStreamInfo(infoVm);
	}
}