﻿using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Dandle.Controllers;
using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.Dandle.Extensions;
using Koishibot.Core.Features.Dandle.Interfaces;
using Koishibot.Core.Features.Dandle.Models;

namespace Koishibot.Core.Features.Dandle;

// TODO: uppercase letters from guesses
public record DandleResultsProcessor(
IAppCache Cache, ISignalrService Signalr,
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
		{ "#787c7e, #787c7e", "⬛" },
	};

	/*═════════◢◣═════════*/
	public async Task DetermineScore()
	{
		var dandleInfo = Cache.GetDandleInfo();
		dandleInfo.GetWinningSuggestion();

		dandleInfo = ProcessAltWords(dandleInfo);
		dandleInfo = ProcessGuessedWord(dandleInfo);

		// determine colors for board & keys
		// add guessed word to list
		// send to client
		// boards, keys,
		dandleInfo = await GetBoardColors(dandleInfo);

		if (dandleInfo.GuessedWordIsTargetWord())
		{
			await GameWon(dandleInfo);
		}
		else
		{
			if (dandleInfo.WasFinalRound())
			{
				await GameLost(dandleInfo);
			}
			else
			{
				await StartNextRound(dandleInfo);
			}
		}
	}

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

		var timer = Toolbox.CreateTimer(5, AnnounceGuessOpen);
		timer.Start();
	}

	private async void AnnounceGuessOpen()
	{
		await Signalr.SendDandleTimer(new DandleTimerVm("!Guess A Word", 0, 0));
		Cache.OpenDandleSuggestions();

		var dandleInfo = Cache.GetDandleInfo();

		var data = new NumberData(dandleInfo.GameRound);
		await ChatReplyService.App(Command.NextRound, data);
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
		await ChatReplyService.App(Command.GameLost, data);

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
		await ChatReplyService.App(Command.SolvedWord, data);

		await DandleWordService.DefineWord(dandleInfo.TargetWord.Word);

		Cache.DisableDandle();
		Cache.ResetDandle();
	}

	public async Task<DandleGame> GetBoardColors(DandleGame dandleInfo)
	{
		var guessedWord = dandleInfo.CurrentGuessedWord?.GetGuessedWord();
		var targetWord = dandleInfo.GetTargetWord();

		var scoredTargetLetters = new List<LetterInfo>();
		var scoredGuessedLetters = new List<LetterInfo>();

		var boards = new List<LetterInfoVm>();
		var keys = new List<LetterInfoVm>();


		// for each matching letter
		for (int i = Math.Min(guessedWord.Count, targetWord.Count) - 1; i >= 0; i--)
		{
			if (guessedWord[i].Letter != targetWord[i].Letter) continue;

			guessedWord[i].PointValue = 2;
			targetWord[i].PointValue = 0;

			scoredTargetLetters.Add(targetWord[i]);
			scoredGuessedLetters.Add(guessedWord[i]);

			guessedWord.RemoveAt(i);
			targetWord.RemoveAt(i);
		}

		// Score remaining letters

		for (var k = guessedWord.Count - 1; k >= 0; k--)
		{
			for (var j = targetWord.Count - 1; j >= 0; j--)
			{
				if (guessedWord[k].Letter == targetWord[j].Letter)
				{
					// make first instance yellow
					if (targetWord[j].PointValue == 0)
					{
						guessedWord[k].PointValue = 1;
					}
					else
					{
						targetWord[j].PointValue = 1;
						guessedWord[k].PointValue = 1;
					}

					// If 1, user already received 1 point for letter being in incorrect place
					scoredTargetLetters.Add(targetWord[j]);
					scoredGuessedLetters.Add(guessedWord[k]);
					guessedWord.RemoveAt(k);
					targetWord.RemoveAt(j);
					break;
				}
				if (j != 0) continue;

				scoredGuessedLetters.Add(guessedWord[k]);
				guessedWord.RemoveAt(k);
			}
		}

		scoredTargetLetters.AddRange(targetWord);

		var groups = scoredGuessedLetters.GroupBy(x => x.Letter);


		var singleLetters = groups.Where(x => x.Count() == 1).Select(x => x.First()).ToList();
		var dupeLetterGroup = groups.Where(x => x.Count() > 1).Select(x => x.ToList()).ToList();

		// for each dupeLetter
		foreach (var dupeLetter in dupeLetterGroup)
		{
			foreach (var t in dupeLetter)
			{
				var yellowAssigned = false;
				// check if there is only one letter in the target word
				var count = scoredTargetLetters.Where(x => x.Letter == t.Letter).Count();
				if (count == 1)
				{
					if (yellowAssigned is false)
					{
						var boardColor1 = AssignColor(t);
						var board1 = new LetterInfoVm(t.Position, t.Letter, boardColor1);
						boards.Add(board1);
						yellowAssigned = true;
					}
					else
					{
						var board1 = new LetterInfoVm(t.Position, t.Letter, gray);
						boards.Add(board1);
						yellowAssigned = true;
					}

					//// check color
					//var scoredTargetLetter = scoredTargetLetters.Where(x => x.Letter == dupeLetter[l].Letter).FirstOrDefault();
					//if (scoredTargetLetter.PointValue == 0) // already blue
					//{

					//}
				}
				else
				{
					// assign board board
					var boardColor = AssignColor(t);
					var board = new LetterInfoVm(t.Position, t.Letter.ToString(), boardColor);
					boards.Add(board);
				}
			}

			var keyColor = AssignColor(dupeLetter);

			var previousKey = dandleInfo.Keyboard.Find(x => x.Letter == dupeLetter[0].Letter);
			if (previousKey is null)
			{
				// assign key color
				var key = new LetterInfoVm(0, dupeLetter[0].Letter.ToString(), keyColor);
				keys.Add(key);
			}
			else
			{
				if (keyColor == gray) continue;

				var key = new LetterInfoVm(0, dupeLetter[0].Letter.ToString(), keyColor);
				keys.Add(key);
			}
		}


		foreach (var letter in singleLetters)
		{
			var newBoardColor = AssignColor(letter);

			// check if letter has already been assigned color 
			var previousKey = dandleInfo.Keyboard.Find(x => x.Letter == letter.Letter);
			if (previousKey is not null)
			{
				// if target has dupes
				var count = scoredTargetLetters.Count(x => x.Letter == letter.Letter);
				if (count > 1) // target word has duplicate letters but guess word only has single
				{
					if (previousKey.Color == blue) // if blue previously
					{
						if (newBoardColor == gray) // current color gray
						{
							var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
							boards.Add(board);
						}
						else if (newBoardColor == orange) // current color orange									
						{
							var allDiscovered = scoredTargetLetters.Where(x => x.Letter == letter.Letter).Any(x => x.PointValue != 2);
							if (!allDiscovered)
							{
								var key = new LetterInfoVm(letter.Position, letter.Letter, split);
								keys.Add(key);
							}

							var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
							boards.Add(board);
						}
						else // blue
						{
							var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
							boards.Add(board);
						}
					}
					else if (previousKey.Color == orange) // if orange previously
					{
						if (newBoardColor == gray) // current color gray
						{
							var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
							boards.Add(board);
						}
						else if (newBoardColor == blue) // current color blue
						{
							var allDiscovered = scoredTargetLetters.Where(x => x.Letter == letter.Letter).Any(x => x.PointValue != 2);
							if (!allDiscovered)
							{
								var key = new LetterInfoVm(letter.Position, letter.Letter, split);
								keys.Add(key);
							}
							else
							{
								var key = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
								keys.Add(key);
							}

							var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
							boards.Add(board);

						}
					}
					else if (previousKey.Color == split) // if split previously
					{
						var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
						boards.Add(board);
					}
				}
				else // target word has single of each letter, guess word only has single
				{
					if (previousKey.Color == orange) // if orange previously
					{
						if (newBoardColor == gray) // current color gray
						{
							var board = new LetterInfoVm(letter.Position, letter.Letter, previousKey.Color);
							boards.Add(board);
						}
						else if (newBoardColor == blue) // current color blue
						{
							var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
							boards.Add(board);
							keys.Add(board);
						}
						else
						{
							var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
							boards.Add(board);
						}
					}
					else if (previousKey.Color == blue) // if blue previously
					{
						var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
						boards.Add(board);
					}
					else
					{
						var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
						boards.Add(board);
					}
				}
			}
			else
			{
				var board = new LetterInfoVm(letter.Position, letter.Letter, newBoardColor);
				boards.Add(board);
				keys.Add(board);
			}
		}


		// sort boards
		boards = boards.OrderBy(x => x.Position).ToList();
		scoredTargetLetters = scoredTargetLetters.OrderBy(x => x.Position).ToList();

		// send to client
		// boards, keys, user points
		var vm = new DandleGuessedWordVm(boards, keys);
		await Signalr.SendDandleWordGuess(vm);


		var chatWord = "";
		var chatColorBlock = "";

		foreach (var board in boards)
		{
			chatWord += board.Letter.ToUpper();

			if (_colorEmoji.TryGetValue(board.Color, out var emoji))
			{
				chatColorBlock += emoji;
			}
			else
			{
				chatColorBlock += "?";
			}
		}

		var data = new GuessData(dandleInfo.GameRound, chatWord, chatColorBlock);
		await ChatReplyService.App(Command.GuessResult, data);

		// store updated targetwordpoints
		dandleInfo.TargetWord.Letters = scoredTargetLetters;

		// update the values in keyboard; if the key exists, replace it with new one
		foreach (var newKey in keys)
		{
			var storedKey = dandleInfo.Keyboard.FirstOrDefault(x => x.Letter == newKey.Letter);
			if (storedKey is not null)
			{
				storedKey.Color = newKey.Color;
			}
			else
			{
				dandleInfo.Keyboard.Add(newKey);
			}
		}

		// add board -> vm to guessedwords
		dandleInfo.GuessedWords.Add(new GuessedWord(dandleInfo.CurrentGuessedWord.Word, boards));
		return dandleInfo;
	}

	private DandleGame ProcessGuessedWord(DandleGame dandleInfo)
	{
		// For each user who voted for the winning suggestion guess
		foreach (var voter in dandleInfo.CurrentGuessedWord.Voters)
		{
			var user = dandleInfo.GetDandleUserPoints(voter);
			var targetWord = dandleInfo.GetTargetWord();
			var guessedWord = dandleInfo.CurrentGuessedWord?.GetGuessedWord();

			// for each matching letter
			for (var i = Math.Min(guessedWord.Count, targetWord.Count) - 1; i >= 0; i--)
			{
				if (guessedWord[i].Letter != targetWord[i].Letter) continue;

				var userTargetScore = user.GetPointsForLetter(targetWord[i].Position);
				user.Points = userTargetScore.PointTotal switch
				{
					2 => dandleInfo.GameRound switch
					{
						1 or 2 => user.Points + 4,
						3 or 4 => user.Points + 3,
						_ => user.Points + 2
					},

					1 => dandleInfo.GameRound switch
					{
						1 or 2 or 3 => user.Points + 2,
						_ => user.Points + 1
					},

					_ => user.Points
				};
				userTargetScore.PointTotal = 0;
				guessedWord.RemoveAt(i);
				targetWord.RemoveAt(i);
			}

			// Score remaining letters
			for (var j = targetWord.Count - 1; j >= 0; j--)
			{
				for (var k = guessedWord.Count - 1; k >= 0; k--)
				{
					if (guessedWord[k].Letter != targetWord[j].Letter) continue;

					var userTargetScore = user.GetPointsForLetter(targetWord[j].Position);
					if (userTargetScore.PointTotal == 2)
					{
						user.Points = dandleInfo.GameRound switch
						{
							1 or 2 or 3 => user.Points + 2,
							_ => user.Points + 1
						};

						userTargetScore.PointTotal = 1;
					}
					// If 1, user already received 1 point for letter being in incorrect place
					guessedWord.RemoveAt(k);
					targetWord.RemoveAt(j);
					break;
				}
			}
		}

		var (userId, points) = dandleInfo.CurrentGuessedWord.GetPointsForVotes();
		dandleInfo.AddPointsByUserId(userId, points);

		return dandleInfo;
	}

	/*═════════◢◣═════════*/
	private DandleGame ProcessAltWords(DandleGame dandleInfo)
	{
		var altWords = dandleInfo.CurrentVotes;
		foreach (var word in altWords)
		{
			foreach (var voter in word.Voters)
			{
				var user = dandleInfo.GetDandleUserPoints(voter);

				var targetWord = dandleInfo.GetTargetWord();
				var guessedWord = word.GetGuessedWord();

				for (var i = Math.Min(targetWord.Count, guessedWord.Count) - 1; i >= 0; i--)
				{
					// found match
					if (guessedWord[i].Letter != targetWord[i].Letter) continue;

					var userLetterScore = user.GetPointsForLetter(targetWord[i].Position);

					user.BonusPoints = userLetterScore.PointTotal switch
					{
						2 => dandleInfo.GameRound switch
						{
							1 or 2 => user.BonusPoints + 4,
							3 or 4 => user.BonusPoints + 3,
							_ => user.BonusPoints + 2
						},

						1 => dandleInfo.GameRound switch
						{
							1 or 2 or 3 => user.BonusPoints + 2,
							_ => user.BonusPoints + 1
						},

						_ => user.BonusPoints
					};

					userLetterScore.PointTotal = 0;
					guessedWord.RemoveAt(i);
					targetWord.RemoveAt(i);
				}

				// Go through remaining letters
				for (var i = targetWord.Count - 1; i >= 0; i--)
				{
					for (var j = guessedWord.Count - 1; j >= 0; j--)
					{
						if (guessedWord[j].Letter != targetWord[i].Letter) continue;

						var userLetterScore = user.GetPointsForLetter(targetWord[i].Position);
						if (userLetterScore.PointTotal == 2)
						{
							user.BonusPoints += dandleInfo.GameRound switch
							{
								1 or 2 or 3 => 2,
								_ => 1
							};

							userLetterScore.PointTotal = 1;
						}
						// If 1, user already go ta point for letter being in incorrect place

						guessedWord.RemoveAt(j);
						targetWord.RemoveAt(i);

						break;
					}
				}
			}

			var (userId, points) = word.GetPointsForVotes();
			dandleInfo.AddPointsByUserId(userId, points);
		}

		dandleInfo.ClearSuggestionVoteList();
		return dandleInfo;
	}


	private string AssignColor(IEnumerable<LetterInfo> list)
	{
		var hasZero = list.Any(x => x.PointValue == 0);
		var hasOne = list.Any(x => x.PointValue == 1);
		var hasTwo = list.Any(x => x.PointValue == 2);

		return ValueProcessor(hasZero, hasOne, hasTwo);
	}

	private string AssignColor(LetterInfo letter)
	{
		var hasZero = letter.PointValue.Equals(0);
		var hasOne = letter.PointValue.Equals(1);
		var hasTwo = letter.PointValue.Equals(2);

		return ValueProcessor(hasZero, hasOne, hasTwo);
	} // #787c7e = gray // f5793a orange // 44a8ff blue

	private string ValueProcessor(bool hasZero, bool hasOne, bool hasTwo)
	{
		if (hasZero && hasOne && hasTwo) return "54deg, #ffbc42 50%, #44a8ff 50%"; // split
		if (hasZero && hasOne) return "#f5793a, #f5793a"; // orange
		if (hasZero && hasTwo) return "#44a8ff, #44a8ff"; // blue
		if (hasOne && hasTwo) return "54deg, #ffbc42 50%, #44a8ff 50%"; // split
		if (hasZero) return "#787c7e, #787c7e"; //grey
		if (hasOne) return "#f5793a, #f5793a"; // orange
		if (hasTwo) return "#44a8ff, #44a8ff"; // blue
		return "#787c7e, #787c7e";
	}
}