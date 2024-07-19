namespace Koishibot.Core.Features.Dandle.Models;

public record GuessedWord(
	string Word,
	List<LetterInfoVm> Letters
	);
