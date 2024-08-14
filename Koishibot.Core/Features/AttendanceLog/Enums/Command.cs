namespace Koishibot.Core.Features.AttendanceLog.Enums;

public static class Command
{
	public const string StreakStarted = "attendance-streakstarted";
	public const string StreakContinued = "attendance-streakcontinued";
	public const string CurrentStreak = "attendance-currentstreak";
	public const string PbStreak = "pbstreak";
	public const string Attendance = "attendance";
	public const string StreakOptOut = "optout";
	public const string OptOutError = "attendance-optouterror";
	public const string StreakOptIn = "optin";
	public const string OptInError = "attendance-optinerror";
	public const string TopStreaks = "topstreaks";
}