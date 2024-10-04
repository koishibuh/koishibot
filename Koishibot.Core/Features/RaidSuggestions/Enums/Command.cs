namespace Koishibot.Core.Features.RaidSuggestions.Enums;

public class Command
{
	// Raid
	/// <summary> Username, Title, Category </summary>
	public const string RaidReceived = "raid-received";

	// Poll
	/// <summary> Title, Winner </summary>
	public const string PollWinner = "poll-winner";
	public const string PollCancelled = "poll-cancelled";

	// OpenRaidSuggestionsHandler

	public const string SuggestionsNowOpen = "raidsuggestion-nowopen";

	// RaidCommands

	public const string StreamNotOver = "raidsuggestion-notopen";
	public const string RaidSuggestionsClosed = "raidsuggestion-nowclosed";
	public const string NoSuggestionMade = "raidsuggestion-empty";

	// RaidSuggestionValidation

	public const string CantSuggestMe = "raidsuggestion-me";
	public const string DupeSuggestion = "raidsuggestion-dupe";
	public const string NotValidUser = "raidsuggestion-invaliduser";
	public const string StreamerOffline = "raidsuggestion-offline";
	public const string MaxViewerCount = "raidsuggestion-maxviewers";
	public const string RestrictedChat = "raidsuggestion-restricted";
	public const string SuggestionSuccessful = "raidsuggestion-successful";

	// SelectRaidCandidatesService

	public const string VotingSoon = "raidsuggestion-votingsoon";

	// RaidPollProcessor

	public const string RaidTarget = "raid-winningtarget";

	// CancelRaidSuggestionHandler

	public const string RaidSuggestionsCancelled = "raidsuggestion-cancelled";

	// AddOutgoingRaidHandler

	public const string RaidLeftBehind = "raid-leftbehind";

	// Unused

	public const string RaidCancelled = "raid-cancelled";
	public const string RaidCall = "raidcall";
	public const string RaidLink = "raid-link";
	public const string RaidNewTarget = "raid-newtarget";
}