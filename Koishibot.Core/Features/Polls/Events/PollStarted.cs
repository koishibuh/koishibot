using Koishibot.Core.Features.AdBreak.Extensions;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Polls.Extensions;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
namespace Koishibot.Core.Features.Polls.Events;

// == ⚫ HANDLER == //

/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollbegin">Channel Poll Begin</see>
/// </summary>
public record PollStartedHandler(
	IAppCache Cache, ITwitchIrcService BotIrc,
	ISignalrService Signalr
	) : IRequestHandler<PollStartedCommand>
{
	public async Task Handle(PollStartedCommand command, CancellationToken cancel)
	{
		var poll = command.CreateDto();
		Cache.AddPoll(poll);

		var pollVm = poll.ConvertToVm();
		await Signalr.SendPoll(pollVm);

		await BotIrc.BotSend($"A {poll.Duration:mm\\:ss} poll has started: {poll.Title}!");

		if (command.IsRaidPoll())
		{
			var timer = command.CreateTimerDto();
			Cache.AddCurrentTimer(timer);
			var timerVm = timer.ConvertToVm();
			await Signalr.UpdateTimerOverlay(timerVm);

			var raidPollVm = new RaidPollVm().Set(poll.Choices);
			await Signalr.SendRaidPollVote(raidPollVm);
		}
	}
}


// == ⚫ COMMAND == //

public record PollStartedCommand(PollBeginEvent e) : IRequest
{
	public CurrentPoll CreateDto()
	{
		var pollChoices = e.Choices?
		.	GroupBy(choice => choice.Title)
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

	public CurrentTimer CreateTimerDto()
	{
		return new CurrentTimer
		{
			EndsAt = e.EndsAt,
			Title = e.Title
		};
	}

	public bool IsRaidPoll()
	{
		return e.Title == "Who should we raid?";
	}
};




public record PollStartedEventModel(
	string Id,
	string Title,
	DateTimeOffset StartedAt,
	DateTimeOffset EndingAt,
	Dictionary<string, int> Choices)
{ 
	public TimeSpan Duration => StartedAt - EndingAt;

	public bool IsRaidPoll()
	{
		return Title == "Who should we raid?";
	}
}