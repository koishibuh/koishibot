using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;

namespace Koishibot.Core.Features.Polls.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollprogress">Channel Poll Progress</see>
/// </summary>
public record VoteReceivedHandler(
IAppCache Cache,
ISignalrService Signalr
) : IVoteReceivedHandler
{
	public async Task Handle(PollProgressEvent e)
	{
		var poll = e.CreateDto();
		Cache.AddPoll(poll);

		// var pollVm = poll.ConvertToVm();
		// await Signalr.SendPoll(pollVm);

		//Update Overlay that vote was received
		if (e.IsRaidPoll())
		{
			// var raidPollVm = new PollVotesVm().Set(poll.Choices);
			// await Signalr.SendRaidPollVote(raidPollVm);
		}
		else
		{
			await Signalr.SendPollVote(poll.Choices);
		}
	}
}
/*═══════════════════【 EXTENSIONS 】═══════════════════*/

public static class PollProgressEventExtensions
{
	public static CurrentPoll CreateDto(this PollProgressEvent e)
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

	public static  bool IsRaidPoll(this PollProgressEvent e) =>
		e.Title == "Who should we raid?";
}


/*══════════════════【 INTERFACE 】══════════════════*/
public interface IVoteReceivedHandler
{
	Task Handle(PollProgressEvent e);
}