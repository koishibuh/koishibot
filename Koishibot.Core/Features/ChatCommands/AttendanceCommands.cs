using Koishibot.Core.Features.AttendanceLog;
using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.ChatMessages.Models;
namespace Koishibot.Core.Features.ChatCommands;

public record AttendanceCommands(
	IAttendanceStreakService AttendanceStreakService,
	IUpdateAttendanceOptService UpdateAttendanceOptService
	) : IAttendanceCommands
{
	public async Task<bool> Process(ChatMessageDto c)
	{
		switch (c.Command)
		{
			case Command.StreakOptOut:
				await UpdateAttendanceOptService.Start(true, c.User);
				return true;

			case Command.StreakOptIn:
				await UpdateAttendanceOptService.Start(false, c.User);
				return true;

			case Command.Attendance:
				await AttendanceStreakService.GetUserAttendanceCount(c.User);
				return true;

			case Command.Streak:
			case Command.Streaks:
				await AttendanceStreakService.GetUsersAttendanceStreak
							("streak", "🔥", c.User);
				return true;

			case Command.Steak:
			case Command.Steaks:
				await AttendanceStreakService.GetUsersAttendanceStreak
							("steak", "🥩", c.User);
				return true;

			case Command.TopStreaks:
			case Command.TopStreak:
				await AttendanceStreakService.GetTopAttendanceStreaks
							("Streaks", "🔥");
				return true;

			case Command.TopSteaks:
			case Command.TopSteak:
				await AttendanceStreakService.GetTopAttendanceStreaks
							("Steaks", "🥩");
				return true;

			case Command.PbStreak:
				await AttendanceStreakService.GetUsersBestStreak
							("streak", "🔥", c.User);
				return true;

			case Command.PbSteak:
				await AttendanceStreakService.GetUsersBestStreak
							("steak", "🥩", c.User);
				return true;

			default: return false;
		}
	}
}

/*═══════════════════【 INTERFACE 】═══════════════════*/
public interface IAttendanceCommands
{
	Task<bool> Process(ChatMessageDto c);
}