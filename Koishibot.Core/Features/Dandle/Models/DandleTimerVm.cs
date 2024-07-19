namespace Koishibot.Core.Features.Dandle.Models;

public record DandleTimerVm(
	string Status,
	int Minutes,
	int Seconds
);