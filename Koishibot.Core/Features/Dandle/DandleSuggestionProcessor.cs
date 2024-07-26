//using Koishibot.Core.Features.ChatCommand.Models;
//using Koishibot.Core.Features.Common;
//using Koishibot.Core.Features.Dandle.Extensions;
//using Koishibot.Core.Features.Dandle.Interfaces;

//namespace Koishibot.Core.Features.Dandle;

///// <summary>
///// Checks suggested Dandle word
///// </summary>
///// <param name="Cache"></param>
///// <param name="Signalr"></param>
//public record DandleSuggestionProcessor(
//	IChatMessageService BotIrc, IDandleTimer DandleTimer,
//	IAppCache Cache, ILogger<DandleSuggestionProcessor> Log,
//	ISignalrService Signalr
//	) : IDandleSuggestionProcessor
//{
//	public async Task Start(ChatMessageCommand c)
//	{
//		var dandleInfo = Cache.GetDandleInfo();

//		if (Toolbox.StringContainsNonLetters(c.Message))
//		{
//			Log.LogInformation("Word contains numbers or punctuation");
//			await Signalr.SendDandleMessage($"{c.User.Name}, {c.Message} is not valid");
//			return;
//		}

//		var result = dandleInfo.ValidateWord(c.Message);
//		if (result is null)
//		{
//			Log.LogInformation("Word not valid");
//			await Signalr.SendDandleMessage($"{c.User.Name}, {c.Message} is not valid");
//			return;
//		}

//		if (dandleInfo.WordAlreadyGuessed(c.Message))
//		{
//			Log.LogInformation("Word already guessed");
//			await Signalr.SendDandleMessage($"{c.User.Name}, {c.Message} already guessed");
//			return;
//		}

//		if (dandleInfo.WordAlreadySuggested(c.Message))
//		{
//			//// check if user suggested the word already
//			if (dandleInfo.WordAlreadySuggestedByUser(c.Message, c.User))
//			{
//				await Signalr.SendDandleMessage($"{c.User.Name}, you've already suggested that word");
//				return;
//			}

//			if (dandleInfo.WordAlreadyUpvotedByUser(c.Message, c.User))
//			{
//				await Signalr.SendDandleMessage($"{c.User.Name}, you've already suggested that word as well");
//				return;
//			}

//			// no points for user if word already suggested
//			await dandleInfo
//				.AddToSuggestionCounter(c.User, c.Message)
//				.AddSuggestionToOverlay(Signalr, c.User, result.Word);

//			Cache.UpdateDandle(dandleInfo);

//			return;			
//		}

//		if (dandleInfo.UserAlreadySuggestedWord(c.User))
//		{
//			Log.LogInformation("User already suggested a word");
//			await Signalr.SendDandleMessage($"{c.User.Name}, you've already suggested a unique word");
//			return;
//		}

//		await dandleInfo
//			.AddWordSuggestion(c.User, result)
//			.AddPointsToUserScore(c.User, 1)
//			.AddSuggestionToOverlay(Signalr, c.User, result.Word);

//		Cache.UpdateDandle(dandleInfo);

//		Log.LogInformation("Word added");
//		await Signalr.SendDandleMessage($"{c.User.Name}, {result.Word} was submitted");

//		if (dandleInfo.FirstGuess())
//		{
//			await DandleTimer.StartSuggestionTimer(dandleInfo.GameRound);
//		}
//	}
//}

