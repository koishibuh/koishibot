using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.AttendanceLog.Interfaces;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.AttendanceLog;

public record UpdateAttendanceOptService(
	KoishibotDbContext Database,
	IChatReplyService ChatReplyService
	) : IUpdateAttendanceOptService
{
	public async Task Start(bool status, TwitchUser user)
	{
		var attendance = await Database.GetAttendanceByUserId(user.Id);
		if (attendance is null) { return; }

		var data = new UsernameData(user.Name);

		if (attendance.OptStatusChanged(status))
		{
			attendance.UpdateOptStatus(status);
			await Database.UpdateAttendance(attendance);

			var code = status is true ? Command.StreakOptOut : Command.StreakOptIn;
			await ChatReplyService.Start(code, data, PermissionLevel.Everyone);
		}
		else
		{
			var code = status is true ? Command.OptOutError : Command.OptInError;
			await ChatReplyService.Start(code, data, PermissionLevel.Everyone);
		}
	}
}