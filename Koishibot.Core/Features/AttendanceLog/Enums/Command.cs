namespace Koishibot.Core.Features.AttendanceLog.Enums;

public static class Command
{
	public const string PbStreak = "pbstreak";
	public const string PbSteak = "pbsteak";
	public const string Attendance = "attendance";
	public const string StreakOptOut = "optout";
	public const string StreakOptIn = "optin";
	public const string TopStreaks = "topstreaks";
	public const string TopStreak = "topstreak";
	public const string TopSteaks = "topsteaks";
	public const string TopSteak = "topsteak";
	public const string Streak = "streak";
	public const string Streaks = "streaks";
	public const string Steak = "steak";
	public const string Steaks = "steaks";
}

public static class Response
{
	public const string StreakStarted = "attendance-streakstarted";
	public const string StreakContinued = "attendance-streakcontinued";
	public const string CurrentStreak = "attendance-currentstreak";
	public const string PbStreak = "attendance-pbstreak";
	public const string Attendance = "attendance-attendance";
	public const string StreakOptOut = "attendance-optout";
	public const string OptOutError = "attendance-optouterror";
	public const string StreakOptIn = "attendance-optin";
	public const string OptInError = "attendance-optinerror";
	public const string TopStreaks = "attendance-topstreaks";
}