using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;
using Koishibot.Core.Features.Dandle.Models;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;

namespace Koishibot.Core.Features.Dandle;

public record DandleTimer(
	IAppCache Cache, ISignalrService Signalr,
	ITwitchIrcService BotIrc, ILogger<DandleTimer> Log,
	IDandleResultsProcessor DandleVoteProcessor
	) : IDandleTimer
{
	public int VotingSeconds = 40;

	public async Task StartSuggestionTimer(int round)
	{
		var text = "!Guess a Word";
		var SuggestionSeconds = 30;

		switch (round)
		{
			case 3:
				SuggestionSeconds = 40;
				break;
			case 4:
				SuggestionSeconds = 50;
				break;
			case 5:
			case 6:
				SuggestionSeconds = 60;
				break;
			default: break;
		}

		await Signalr.SendDandleTimer(new DandleTimerVm(text, 0, SuggestionSeconds));
		var timer = Toolbox.CreateTimer(TimeSpan.FromSeconds(SuggestionSeconds), () => CloseSuggestions());
		timer.Start();
	}

	public async void CloseSuggestions()
	{
		Cache.CloseDandleSuggestions();
		var dandleInfo = Cache.GetDandleInfo();

		dandleInfo.SelectWordCandidates();

		dandleInfo.ClearCurrentSuggestions();
		await Signalr.SendClearDandleSuggestions();

		if (dandleInfo.OnlyOneSuggestionSubmitted())
		{
			// add suggestor as the voter
			dandleInfo.AddSuggestorAsVoter();

			// submit word as suggested word
			Cache.UpdateDandle(dandleInfo);
			await DandleVoteProcessor.DetermineScore();
		}
		else
		{
			Cache.UpdateDandle(dandleInfo);
			Cache.OpenDandleVoting();

			var wordString = dandleInfo.CreateDandleWordString();
			await BotIrc.PostDandleNowVoting(wordString, VotingSeconds);

			await Signalr.SendDandleGuessChoices(dandleInfo.CreatePollChoiceVm());
			await Signalr.SendDandleTimer(new DandleTimerVm("Now Voting", 0, VotingSeconds));
			StartVotingTimer();
		}
	}

	public void StartVotingTimer()
	{
		var timer = Toolbox.CreateTimer(TimeSpan.FromSeconds(VotingSeconds), () => CloseVoting());
		timer.Start();
	}

	public async void CloseVoting()
	{
		Cache.CloseDandleVoting();
		await DandleVoteProcessor.DetermineScore();
	}
}

public interface IDandleTimer
{
	Task StartSuggestionTimer(int round);
}

