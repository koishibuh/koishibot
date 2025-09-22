using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;
namespace Koishibot.Core.Features.RaidSuggestions;

// == ⚫ == //

public record RaidPollProcessor(
		IAppCache Cache,
		IChatReplyService ChatReplyService,
		ISignalrService Signalr
		) : IRaidPollProcessor
{
	public async Task Start(PollEndedEvent e)
	{
		var random = new Random();

		var pollResults = e.Choices
			.OrderByDescending(pr => pr.Votes)
			.ThenBy(pr => random.Next())
			.Select(pr => pr.Title)
			.ToList();

		var raidTargetName = pollResults.First();
		pollResults.Remove(raidTargetName);

		var pollChoices = e.Choices?
			.GroupBy(choice => choice.Title)
			.Select(x => new PollChoiceInfo(x.Key, x.Sum(y => y.Votes)))
			.ToList();

		
		var poll = new CurrentPoll{
			Id = e.PollId, 
			Title = e.Title, 
			StartedAt = e.StartedAt, 
			EndingAt = e.EndedAt,
			Duration = (e.StartedAt - e.EndedAt),
			Choices = pollChoices
		};

		Cache.AddPoll(poll);

		var raidCandidates = Cache.GetRaidCandidates();

		var raidTarget = raidCandidates.GetCandidateByName(raidTargetName);

		raidCandidates.Remove(raidTarget);

		Cache.AddRaidTarget(raidTarget);

		// TODO: Have it display the raid poll winner on overlay

		var data = new UserStreamerData(raidTarget.SuggestedByUser.Name, raidTarget.Streamer.Name);

		await ChatReplyService.CreateResponse(Response.RaidTarget, data);

		await Signalr.SendRaidOverlayStatus(false);
	}
}