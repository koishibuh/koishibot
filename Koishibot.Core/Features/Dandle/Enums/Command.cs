namespace Koishibot.Core.Features.Dandle.Enums;
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

	public const string NoDefinition = "dandle-nodefinition";
	public const string Definition = "dandle-definition";
	public const string WordNotFound = "dandle-wordnotfound";
	public const string InvalidLength = "dandle-invalidwordlength";
	public const string WordExists = "dandle-wordexists";
	public const string InvalidWord = "dandle-invalidword";
	public const string WordAdded = "dandle-wordadded";
	public const string WordRemoved = "dandle-wordremoved";
}
