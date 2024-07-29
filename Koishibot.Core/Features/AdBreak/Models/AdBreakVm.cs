using System;

namespace Koishibot.Core.Features.AdBreak.Models;

public record AdBreakVm(
	TimeSpan AdLength,
	DateTime TimerEnds
	);