using Koishibot.Core.Features.StreamInformation.Models;

namespace Koishibot.Core.Features.Polls.Models;

public class PollResult
{
	public int Id { get; set; }
	public int TwitchStreamId { get; set; }
	public DateTimeOffset StartedAt { get; set; }
	public string Title { get; set; } = null!;
	public string ChoiceOne { get; set; } = null!;
	public int VoteOne { get; set; }
	public string ChoiceTwo { get; set; } = null!;
	public int VoteTwo { get; set; }
	public string? ChoiceThree { get; set; }
	public int VoteThree { get; set; }
	public string? ChoiceFour { get; set; }
	public int VoteFour { get; set; }
	public string? ChoiceFive { get; set; }
	public int VoteFive { get; set; }
	public string WinningChoice { get; set; } = null!;

	// NAVIGATION

	public TwitchStream TwitchStream { get; set; } = null!;
}
