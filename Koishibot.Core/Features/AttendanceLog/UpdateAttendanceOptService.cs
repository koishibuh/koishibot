using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChatCommands;
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
		if (attendance is null) return;

		var data = new { User = user.Name };

		if (attendance.OptStatusChanged(status))
		{
			attendance.UpdateOptStatus(status);
			await Database.UpdateAttendance(attendance);

			var code = status ? Response.StreakOptOut : Response.StreakOptIn;
			await ChatReplyService.CreateResponse(code, data);
		}
		else
		{
			var code = status ? Response.OptOutError : Response.OptInError;
			await ChatReplyService.CreateResponse(code, data);
		}
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IUpdateAttendanceOptService
{
	Task Start(bool status, TwitchUser user);
}