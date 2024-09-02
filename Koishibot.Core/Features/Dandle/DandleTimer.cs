﻿using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;
using Koishibot.Core.Features.Dandle.Models;

namespace Koishibot.Core.Features.Dandle;

public record DandleTimer(
IAppCache Cache,
ISignalrService Signalr,
IChatReplyService ChatReplyService,
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
		var timer = Toolbox.CreateTimer(SuggestionSeconds, CloseSuggestions);
		timer.Start();
	}

	private async void CloseSuggestions()
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

			var data = new WordsTimeData(wordString, VotingSeconds);
			await ChatReplyService.App(Command.NowVoting, data);

			await Signalr.SendDandleGuessChoices(dandleInfo.CreatePollChoiceVm());
			await Signalr.SendDandleTimer(new DandleTimerVm("Now Voting", 0, VotingSeconds));
			StartVotingTimer();
		}
	}

	private void StartVotingTimer()
	{
		var timer = Toolbox.CreateTimer(VotingSeconds, CloseVoting);
		timer.Start();
	}

	private async void CloseVoting()
	{
		Cache.CloseDandleVoting();
		await DandleVoteProcessor.DetermineScore();
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IDandleTimer
{
	Task StartSuggestionTimer(int round);
}