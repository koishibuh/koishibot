//using Koishibot.Core.Features.AdBreak.Extensions;
//using Koishibot.Core.Features.Common.Models;
//using Koishibot.Core.Features.Polls.Extensions;
//using Koishibot.Core.Features.Polls.Interfaces;
//using Koishibot.Core.Features.Polls.Models;
//using Koishibot.Core.Features.RaidSuggestions.Models;
//using Koishibot.Core.Services.TwitchEventSub.Extensions;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
//namespace Koishibot.Core.Features.Polls.Events;

//// == ⚫ EVENT SUB == //

//public class PollStarted(
//		IOptions<Settings> Settings,
//		EventSubWebsocketClient EventSubClient,
//		ITwitchAPI TwitchApi,
//		IServiceScopeFactory ScopeFactory
//		) : IPollStarted
//{
//	public async Task SetupHandler()
//	{
//		EventSubClient.ChannelPollBegin += OnPollStarted;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster("channel.poll.begin", "1", Settings);
//	}

//	private async Task OnPollStarted(object sender, ChannelPollBeginArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new PollStartedCommand(args));
//	}

//}

//// == ⚫ COMMAND == //

//public record PollStartedCommand
//	(ChannelPollBeginArgs args) : IRequest;


//// == ⚫ HANDLER == //

///// <summary>
///// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollbegin">Channel Poll Begin</see>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record PollStartedHandler
//		(IAppCache Cache, IChatMessageService BotIrc,
//		ISignalrService Signalr
//		) : IRequestHandler<PollStartedCommand>
//{
//	public async Task Handle(PollStartedCommand command, CancellationToken cancel)
//	{
//		var e = command.args.ConvertToEvent();

//		var poll = new CurrentPoll().ConvertFromEvent(e);
//		Cache.AddPoll(poll);

//		var pollVm = poll.ConvertToVm();
//		await Signalr.SendPoll(pollVm);

//		await BotIrc.BotSend($"A {e.Duration:mm\\:ss} poll has started: {e.Title}!");

//		if (e.IsRaidPoll())
//		{
//			var timer = new CurrentTimer().SetPoll(e);
//			Cache.AddCurrentTimer(timer);
//			var timerVm = timer.ConvertToVm();
//			await Signalr.UpdateTimerOverlay(timerVm);

//			var raidPollVm = new RaidPollVm().Set(e.Choices);
//			await Signalr.SendRaidPollVote(raidPollVm);
//		}
//	}	
//}

//public record PollStartedEvent(
//	string Id,
//	string Title,
//	DateTimeOffset StartedAt,
//	DateTimeOffset EndingAt,
//	Dictionary<string, int> Choices) : INotification
//{
//	public TimeSpan Duration => StartedAt - EndingAt;

//	public bool IsRaidPoll()
//	{
//		return Title == "Who should we raid?";
//	}
//}