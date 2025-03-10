﻿namespace Koishibot.Core.Features.Dandle.Enums;
public class Command
{
	public const string NewGame = "dandle-gamestarted";
	public const string GameOver = "dandle-gameover";
	public const string NowVoting = "dandle-nowvoting";

	public const string ChosenWord = "dandle-chosenword";
	public const string SolvedWord = "dandle-solved";
	public const string NextRound = "dandle-nextround";
	public const string GameLost = "dandle-gamelost";
	public const string GuessResult = "dandle-guessresult";

	// Dandle Voting

	public const string InvalidVote = "dandle-invalidvote";
	public const string AlreadyVoted = "dandle-alreadyvoted";

	// Dandle Definition

	public const string Definition = "define";
	public const string NoDefinition = "dandle-definefailed";
	public const string WordAdded = "dandle-wordadded";
	public const string WordAddedFailed = "dandle-wordaddedfailed";
	public const string WordRemoved = "dandle-wordremoved";
	public const string WordRemovedFailed = "dandle-wordremovedfailed";
	public const string InvalidWord = "dandle-wordinvalid";
	public const string InvalidWordLength = "dandle-wordinvalidlength";
	public const string FindWord = "dandle-wordfind";
	public const string WordNotFound = "dandle-wordfindfailed";
}