using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.AttendanceLog.Interfaces;
using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.StreamInformation.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.AttendanceLog;

public record AttendanceProcessor(
KoishibotDbContext Database,
IChatReplyService ChatReplyService,
IStreamSessionService StreamSessionService,
IAppCache Cache
) : IAttendanceProcessor
{
	public async Task Start(TwitchUser user)
	{
		if (user.IsIgnored()) return;
		if (Cache.AttendanceDisabled()) return;

		var attendance = await Database.GetAttendanceByUserId(user.Id);
		if (attendance is null)
		{
			attendance = new Attendance().Set(user);

			await Database.UpdateAttendance(attendance);

			var data = new { User = user.Name };
			await ChatReplyService.App(Command.StreakStarted, data);
		}
		else // Attendance record found
		{
			if (attendance.WasAlreadyRecordedToday()) return;

			DateOnly date;

			try
			{
				date = Cache.GetLastMandatoryStreamDate();
			}
			catch (Exception e)
			{
				await StreamSessionService.CreateOrReloadStreamSession();
				date = Cache.GetLastMandatoryStreamDate();
			}

			attendance
			.UpdateStreakCount(date)
			.SetLastUpdatedDate();

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