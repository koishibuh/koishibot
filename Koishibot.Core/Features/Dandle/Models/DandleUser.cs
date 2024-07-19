namespace Koishibot.Core.Features.Dandle.Models;

// Keeps track of Dandle Points for user
public class DandleUser
{
	public int UserId { get; set; }
	public string Username { get; set; } = null!;

	public List<LetterPoints> CurrentLetterScore { get; set; } =
		new List<LetterPoints>
		{
			new() { Position = 1, PointTotal = 2},
			new() { Position = 2, PointTotal = 2},
			new() { Position = 3, PointTotal = 2},
			new() { Position = 4, PointTotal = 2},
			new() { Position = 5, PointTotal = 2}
		};

	public int Points { get; set; }
	public int BonusPoints { get; set; }

	public void UpdatePublicPoints(int points)
	{
		Points = Points + points;
	}

	public LetterPoints GetPointsForLetter(int position)
	{
		return CurrentLetterScore.Find(x => x.Position == position)
			?? throw new Exception("Unable to find User's points for letter");
	}
}

public class LetterPoints
{
	public int Position { get; set; }
	public int PointTotal { get; set; }
}