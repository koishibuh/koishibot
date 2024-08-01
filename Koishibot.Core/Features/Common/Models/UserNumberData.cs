namespace Koishibot.Core.Features.Common.Models;
public record UserNumberData(
	string User,
	string Number);

public record UserCountData(
	string User,
	int Number);

public record UserCountStreakData(
	string User,
	int Number,
	string Word,
	string Emoji);

public record TopAttendanceData(
	string Emoji,
	string Word,
	string List);