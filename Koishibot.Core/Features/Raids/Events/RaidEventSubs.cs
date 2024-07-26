//using Koishibot.Core.Services.TwitchEventSub.Extensions;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;

//namespace Koishibot.Core.Features.Raids.Events;

//public class RaidEventSubs(
//	IOptions<Settings> Settings,
//	EventSubWebsocketClient EventSubClient,
//	ITwitchAPI TwitchApi,
//	IServiceScopeFactory ScopeFactory
//	) : IRaidEventSubs
//{
//	public async Task SetupAndSubscribe()
//	{
//		EventSubClient.ChannelRaid += OnChannelRaid;
//		await SubscribeToEvents();
//	}

//	public async Task SubscribeToEvents()
//	{
//		await TwitchApi.CreateEventSubToBroadcaster("channel.raid", "1", Settings);
//		await TwitchApi.CreateEventSubFromBroadcaster("channel.raid", "1", Settings);
//	}

//	private async Task OnChannelRaid(object sender, ChannelRaidArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		if (args.IsIncomingRaid())
//		{
//			await mediatr.Send(new IncomingRaidCommand(args));
//		}
//		else // Outgoing Raid
//		{
//			// await mediatr.Send(new OutgoingRaidCommand(args));
//		}
//	}
//}
