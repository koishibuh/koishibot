using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Polls.Extensions;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Common;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
using Koishibot.Core.Services.TwitchApi.Models;

namespace Koishibot.Core.Features.Polls.Events;

// == ⚫ COMMAND == //

public record PollEndedCommand
	(PollEndedEvent args) : IRequest;

// == ⚫ HANDLER == //

/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Channel Poll End</see></para>
/// <para>When poll is completed and the banner is displaying at the top of chat, this triggers the OnPollEnd with the status: Completed</para>
/// <para>When the poll chat banner disappears, the OnPollEnd is triggered with the Status: Archived</para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record PollEndedHandler(
	IAppCache Cache,
		IOptions<Settings> Settings,
	ITwitchApiRequest TwitchApiRequest,
		ISignalrService Signalr,
		ITwitchIrcService BotIrc,
		IRaidPollProcessor RaidPollProcessor,
		KoishibotDbContext Database
		) : IRequestHandler<PollEndedCommand>
{
	public async Task Handle(PollEndedCommand command, CancellationToken cancel)
	{
		if (command.args.Status == PollStatus.Archived) { return; }

		if (command.args.Title == "Who should we raid?")
		{
			await RaidPollProcessor.Start(command.args);
			return;
		}

		var random = new Random();
		var sortedChoices = command.args.Choices
						 .OrderBy(x => random.Next())
						 .ToDictionary(x => x.Title, x => x.Votes);

		var winner = sortedChoices.MaxBy(p => p.Value);


		await BotIrc.PollResult(command.args.Title, winner.Key);

		var poll = new CurrentPoll
		{
			Id = command.args.PollId,
			Title = command.args.Title,
			StartedAt = command.args.StartedAt,
			EndingAt = command.args.EndedAt,
			Duration = (command.args.StartedAt - command.args.EndedAt),
			Choices = sortedChoices
		};

		Cache.AddPoll(poll);
		var pollVm = poll.ConvertToVm();
		await Signalr.SendPoll(pollVm);

		var streamSessionId = Cache.GetCurrentStreamId();



		var pollResult = ConvertToEntity(sortedChoices, command.args.Title, command.args.StartedAt, winner.Key, 1);
		await Database.AddPollResult(pollResult);

		// Todo: Update Overlay that poll ended 
	}

	public static Dictionary<string, int> AddVotesToDictionary(List<Choice> pollChoices)
	{
		var pollResults = new Dictionary<string, int>();
		foreach (var pollChoice in pollChoices)
		{
			pollResults[pollChoice.Title]
					= pollResults.TryGetValue(pollChoice.Title, out int currentVoteCount)
							? currentVoteCount + pollChoice.Votes
							: pollChoice.Votes;
			// ?? if null then 0; coalesce operator
		}

		return pollResults;
	}

	public PollResult ConvertToEntity(Dictionary<string, int> Choices, string title,
		DateTimeOffset startedAt, string winner, int streamSessionId)
	{
		var count = Choices.Count;

		switch (count)
		{
			case 2:
				Choices.Add("None 3", 0);
				Choices.Add("None 4", 0);
				Choices.Add("None 5", 0);
				break;
			case 3:
				Choices.Add("None 4", 0);
				Choices.Add("None 5", 0);
				break;
			case 4:
				Choices.Add("None 5", 0);
				break;

			default: break;
		}

		var results = Choices
						 .Select(p => new KeyValuePair<string, int>(p.Key, p.Value))
						 .ToList();

		return new PollResult
		{
			StartedAt = startedAt,
			TwitchStreamId = streamSessionId,
			Title = title,
			ChoiceOne = results[0].Key,
			VoteOne = results[0].Value,
			ChoiceTwo = results[1].Key,
			VoteTwo = results[1].Value,
			ChoiceThree = results[2].Key,
			VoteThree = results[2].Value,
			ChoiceFour = results[3].Key,
			VoteFour = results[3].Value,
			ChoiceFive = results[4].Key,
			VoteFive = results[4].Value,
			WinningChoice = winner
		};
	}


	// == ⚫ == //

	public record PollEndedEventModel(
					string Id,
					string Title,
					DateTimeOffset StartedAt,
					DateTimeOffset EndingAt,
					Dictionary<string, int> Choices) : INotification
	{
		public TimeSpan Duration => StartedAt - EndingAt;

		public bool IsRaidPoll()
		{
			return Title == "Who should we raid?";
		}


	}
}

public static class PollChatReply
{
	public static async Task PollResult(this ITwitchIrcService irc, string title, string winningChoice)
	{
		await irc.BotSend($"For '{title}', the koimmunity popular vote was {winningChoice}");
	}
}