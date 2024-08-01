using Koishibot.Core.Features.AttendanceLog.Interfaces;
using Koishibot.Core.Features.ChatCommands.Interface;
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
			case "optout":
				await UpdateAttendanceOptService.Start(true, c.User);
				return true;

			case "optin":
				await UpdateAttendanceOptService.Start(false, c.User);
				return true;

			case "attendance":
				await AttendanceStreakService.GetUserAttendanceCount(c.User);
				return true;

			case "streak":
			case "streaks":
				await AttendanceStreakService.GetUsersAttendanceStreak
							("streak", "🔥", c.User);
				return true;

			case "steak":
			case "steaks":
				await AttendanceStreakService.GetUsersAttendanceStreak
							("steak", "🥩", c.User);
				return true;

			case "topstreaks":
			case "topstreak":
				await AttendanceStreakService.GetTopAttendanceStreaks
							("Streaks", "🔥");
				return true;

			case "topsteaks":
			case "topsteak":
				await AttendanceStreakService.GetTopAttendanceStreaks
							("Steaks", "🥩");
				return true;

			case "pbstreak":
				await AttendanceStreakService.GetUsersAttendanceStreak
							("streak", "🔥", c.User);
				return true;

			case "pbsteak":
				await AttendanceStreakService.GetUsersAttendanceStreak
							("steak", "🥩", c.User);
				return true;

			default: return false;
		}
	}
}