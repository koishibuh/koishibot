namespace Koishibot.Core.Features.Dandle.Models;

public class TargetWord
{
	public int WordId { get; set; }
	public string Word { get; set; } = null!;
	public List<LetterInfo> Letters { get; set; } = [];
}