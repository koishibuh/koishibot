using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.AttendanceLog.Interfaces;
using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.AttendanceLog;

public record AttendanceProcessor(
KoishibotDbContext Database,
IChatReplyService ChatReplyService,
IAppCache Cache
) : IAttendanceProcessor
{
	public async Task Start(TwitchUser user)
	{
		if (user.IsIgnored()) return;

		var session = Cache.Get<CurrentSession>(CacheName.CurrentSession);

		var attendance = await Database.GetAttendanceByUserId(user.Id);
		if (attendance is null)
		{
			attendance = new Attendance().Set(user, session.LastMandatorySessionId);

			await Database.UpdateAttendance(attendance);

			var data = new { User = user.Name };
			await ChatReplyService.App(Command.StreakStarted, data);
		}
		else // Attendance record found
		{
			if (attendance.WasAlreadyRecordedToday(session.LastMandatorySessionId)) return;

			attendance
			.UpdateStreakCount(session.LastMandatorySessionId)
			.SetLastUpdatedDate(session.LastMandatorySessionId);

			await Database.UpdateAttendance(attendance);

			if (attendance.OptedOutFromStreaks())
			{
				var data = new { User = user.Name, Number = attendance.AttendanceCount };
				await ChatReplyService.Start(Command.Attendance, data, PermissionLevel.Everyone);
			}
			else if (attendance.NewStreakStarted())
			{
				var data = new { User = user.Name };
				await ChatReplyService.App(Command.StreakStarted, data);
			}
			else
			{
				var data = new { User = user.Name, Number = attendance.AttendanceCount };
				await ChatReplyService.App(Command.StreakContinued, data);
			}
		}
	}
}