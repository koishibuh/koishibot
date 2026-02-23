using Koishibot.Core.Features.Dandle.Enums;
using Koishibot.Core.Features.RaidSuggestions.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
namespace Koishibot.Core.Features.Dandle.Models;

public class DandleGame
{
	public Phase GamePhase { get; set; } = Phase.Suggestions;
	public int GameRound { get; set; }
	public TargetWord TargetWord { get; set; } = new();

	public DandleVotes CurrentGuessedWord { get; set; } = null!;
	public List<DandleSuggestion> CurrentSuggestedWords { get; set; } = [];
	public List<DandleVotes> CurrentVotes { get; set; } = [];

	public List<GuessedWord> GuessedWords { get; set; } = [];
	public List<LetterInfoVm> Keyboard { get; set; } = [];

	public List<DandleUser> UserPoints { get; set; } = [];

	public List<DandleWord> DandleDictionary { get; set; } = [];
	
	public HashSet<string> LockedBlueKeys { get; set; } = new HashSet<string>();

	//

	public void AddPointsByUserId(int userId, int points)
	{
		var user = UserPoints.First(x => x.UserId == userId);
		user.Points += points;
	}

	public void ClearCurrentSuggestions()
	{
		CurrentSuggestedWords.Clear();
	}

	public bool GuessedWordIsTargetWord()
	{
		return CurrentGuessedWord.WordId == TargetWord.WordId;
	}

	public DandleGame SetNewGame()
	{
		GamePhase = Phase.Suggestions;
		GameRound = 1;
		return this;
	}

	public DandleGame LoadDictionary(List<DandleWord> dandleDictionary)
	{
		DandleDictionary = dandleDictionary;
		return this;
	}

	public DandleGame SelectRandomWord()
	{
		// Debug
		// var targetWord = DandleDictionary.Find(x => x.Id == 1723);

		var targetWord = DandleDictionary.MinBy(x => new Random().Next())
			?? throw new Exception("Dandle word list empty");

		var letters = targetWord.Word
			.Select((c, index) => new LetterInfo
				{ Letter = c.ToString(), Position = index + 1, PointValue = 2 })
			.ToList();

		TargetWord = new TargetWord
		{
			WordId = targetWord.Id,
			Word = targetWord.Word,
			Letters = letters
		};

		return this;
	}

	public async Task AddSuggestionToOverlay
		(ISignalrService signalrR, TwitchUser user, string word)
	{
		var vm = new DandleSuggestionVm(user.Id, user.Name, word);
		await signalrR.SendDandleWordSuggestion(vm);
	}

	public bool UserAlreadySuggestedWord(TwitchUser user)
	{
		var result = CurrentSuggestedWords
			.FirstOrDefault(x => x.User.Id == user.Id);

		return result is not null;
	}

	public bool UserAlreadySuggestedUniqueWord(TwitchUser user)
	{
		var result = CurrentSuggestedWords
			.GroupBy(x => x.DandleWord.Word)
			.Where(x => x.Count() == 1 && x.Any(x => x.User.Id == user.Id))
			.Select(x => x.Key);

		return result.Count() == 1;
	}

	public bool WordAlreadySuggestedByUser(string word, TwitchUser user)
	{
		var result = CurrentSuggestedWords
			.FirstOrDefault(x => x.DandleWord.Word == word && x.User.Id == user.Id);

		return result is not null;
	}

	public bool WordAlreadyUpvotedByUser(string word, TwitchUser user)
	{
		var result = CurrentSuggestedWords
			.FirstOrDefault(x => x.DandleWord.Word == word
				&& x.SuggestionCounter.Any(x => x.Id == user.Id));

		return result is not null;
	}


	public bool WordAlreadySuggested(string word)
	{
		var result = CurrentSuggestedWords
			.FirstOrDefault(x => x.DandleWord.Word == word);

		return result is not null;
	}

	public bool WordAlreadyGuessed(string word)
	{
		var result = GuessedWords
			.FirstOrDefault(x => x.Word == word);

		return result is not null;
	}

	public DandleWord? ValidateWord(string word) =>
		DandleDictionary.FirstOrDefault(x => x.Word == word);

	public DandleGame AddWordSuggestion(TwitchUser user, DandleWord word)
	{
		CurrentSuggestedWords
			.Add(new DandleSuggestion(user, word, []));
		return this;
	}

	public DandleGame AddToSuggestionCounter(TwitchUser user, string word)
	{
		var result = CurrentSuggestedWords
			.FirstOrDefault(x => x.DandleWord.Word == word);

		if (result is not null)
		{
			result.SuggestionCounter.Add(user);
		}
		else
		{
			throw new Exception("Word not found in Suggestions");
		}

		return this;

	}

	public DandleGame AddPointsToUserScore(TwitchUser user, int points)
	{
		var result = UserPoints.FirstOrDefault(x => x.UserId == user.Id);
		if (result is null)
		{
			var dandlePoint = new DandleUser { UserId = user.Id, Username = user.Name, Points = points, BonusPoints = 0 };
			UserPoints.Add(dandlePoint);
		}
		else
		{
			UserPoints
				.Where(x => x.UserId == user.Id)
				.ToList()
				.ForEach(x => x.UpdatePublicPoints(points));
		}
		return this;
	}

	public DandleUser GetDandleUserPoints(TwitchUser user)
	{
		var result = UserPoints.FirstOrDefault(x => x.UserId == user.Id);
		if (result is not null) return result;

		var newUserPoint = new DandleUser { UserId = user.Id, Username = user.Name, Points = 0, BonusPoints = 0 };
		UserPoints.Add(newUserPoint);
		return newUserPoint;
	}

