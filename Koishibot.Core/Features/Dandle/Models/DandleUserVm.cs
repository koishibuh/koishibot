namespace Koishibot.Core.Features.Dandle.Models;

public record DandleUserVm(
	int UserId,
	string Username,
	int Points,
	int BonusPoints
	);