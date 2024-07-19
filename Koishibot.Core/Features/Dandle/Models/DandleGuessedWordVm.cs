namespace Koishibot.Core.Features.Dandle.Models;

public record DandleGuessedWordVm(
	List<LetterInfoVm> Letters,
	List<LetterInfoVm> Keys
	);