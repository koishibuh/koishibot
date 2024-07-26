//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Polls.Extensions;
//using Koishibot.Core.Features.Polls.Interfaces;
//using Koishibot.Core.Features.Polls.Models;
//using Koishibot.Core.Features.RaidSuggestions.Interfaces;
//using Koishibot.Core.Persistence;
//using Koishibot.Core.Services.TwitchEventSub.Extensions;
//using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
//namespace Koishibot.Core.Features.Polls.Events;

//// == ⚫ EVENT SUB == //

//public class PollEnded(
//		IOptions<Settings> Settings,
//		EventSubWebsocketClient EventSubClient,
//		ITwitchAPI TwitchApi,
//		IServiceScopeFactory ScopeFactory
//		) : IPollEnded
//{
//	public async Task SetupHandler()
//	{
//		EventSubClient.ChannelPollEnd += OnPollEnded;
//		await SubToEvent();
//	}

//	public async Task SubToEvent()
//	{
//		await TwitchApi.CreateEventSubBroadcaster("channel.poll.end", "1", Settings);
//	}

//	public async Task OnPollEnded(object sender, ChannelPollEndArgs args)
//	{
//		using var scope = ScopeFactory.CreateScope();
//		var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

//		await mediatr.Send(new PollEndedCommand(args));
//	}
//}

//// == ⚫ COMMAND == //

//public record PollEndedCommand
//	(ChannelPollEndArgs args) : IRequest;

//// == ⚫ HANDLER == //

///// <summary>
///// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Channel Poll End</see></para>
///// <para>When poll is completed and the banner is displaying at the top of chat, this triggers the OnPollEnd with the status: Completed</para>
///// <para>When the poll chat banner disappears, the OnPollEnd is triggered with the Status: Archived</para>
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//public record PollEndedHandler
//		(IAppCache Cache, IChatMessageService BotIrc,
//		ISignalrService Signalr, IRaidPollProcessor RaidPollProcessor,
//		KoishibotDbContext Database
//		) : IRequestHandler<PollEndedCommand>
//{
//	public async Task Handle(PollEndedCommand command, CancellationToken cancel)
//	{
//		var e = command.args.ConvertToEvent();

//		if (command.args.IsPollStatusArchive())
//		{
//			return;
//		}

//		if (e.IsRaidPoll())
//		{
//			await RaidPollProcessor.Start(e);
//			return;
//		}

//		var winner = e.DeterminePollWinner();

//		await BotIrc.PollResult(e.Title, winner);

//		var poll = new CurrentPoll();
//		poll.ConvertFromEvent(e);

//		Cache.AddPoll(poll);
//		var pollVm = poll.ConvertToVm();
//		await Signalr.SendPoll(pollVm);

//		var streamSessionId = Cache.GetCurrentStreamId();

//		var pollResult = e.ConvertToEntity(winner, 1);
//		await Database.AddPollResult(pollResult);

//		// Todo: Update Overlay that poll ended 
//	}
//}


//// == ⚫ == //

//public record PollEndedEvent(
//				string Id,
//				string Title,
//				DateTimeOffset StartedAt,
//				DateTimeOffset EndingAt,
//				Dictionary<string, int> Choices) : INotification
//{
//	public TimeSpan Duration => StartedAt - EndingAt;

//	public bool IsRaidPoll()
//	{
//		return Title == "Who should we raid?";
//	}

//	public string DeterminePollWinner()
//	{
//		var random = new Random();
//		var sortedChoices = Choices
//						 .OrderBy(x => random.Next())
//						 .ToDictionary(x => x.Key, x => x.Value);

//		var winner = sortedChoices.MaxBy(p => p.Value);
//		return winner.Key;
//	}

//	public PollResult ConvertToEntity(string winner, int streamSessionId)
//	{
//		var count = Choices.Count;

//		switch (count)
//		{
//			case 2:
//				Choices.Add("None 3", 0);
//				Choices.Add("None 4", 0);
//				Choices.Add("None 5", 0);
//				break;
//			case 3:
//				Choices.Add("None 4", 0);
//				Choices.Add("None 5", 0);
//				break;
//			case 4:
//				Choices.Add("None 5", 0);
//				break;

//			default: break;
//		}

//		var results = Choices
//						 .Select(p => new KeyValuePair<string, int>(p.Key, p.Value))
//						 .ToList();

//		return new PollResult
//		{
//			StartedAt = StartedAt,
//			TwitchStreamId = streamSessionId,
//			Title = Title,
//			ChoiceOne = results[0].Key,
//			VoteOne = results[0].Value,
//			ChoiceTwo = results[1].Key,
//			VoteTwo = results[1].Value,
//			ChoiceThree = results[2].Key,
//			VoteThree = results[2].Value,
//			ChoiceFour = results[3].Key,
//			VoteFour = results[3].Value,
//			ChoiceFive = results[4].Key,
//			VoteFive = results[4].Value,
//			WinningChoice = winner
//		};
//	}
//}

//public static class PollChatReply
//{
//	public static async Task PollResult(this IChatMessageService irc, string title, string winningChoice)
//	{
//		await irc.BotSend($"For '{title}', the koimmunity popular vote was {winningChoice}");
//	}
//}