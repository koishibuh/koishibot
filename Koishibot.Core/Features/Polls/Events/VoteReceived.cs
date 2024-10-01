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
) : IRequestHandler<PollVoteReceivedCommand>
{
	public async Task Handle(PollVoteReceivedCommand command, CancellationToken cancel)
	{
		var poll = command.CreateDto();
		Cache.AddPoll(poll);

		// var pollVm = poll.ConvertToVm();
		// await Signalr.SendPoll(pollVm);

		//Update Overlay that vote was received
		if (command.IsRaidPoll())
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

/*═══════════════════【 COMMAND 】═══════════════════*/
public record PollVoteReceivedCommand(PollProgressEvent e) : IRequest
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

	public bool IsRaidPoll() => e.Title == "Who should we raid?";
}