	public void AddVote(TwitchUser user, int index)
	{
		var result = CurrentVotes[index];
		result.Voters.Add(user);
	}

	public void AddSuggestorAsVoter()
	{
		var user = CurrentVotes[0].SuggestedBy;
		AddVote(user, 0);
	}


	public void SelectWordCandidates()
	{
		if (CurrentSuggestedWords.Count() <= 3)
		{
			CurrentVotes = CurrentSuggestedWords.Select(x => new DandleVotes
			{
				SuggestedBy = x.User,
				WordId = x.DandleWord.Id,
				Word = x.DandleWord.Word,
				Letters = x.DandleWord.Word
					.Select((c, index) => new LetterInfo
						{ Letter = c.ToString(), Position = index + 1, PointValue = 2 })
					.ToList(),
				Voters = []
			}).ToList();
		}
		else
		{
			var selectedWords = new List<DandleVotes>();

			while (selectedWords.Count < 3)
			{
				var suggestionCount = CurrentSuggestedWords.Sum(x => x.SuggestionCounter.Count);
				var randomNumber = new Random().Next(1, suggestionCount + 1);

				var first = true;
				var counter = 0;

				foreach (var item in CurrentSuggestedWords)
				{
					if (first)
					{
						counter = item.SuggestionCounter.Count;

						if (item.SuggestionCounter.Count >= randomNumber)
						{
							CurrentVotes.Add(new DandleVotes
							{
								SuggestedBy = item.User,
								WordId = item.DandleWord.Id,
								Word = item.DandleWord.Word,
								Letters = item.DandleWord.Word
									.Select((c, index) => new LetterInfo
										{ Letter = c.ToString(), Position = index + 1, PointValue = 2 })
									.ToList(),
								Voters = []
							});

							CurrentSuggestedWords.Remove(item);
							counter = 0;
							break;
						}

						first = false;
					}
					else
					{
						if (counter + 1 >= randomNumber)
						{
							CurrentVotes.Add(new DandleVotes
							{
								SuggestedBy = item.User,
								WordId = item.DandleWord.Id,
								Word = item.DandleWord.Word,
								Letters = item.DandleWord.Word
									.Select((c, index) => new LetterInfo
										{ Letter = c.ToString(), Position = index + 1, PointValue = 2 })
									.ToList(),
								Voters = []
							});

							CurrentSuggestedWords.Remove(item);
							counter = 0;
							first = true;
							break;
						}
					}
				}
			}
		}
	}

	public string CreateDandleWordString()
	{
		return string.Join(" ", CurrentVotes.Select((word, index) =>
			$"{index + 1}. {word.Word.ToUpper()}"));
	}

	// if (dandleInfo.GameRound < 6)
	public bool WasFinalRound() => GameRound >= 6;

	public List<PollChoiceInfo> CreatePollChoiceVm() =>
		CurrentVotes.Select(x => new PollChoiceInfo(x.Word, 0)).ToList();

	public bool UserAlreadyVoted(TwitchUser user, int index)
	{
		var result = CurrentVotes.Any(x => x.Voters.Any(y => y.Id == user.Id));
		return result;

		//var result = CurrentVotes[index];
		//var voter = result.Voters.FirstOrDefault(x => x.UserId == user.Id);

		//return voter is not null;
	}

	public void GetWinningSuggestion()
	{
		var random = new Random();

		var winningSuggestion = CurrentVotes
			.OrderByDescending(x => x.Voters.Count)
			.ThenBy(pr => random.Next())
			.FirstOrDefault();

		if (winningSuggestion is null)
			throw new Exception("Unable to get Winning Suggestion");

		CurrentGuessedWord = winningSuggestion;
		CurrentVotes.Remove(winningSuggestion);
	}

	public void ClearSuggestionVoteList() => CurrentVotes = [];

	public List<LetterInfo> GetTargetWord() =>
	[
		new() { Position = TargetWord.Letters[0].Position, Letter = TargetWord.Letters[0].Letter, PointValue = TargetWord.Letters[0].PointValue },
		new() { Position = TargetWord.Letters[1].Position, Letter = TargetWord.Letters[1].Letter, PointValue = TargetWord.Letters[1].PointValue },
		new() { Position = TargetWord.Letters[2].Position, Letter = TargetWord.Letters[2].Letter, PointValue = TargetWord.Letters[2].PointValue },
		new() { Position = TargetWord.Letters[3].Position, Letter = TargetWord.Letters[3].Letter, PointValue = TargetWord.Letters[3].PointValue },
		new() { Position = TargetWord.Letters[4].Position, Letter = TargetWord.Letters[4].Letter, PointValue = TargetWord.Letters[4].PointValue }
	];


	public bool FirstGuess() => CurrentSuggestedWords.Count == 1;

	public PollChoiceInfo CreateVoteVm(int index)
	{
		var result = CurrentVotes[index];
		return new PollChoiceInfo(result.Word, 1);
	}

	public bool OnlyOneSuggestionSubmitted() => CurrentVotes.Count == 1;

	public List<DandleUserVm> CreateDandleUserVmList() =>
		UserPoints
			.Select(x => new DandleUserVm(x.UserId, x.Username, x.Points, x.BonusPoints))
			.ToList();

	public void IncreaseGameRound() => GameRound++;
}