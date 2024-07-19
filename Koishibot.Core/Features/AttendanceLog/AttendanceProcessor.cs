using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.AttendanceLog.Interfaces;
using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.AttendanceLog;

public record AttendanceProcessor(
	KoishibotDbContext Database,
	IChatMessageService BotIrc, IAppCache Cache
	) : IAttendanceProcessor
{
	public async Task Start(TwitchUser user)
	{
		if (user.IsOnBlocklist()) { return; }

		if (Cache.AttendanceDisabled()) { return; }

		var attendance = await Database.GetAttendanceByUserId(user.Id);
		if (attendance is null)
		{
			attendance = new Attendance().Set(user);

			await Database.UpdateAttendance(attendance);
			await BotIrc.PostAttendanceLogged(Code.StreakStarted, attendance);
		}
		else // Attendance record found
		{
			if (attendance.WasAlreadyRecordedToday()) { return; }
		
			attendance
				.UpdateStreakCount(Cache.GetLastMandatoryStreamDate())
				.SetLastUpdatedDate();

			await Database.UpdateAttendance(attendance);

			if (attendance.OptedOutFromStreaks())
			{
				await BotIrc.PostAttendanceCount
					(Code.Attendance, user.Name, attendance.AttendanceCount);
			}
			else if (attendance.NewStreakStarted())
			{
				await BotIrc.PostAttendanceLogged
					(Code.StreakStarted, attendance);
			}
			else
			{
				await BotIrc.PostAttendanceLogged
					(Code.StreakContinued, attendance);
			}
		}
	}
}