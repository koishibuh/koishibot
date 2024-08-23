using System;

namespace Koishibot.Core.Features.AdBreak.Models;

public record AdBreakVm(
	int AdLength,
	DateTime TimerEnds
	);