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
) : IPollStartedHandler
{
	public async Task Handle(PollBeginEvent command)
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

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class PollStartedExtensions
{
	public static CurrentPoll CreateDto(this PollBeginEvent e)
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

	public static PollVm CreateVm(this PollBeginEvent e)
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

	public static CurrentTimer CreateTimerDto(this PollBeginEvent e)
	{
		return new CurrentTimer
		{
			EndsAt = e.EndsAt,
			Title = e.Title
		};
	}

	public static bool IsRaidPoll(this PollBeginEvent e) =>
		e.Title == "Who should we raid?";
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IPollStartedHandler
{
	Task Handle(PollBeginEvent e);
}