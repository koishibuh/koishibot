using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.AttendanceLog.Interfaces;

public interface IAttendanceStreakService
{
	Task GetUsersAttendanceStreak(string word, string emoji, TwitchUser user);
	Task GetTopAttendanceStreaks(string word, string emoji);
	Task GetUserAttendanceCount(TwitchUser user);
	Task ResetAttendanceStreaks();
}