namespace Koishibot.Core.Features.AttendanceLog.Models;

public record AttendanceStreak(
	string Username,
	int StreakCount
	);