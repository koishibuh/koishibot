using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.AttendanceLog.Interfaces;

public interface IAttendanceStreakService
{
	Task GetUsersAttendanceStreak(Code code, TwitchUser user);
	Task GetTopAttendanceStreaks(Code code);
	Task GetUserAttendanceCount(TwitchUser user);
	Task ResetAttendanceStreaks();
}