using Koishibot.Core.Features.Polls.Events;
using Koishibot.Core.Features.Polls.Extensions;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.Raids.Enums;
using Koishibot.Core.Features.RaidSuggestions.Extensions;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
namespace Koishibot.Core.Features.RaidSuggestions;

// == ⚫ == //

public record RaidPollProcessor(
		IAppCache Cache, IChatMessageService BotIrc,
		ISignalrService Signalr
		) : IRaidPollProcessor
{
	public async Task Start(PollEndedEvent e)
	{
		var random = new Random();

		var pollResults = e.Choices
			.OrderByDescending(pr => pr.Value)
			.ThenBy(pr => random.Next())
			.Select(pr => pr.Key)
			.ToList();

		var raidTargetName = pollResults.First();
		pollResults.Remove(raidTargetName);

		var poll = new CurrentPoll().ConvertFromEvent(e);

		Cache.AddPoll(poll);

		var raidCandidates = Cache.GetRaidCandidates();

		var raidTarget = raidCandidates.GetCandidateByName(raidTargetName);

		raidCandidates.Remove(raidTarget);

		Cache.AddRaidTarget(raidTarget);

		// TODO: Have it display the raid poll winner on overlay

		await BotIrc.RaidTarget
				(Code.WinningRaidTarget, raidTarget.SuggestedByUser.Name, raidTarget.Streamer.Name);

		await Signalr.SendRaidOverlayStatus(false);
	}
}
