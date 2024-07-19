using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.Dandle.Models;

public class DandleVotes
{
	public TwitchUser SuggestedBy { get; set; } = null!;
	public int WordId { get; set; }
	public string Word { get; set; } = string.Empty;
	public List<LetterInfo> Letters { get; set; } = [];
	public List<TwitchUser> Voters { get; set; } = [];

	//

	public List<LetterInfo> GetGuessedWord()
	{
		return new List<LetterInfo>
		{
			new() { Position = Letters[0].Position, Letter = Letters[0].Letter, PointValue = 0 },
			new() { Position = Letters[1].Position, Letter = Letters[1].Letter, PointValue = 0 },
			new() { Position = Letters[2].Position, Letter = Letters[2].Letter, PointValue = 0 },
			new() { Position = Letters[3].Position, Letter = Letters[3].Letter, PointValue = 0 },
			new() { Position = Letters[4].Position, Letter = Letters[4].Letter, PointValue = 0 }
		};
	}

	/// <summary>
	/// Calculate number of votes word received not including SuggestedBy
	/// Add points to public points.
	/// </summary>
	public (int userId, int points) GetPointsForVotes()
	{
		var voters = Voters.Where(x => x.Id != SuggestedBy.Id).ToList();
		return (SuggestedBy.Id, voters.Count);
	}

}