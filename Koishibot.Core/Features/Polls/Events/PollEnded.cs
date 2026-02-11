using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Extensions;
using Koishibot.Core.Features.Polls.Models;
using Koishibot.Core.Features.RaidSuggestions.Enums;
using Koishibot.Core.Features.RaidSuggestions.Interfaces;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using Koishibot.Core.Services.Twitch.Enums;
using Koishibot.Core.Services.Twitch.EventSubs.ResponseModels.Polls;

namespace Koishibot.Core.Features.Polls.Events;

/*═══════════════════【 HANDLER 】═══════════════════*/
/// <summary>
/// <para><see href="https://dev.twitch.tv/docs/eventsub/eventsub-subscription-types/#channelpollend">Channel Poll End</see></para>
/// <para>When poll is completed and the banner is displaying at the top of chat, this triggers the OnPollEnd with the status: Completed</para>
/// <para>When the poll chat banner disappears, the OnPollEnd is triggered with the Status: Archived</para>
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
public record PollEndedHandler(
IAppCache Cache,
ISignalrService Signalr,
IRaidPollProcessor RaidPollProcessor,
IChatReplyService ChatReplyService,
KoishibotDbContext Database
) : IPollEndedHandler
{
	public async Task Handle(PollEndedEvent e)
	{
		ClearPollCache();

		if (e.PollStatusIsArchived())
		{
			// When poll is cancelled or hides from Twitch chat
			await Signalr.SendPollCancelled();
			return;
		}

		// if (command.PollIsRaid())
		// {
		// 	await RaidPollProcessor.Start(command.args);
		// 	return;
		// }

		var rankedChoices = e.RankChoices();
		var winner = rankedChoices.FirstOrDefault();

		var template = new { Title = e.Title, Winner = winner };
		await ChatReplyService.CreateResponse(Response.PollWinner, template);

		// var pollVm = poll.ConvertToVm();
		// await Signalr.SendPoll(pollVm);

		var pollResult = new PollResult()
		{
			StartedAt = e.StartedAt,
			Title = e.Title,
			ChoiceOne = rankedChoices[0].Choice,
			VoteOne = rankedChoices[0].VoteCount,
			ChoiceTwo = rankedChoices[1].Choice,
			VoteTwo = rankedChoices[1].VoteCount,
			ChoiceThree = rankedChoices.Count >= 3 ? rankedChoices[2].Choice : null,
			VoteThree = rankedChoices.Count >= 3 ? rankedChoices[2].VoteCount : null,
			ChoiceFour = rankedChoices.Count >= 4 ? rankedChoices[3].Choice : null,
			VoteFour = rankedChoices.Count >= 4 ? rankedChoices[3].VoteCount : null,
			ChoiceFive = rankedChoices.Count >= 5 ? rankedChoices[4].Choice : null,
			VoteFive = rankedChoices.Count >= 5 ? rankedChoices[4].VoteCount : null,
			WinningChoice = winner.Choice
		};

		await Database.UpdateEntry(pollResult);
		await Signalr.SendPollEnded(winner.Choice);
	}

	private void ClearPollCache() => Cache.Remove(CacheName.CurrentPoll);
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class PollEndedEventExtensions
{
	public static bool PollStatusIsArchived(this PollEndedEvent e) =>
		e.Status == PollStatus.Archived;

	public static bool PollIsRaid(this PollEndedEvent e) =>
		e.Title == "Who should we raid?";

	public static List<PollChoiceInfo> RankChoices(this PollEndedEvent e)
	{
		var random = new Random();
		return e.Choices
			.OrderByDescending(x => x.Votes)
			.ThenBy(x => random.Next())
			.Select(x => new PollChoiceInfo(x.Title, x.Votes))
			.ToList();
	}

	public static CurrentPoll CreatePoll(this PollEndedEvent e, List<PollChoiceInfo> rankedChoices) =>
		new()
		{
			Id = e.PollId,
			Title = e.Title,
			StartedAt = e.StartedAt,
			EndingAt = e.EndedAt,
			Duration = (e.StartedAt - e.EndedAt),
			Choices = rankedChoices
		};

	public static object CreateTemplate(this PollEndedEvent e, string winner)
		=> new { Title = e.Title, Winner = winner };
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IPollEndedHandler
{
	Task Handle(PollEndedEvent e);
}