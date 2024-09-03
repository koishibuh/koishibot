using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Features.RaidSuggestions.Models;

namespace Koishibot.Core.Services.SignalR;

/*═══════════════════【 DANDLE 】═══════════════════*/
public partial record SignalrService : ISignalrService
{
	public async Task SendDandleTimer(DandleTimerVm dandleTimerVm) =>
	await HubContext.Clients.All.ReceiveDandleTimer(dandleTimerVm);

	public async Task SendDandleMessage(string message) =>
	await HubContext.Clients.All.ReceiveDandleMessage(message);


	public async Task SendDandleWordSuggestion(DandleSuggestionVm wordSuggestionVm) =>
	await HubContext.Clients.All.ReceiveDandleWordSuggestion(wordSuggestionVm);

	public async Task SendDandleGuessChoices(List<PollChoiceInfo> dandleChoices) =>
	await HubContext.Clients.All.ReceiveDandleGuessChoices(dandleChoices);

	public async Task SendDandleVote(PollChoiceInfo pollChoiceInfo) =>
	await HubContext.Clients.All.ReceiveDandleVote(pollChoiceInfo);

	public async Task SendDandleWordGuess(DandleGuessedWordVm dandleWordGuessVm) =>
	await HubContext.Clients.All.ReceiveDandleWordGuess(dandleWordGuessVm);

	public async Task SendDandleScore(List<DandleUserVm> dandleUsers) =>
	await HubContext.Clients.All.ReceiveDandleScore(dandleUsers);


	public async Task SendClearDandleBoard() =>
	await HubContext.Clients.All.ReceiveClearDandleBoard();

	public async Task SendClearDandleSuggestions() =>
	await HubContext.Clients.All.ReceiveClearDandleSuggestions();
}

/*═════════════════【SEND INTERFACE 】═════════════════*/
public partial interface ISignalrService
{
	Task SendDandleTimer(DandleTimerVm dandleTimerVm);
	Task SendDandleMessage(string message);


	Task SendDandleWordSuggestion(DandleSuggestionVm wordSuggestionVm);
	Task SendDandleGuessChoices(List<PollChoiceInfo> dandleChoices);
	Task SendDandleVote(PollChoiceInfo pollChoiceInfo);
	Task SendDandleWordGuess(DandleGuessedWordVm dandleWordGuessVm);
	Task SendDandleScore(List<DandleUserVm> dandleUsers);


	Task SendClearDandleBoard();
	Task SendClearDandleSuggestions();
}

/*═══════════════【RECEIVE INTERFACE 】═══════════════*/
public partial interface ISignalrHub
{
	Task ReceiveDandleTimer(DandleTimerVm dandleTimerVm);
	Task ReceiveDandleMessage(string message);


	Task ReceiveDandleWordSuggestion(DandleSuggestionVm wordSuggestionVm);
	Task ReceiveDandleGuessChoices(List<PollChoiceInfo> dandleChoices);
	Task ReceiveDandleVote(PollChoiceInfo pollChoiceInfo);
	Task ReceiveDandleWordGuess(DandleGuessedWordVm dandleWordGuessVm);
	Task ReceiveDandleScore(List<DandleUserVm> dandleUsers);


	Task ReceiveClearDandleBoard();
	Task ReceiveClearDandleSuggestions();
}