using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.AttendanceLog.Interfaces;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Services.Twitch.Irc.Interfaces;
namespace Koishibot.Core.Features.AttendanceLog;

public record UpdateAttendanceOptService(
	KoishibotDbContext Database,
	ITwitchIrcService BotIrc
	) : IUpdateAttendanceOptService
{
	public async Task Start(bool status, TwitchUser user)
	{
		var attendance = await Database.GetAttendanceByUserId(user.Id);
		if (attendance is null) { return; }

		if (attendance.OptStatusChanged(status))
		{
			attendance.UpdateOptStatus(status);
			await Database.UpdateAttendance(attendance);

			var code = status is true ? Code.StreakOptOut : Code.StreakOptIn;
			await BotIrc.OptStatus(code, user.Name);
		}
		else
		{
			var code = status is true ? Code.OptOutError : Code.OptInError;
			await BotIrc.OptStatus(code, user.Name);
		}
	}
}