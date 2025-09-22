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
) : IRequestHandler<PollEndedCommand>
{
	public async Task Handle(PollEndedCommand command, CancellationToken cancel)
	{
		ClearPollCache();

		if (command.PollStatusIsArchived())
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

		command.RankChoices();
		command.DetermineWinner();

		await PostChatResponse(command);

		// var pollVm = poll.ConvertToVm();
		// await Signalr.SendPoll(pollVm);

		await AddResultToDatabase(command);

		await Signalr.SendPollEnded(command.winner.Choice);
	}

	private async Task PostChatResponse(PollEndedCommand command)
	{
		var data = command.CreateTemplate();
		await ChatReplyService.CreateResponse(Response.PollWinner, data);
	}

	private void ClearPollCache() => Cache.Remove(CacheName.CurrentPoll);

	private async Task AddResultToDatabase(PollEndedCommand command)
	{
		var pollResult = command.CreateEntity();
		await Database.UpdateEntry(pollResult);
	}
}

/*═══════════════════【 COMMAND 】═══════════════════*/
public record PollEndedCommand(PollEndedEvent args) : IRequest
{
	public List<PollChoiceInfo> rankedChoices = [];
	public PollChoiceInfo? winner;

	public bool PollStatusIsArchived() => args.Status == PollStatus.Archived;
	public bool PollIsRaid() => args.Title == "Who should we raid?";

	public void RankChoices()
	{
		var random = new Random();
		rankedChoices = args.Choices
			.OrderByDescending(x => x.Votes)
			.ThenBy(x => random.Next())
			.Select(x => new PollChoiceInfo(x.Title, x.Votes))
			.ToList();
	}

	public void DetermineWinner() =>
		winner = rankedChoices.FirstOrDefault();

	public CurrentPoll CreatePoll() =>
		new()
		{
			Id = args.PollId,
			Title = args.Title,
			StartedAt = args.StartedAt,
			EndingAt = args.EndedAt,
			Duration = (args.StartedAt - args.EndedAt),
			Choices = rankedChoices
		};

	public PollResult CreateEntity() => new()
	{
		StartedAt = args.StartedAt,
		Title = args.Title,
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


	public object CreateTemplate() => new { Title = args.Title, Winner = winner.Choice };
}