using Koishibot.Core.Features.AttendanceLog.Enums;
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
							(Code.CurrentStreak, c.User);
				return true;

			case "steak":
			case "steaks":
				await AttendanceStreakService.GetUsersAttendanceStreak
							(Code.CurrentSteak, c.User);
				return true;

			case "topstreaks":
			case "topstreak":
				await AttendanceStreakService.GetTopAttendanceStreaks
							(Code.TopStreaks);
				return true;

			case "topsteaks":
			case "topsteak":
				await AttendanceStreakService.GetTopAttendanceStreaks
							(Code.TopSteaks);
				return true;

			case "pbstreak":
				await AttendanceStreakService.GetUsersAttendanceStreak
							(Code.PBStreak, c.User);
				return true;

			case "pbsteak":
				await AttendanceStreakService.GetUsersAttendanceStreak
							(Code.PBSteak, c.User);
				return true;

			default: return false;
		}
	}
}