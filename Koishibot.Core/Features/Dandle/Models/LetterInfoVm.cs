namespace Koishibot.Core.Features.Dandle.Models;

public record LetterInfoVm(
	int Position,
	string Letter,
	string Color
	)
{
	public string Color { get; set; } = Color;
}