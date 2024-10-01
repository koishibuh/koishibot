using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;
using Koishibot.Core.Services.Twitch.Irc;
namespace Koishibot.Core.Features.Polls.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollbegin">Channel Poll Begin</see>
/// </summary>
public record PollStartedHandler(
IAppCache Cache,
ITwitchIrcService BotIrc,
ISignalrService Signalr
) : IRequestHandler<PollStartedCommand>
{
	public async Task Handle(PollStartedCommand command, CancellationToken cancel)
	{
		var poll = command.CreateDto();
		Cache.AddPoll(poll);

		await BotIrc.BotSend($"A {poll.Duration:mm\\:ss} poll has started: {poll.Title}!");

		if (command.IsRaidPoll())
		{
			// var timer = command.CreateTimerDto();
			// Cache.AddCurrentTimer(timer);
			//
			// var timerVm = timer.ConvertToVm();
			// await Signalr.UpdateTimerOverlay(timerVm);

			// var raidPollVm = new PollVotesVm().Set(poll.Choices);
			// await Signalr.SendRaidPollVote(raidPollVm);
		}
		else
		{
			var pollVm1 = command.CreateVm();
			await Signalr.SendPollStarted(pollVm1);
		}
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record PollStartedCommand(PollBeginEvent e) : IRequest
{
	public CurrentPoll CreateDto()
	{
		var pollChoices = e.Choices?
			.GroupBy(choice => choice.Title)
			.Select(x => new PollChoiceInfo(x.Key, x.Sum(y => y.Votes)))
			.ToList();

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

	public PollVm CreateVm()
	{
		var pollChoices = e.Choices?
			.GroupBy(choice => choice.Title)
			.Select(x => new PollChoiceInfo(x.Key, x.Sum(y => y.Votes)))
			.ToList();

		return new PollVm(
			e.PollId,
			e.Title,
			e.StartedAt,
			e.EndsAt,
			e.EndsAt - e.StartedAt,
			pollChoices
			);
	}

	public CurrentTimer CreateTimerDto()
	{
		return new CurrentTimer
		{
			EndsAt = e.EndsAt,
			Title = e.Title
		};
	}

	public bool IsRaidPoll() => e.Title == "Who should we raid?";
};