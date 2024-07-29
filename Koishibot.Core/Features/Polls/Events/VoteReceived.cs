using Koishibot.Core.Features.Polls.Extensions;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
namespace Koishibot.Core.Features.Polls.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollprogress">Channel Poll Progress</see>
/// </summary>
public record VoteReceivedHandler(
	IAppCache Cache,
	ITwitchIrcService BotIrc,
	ISignalrService Signalr
	) : IRequestHandler<PollVoteReceivedCommand>
{
	public async Task Handle(PollVoteReceivedCommand command, CancellationToken cancel)
	{
		var poll = command.CreateDto();
		Cache.AddPoll(poll);

		var pollVm = poll.ConvertToVm();
		await Signalr.SendPoll(pollVm);

		//Update Overlay that vote was received
		if (command.IsRaidPoll())
		{
			var raidPollVm = new RaidPollVm().Set(poll.Choices);
			await Signalr.SendRaidPollVote(raidPollVm);
		}
	}
}

// == ⚫  == //

public record VoteReceivedEvent(
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

// == ⚫ COMMAND == //

public record PollVoteReceivedCommand
	(PollProgressEvent e) : IRequest
{
	public CurrentPoll CreateDto()
	{
		var pollChoices = e.Choices?
		.GroupBy(choice => choice.Title)
		.ToDictionary(group => group.Key, group => group.Sum(choice => choice.Votes));

		return new CurrentPoll
		{
			Id = e.PollId,
			Title = e.Title,
			StartedAt = e.StartedAt,
			EndingAt = e.EndsAt,
			Duration = e.EndsAt - e.StartedAt,
			Choices = pollChoices
		};
	}
	public bool IsRaidPoll()
	{
		return e.Title == "Who should we raid?";
	}

};

