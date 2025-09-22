using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;
using Koishibot.Core.Features.Dandle.Models;

namespace Koishibot.Core.Features.Dandle;

// TODO: Finish scoring logic
public record DandleResultsProcessor(
IAppCache Cache,
ISignalrService Signalr,
IChatReplyService ChatReplyService,
ILogger<DandleResultsProcessor> Log,
IDandleWordService DandleWordService,
IDandleService DandleService
) : IDandleResultsProcessor
{
	string blue = "#44a8ff, #44a8ff";
	string orange = "#f5793a, #f5793a";
	string gray = "#787c7e, #787c7e";
	string split = "54deg, #f5793a 50%, #44a8ff 50%";

	private readonly Dictionary<string, string> _colorEmoji = new()
	{
		{ "#44a8ff, #44a8ff", "🟦" },
		{ "#f5793a, #f5793a", "🟧" },
		{ "#787c7e, #787c7e", "⬛" }
	};

	/*══════════════════【 Scoring 】══════════════════*/
	public async Task DetermineScore()
	{
		var dandleInfo = Cache.GetDandleInfo();
		dandleInfo.GetWinningSuggestion();

		dandleInfo = ProcessAltWords(dandleInfo);
		dandleInfo = ProcessGuessedWord(dandleInfo);

		dandleInfo = await GetBoardColors(dandleInfo);

		if (dandleInfo.GuessedWordIsTargetWord())
		{
			await GameWon(dandleInfo);
		}
		else if (dandleInfo.WasFinalRound())
		{
			await GameLost(dandleInfo);
		}
		else
		{
			await StartNextRound(dandleInfo);
		}
	}

	/*══════════════════【  】══════════════════*/
	private async Task StartNextRound(DandleGame dandleInfo)
	{
		var scores = dandleInfo.CreateDandleUserVmList();

		// foreach (var score in scores)
		// {
		// 	Log.LogInformation($"{score.Username}: {score.Points} | {score.BonusPoints}");
		// }

		await Signalr.SendDandleScore(scores);

		dandleInfo.IncreaseGameRound();

		Cache.UpdateDandle(dandleInfo);

		var timer = Toolbox.CreateTimer(5, async () => await AnnounceGuessOpen());
		timer.Start();
	}

	private async Task AnnounceGuessOpen()
	{
		await Signalr.SendDandleTimer(new DandleTimerVm("!Guess A Word", 0, 0));
		Cache.OpenDandleSuggestions();

		var dandleInfo = Cache.GetDandleInfo();

		var data = new NumberData(dandleInfo.GameRound);
		await ChatReplyService.CreateResponse(Response.NextRound, data);
	}

	private async Task GameLost(DandleGame dandleInfo)
	{
		// game lost
		// display score
		// add bonus points
		var scores = dandleInfo.CreateDandleUserVmList();

		// foreach (var score in scores)
		// {
		// 	Log.LogInformation($"{score.Username}: {score.Points} | {score.BonusPoints}");
		// }

		await Signalr.SendDandleScore(scores);
		await Signalr.SendDandleTimer(new DandleTimerVm("Better luck next time!", 0, 0));

		var data = new WordData(dandleInfo.TargetWord.Word);
		await ChatReplyService.CreateResponse(Response.GameLost, data);

		await DandleWordService.DefineWord(dandleInfo.TargetWord.Word);

		Cache.DisableDandle();
		Cache.ResetDandle();
	}

	private async Task GameWon(DandleGame dandleInfo)
	{
		// if so, end game and display scores
		//	bonus points for round?
		// display score
		// add bonus points

		var scores = dandleInfo.CreateDandleUserVmList();

		// foreach (var score in scores)
		// {
		// 	Log.LogInformation($"{score.Username}: {score.Points} | {score.BonusPoints}");
		// }

		await Signalr.SendDandleScore(scores);
		await Signalr.SendDandleTimer(new DandleTimerVm("Solved!", 0, 0));

		var data = new WordData(dandleInfo.TargetWord.Word);
		await ChatReplyService.CreateResponse(Response.SolvedWord, data);

		await DandleWordService.DefineWord(dandleInfo.TargetWord.Word);

		Cache.DisableDandle();
		Cache.ResetDandle();
	}

	/*══════════════════【 PROCESSING 】══════════════════*/

	private DandleGame ProcessGuessedWord(DandleGame dandleInfo)
	{
		foreach (var voter in dandleInfo.CurrentGuessedWord.Voters)
		{
			var user = dandleInfo.GetDandleUserPoints(voter);
			var targetWord = dandleInfo.GetTargetWord();
			var guessedWord = dandleInfo.CurrentGuessedWord?.GetGuessedWord();

			MatchLettersForColors(targetWord, guessedWord);
			CalculateUserScore(targetWord, guessedWord, user, dandleInfo.GameRound, isAltWord: false);
		}

		var (userId, points) = dandleInfo.CurrentGuessedWord.GetPointsForVotes();
		dandleInfo.AddPointsByUserId(userId, points);
		return dandleInfo;
	}

	private DandleGame ProcessAltWords(DandleGame dandleInfo)
	{
		foreach (var word in dandleInfo.CurrentVotes)
		{
			foreach (var voter in word.Voters)
			{
				var user = dandleInfo.GetDandleUserPoints(voter);
				var targetWord = dandleInfo.GetTargetWord();
				var guessedWord = word.GetGuessedWord();

				MatchLettersForColors(targetWord, guessedWord);
				CalculateUserScore(targetWord, guessedWord, user, dandleInfo.GameRound, isAltWord: true);
			}

			var (userId, points) = word.GetPointsForVotes();
			dandleInfo.AddPointsByUserId(userId, points);
		}

		dandleInfo.ClearSuggestionVoteList();
		return dandleInfo;
	}

	public async Task<DandleGame> GetBoardColors(DandleGame dandleInfo)
	{
		var guessedWord = dandleInfo.CurrentGuessedWord?.GetGuessedWord();
		var targetWord = dandleInfo.GetTargetWord();

		MatchLettersForColors(targetWord, guessedWord);

		var wordTiles = new List<LetterInfoVm>();

		// Create the guessed word tiles
		foreach (var letter in guessedWord)
		{
			var color = AssignColor(letter);
			wordTiles.Add(new LetterInfoVm(letter.Position, letter.Letter, color));
		}

		// Count letter occurrences in target word for duplicate letters
		var targetLetterCounts = targetWord
			.GroupBy(l => l.Letter)
			.ToDictionary(g => g.Key.ToString().ToUpper(), g => g.Count());

		// Count blue matches per letter in current guess
		var blueCountPerLetter = wordTiles
			.Where(b => b.Color == blue)
			.GroupBy(b => b.Letter.ToUpper())
			.ToDictionary(g => g.Key, g => g.Count());

		// Aggregate all letters guessed so far (including current guess)
		var allLetters = dandleInfo.GuessedWords
			.SelectMany(gw => gw.Letters)
			.Concat(wordTiles)
			.GroupBy(x => x.Letter.ToUpper());

		// Initialize locked blue keys set if null
		dandleInfo.LockedBlueKeys ??= [];

		// Update locked blue keys for letters fully matched this round
		foreach (var letter in blueCountPerLetter)
		{
			if (!targetLetterCounts.TryGetValue(letter.Key, out var targetCount)) continue;
			if (letter.Value == targetCount && targetCount > 0)
			{
				dandleInfo.LockedBlueKeys.Add(letter.Key);
			}
		}

		dandleInfo.Keyboard.Clear();

		// Create keyboard tiles
		foreach (var group in allLetters)
		{
			var letter = group.Key;

			// Persist locked blue keys forever
			if (dandleInfo.LockedBlueKeys.Contains(letter))
			{
				dandleInfo.Keyboard.Add(new LetterInfoVm(0, letter, blue));
				continue;
			}

			var targetCount = targetLetterCounts.ContainsKey(letter) ? targetLetterCounts[letter] : 0;
			var blueCount = blueCountPerLetter.ContainsKey(letter) ? blueCountPerLetter[letter] : 0;
			var occurrencesInCurrentGuess = wordTiles.Count(b => b.Letter.ToUpper() == letter);

			var hasBlue = group.Any(x => x.Color == blue);
			var hasOrange = group.Any(x => x.Color == orange);

			var color = "";

			if (blueCount == targetCount && targetCount > 1)
			{
				color = blue;
				dandleInfo.LockedBlueKeys.Add(letter); // Double safety lock
			}
			else
				switch (hasBlue)
				{
					case true when hasOrange:
					{
						color = occurrencesInCurrentGuess > 1 ? split : orange;
						break;
					}
					case true:
						color = blue;
						break;
					default:
					{
						color = hasOrange ? orange : gray;
						break;
					}
				}

			dandleInfo.Keyboard.Add(new LetterInfoVm(0, letter, color));
		}

		// Ensure guessed word tiles are in correct position
		wordTiles = wordTiles.OrderBy(x => x.Position).ToList();

		// Send board + keyboard via SignalR
		var vm = new DandleGuessedWordVm(wordTiles, dandleInfo.Keyboard);
		await Signalr.SendDandleWordGuess(vm);

		// Post chat message
		var chatWord = string.Join("", wordTiles.Select(x => x.Letter.ToUpper()));
		var chatColorBlock = string.Join("", wordTiles.Select(x =>
			_colorEmoji.TryGetValue(x.Color, out var emoji) ? emoji : "?"));

		var data = new GuessData(dandleInfo.GameRound, chatWord, chatColorBlock);
		await ChatReplyService.CreateResponse(Response.GuessResult, data);

		// Save current game state
		dandleInfo.TargetWord.Letters = targetWord;
		dandleInfo.GuessedWords.Add(new GuessedWord(dandleInfo.CurrentGuessedWord.Word, wordTiles));

		return dandleInfo;
	}


	/*══════════════════【  】══════════════════*/
	private void MatchLettersForColors(List<LetterInfo> targetWord, List<LetterInfo> guessedWord)
	{
		foreach (var g in guessedWord)
		{
			g.IsMatched = false;
			g.PointValue = 0;
		}
		foreach (var t in targetWord) { t.IsMatched = false; }

		// First pass to find exact matches
		for (var i = 0; i < Math.Min(targetWord.Count, guessedWord.Count); i++)
		{
			if (guessedWord[i].Letter != targetWord[i].Letter) continue;
			guessedWord[i].PointValue = 2;
			guessedWord[i].IsMatched = true;
			targetWord[i].IsMatched = true;
		}

		// Second pass to find partial matches
		foreach (var guess in guessedWord)
		{
			if (guess.IsMatched) continue;

			foreach (var target in targetWord)
			{
				if (target.IsMatched) continue;
				if (guess.Letter != target.Letter) continue;

				guess.PointValue = 1;
				guess.IsMatched = true;
				target.IsMatched = true;
				break;
			}
		}
	}

	// TODO: Need to see if scoring is correct
	private void CalculateUserScore(
		List<LetterInfo> targetWord,
		List<LetterInfo> guessedWord,
		DandleUser user,
		int gameRound,
		bool isAltWord)
	{
		foreach (var g in guessedWord)
		{
			if (!g.IsMatched) continue;

			var targetLetter = targetWord.FirstOrDefault(t => t.Position == g.Position);
			var userLetterScore = targetLetter != null ? user.GetPointsForLetter(targetLetter.Position) : null;

			if (userLetterScore == null) continue;

			if (g.PointValue == 2)
			{
				if (isAltWord)
				{
					user.BonusPoints += gameRound switch
					{
						1 or 2 => 4,
						3 or 4 => 3,
						_ => 2
					};
				}
				else
				{
					user.Points += gameRound switch
					{
						1 or 2 => 4,
						3 or 4 => 3,
						_ => 2
					};
				}
				userLetterScore.PointTotal = 0;
			}
			else if (g.PointValue == 1)
			{
				if (isAltWord)
				{
					user.BonusPoints += gameRound switch
					{
						1 or 2 or 3 => 2,
						_ => 1
					};
				}
				else
				{
					user.Points += gameRound switch
					{
						1 or 2 or 3 => 2,
						_ => 1
					};
				}
				userLetterScore.PointTotal = 1;
			}
		}
	}
	
	// private string MergeKeyColors(string oldColor, string newColor)
	// {
	// 	if (oldColor == newColor) return oldColor;
	//
	// 	if ((oldColor == orange && newColor == blue) ||
	// 	    (oldColor == blue && newColor == orange) ||
	// 	    (oldColor == split || newColor == split))
	// 		return split;
	//
	// 	if (oldColor == blue || newColor == blue) return blue;
	// 	if (oldColor == orange || newColor == orange) return orange;
	// 	return gray;
	// }

	// private string AssignColor(IEnumerable<LetterInfo> list)
	// {
	// 	var hasZero = list.Any(x => x.PointValue == 0);
	// 	var hasOne = list.Any(x => x.PointValue == 1);
	// 	var hasTwo = list.Any(x => x.PointValue == 2);
	// 	return ValueProcessor(hasZero, hasOne, hasTwo);
	// }

	private string AssignColor(LetterInfo letter)
	{
		var hasZero = letter.PointValue.Equals(0);
		var hasOne = letter.PointValue.Equals(1);
		var hasTwo = letter.PointValue.Equals(2);
		return ValueProcessor(hasZero, hasOne, hasTwo);
	}

	private string ValueProcessor(bool hasZero, bool hasOne, bool hasTwo)
	{
		if (hasZero && hasOne && hasTwo) return split;
		if (hasZero && hasOne) return orange;
		if (hasZero && hasTwo) return blue;
		if (hasOne && hasTwo) return split;
		if (hasZero) return gray;
		if (hasOne) return orange;
		if (hasTwo) return blue;
		return gray;
	}

}