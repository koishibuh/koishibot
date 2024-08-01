using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.AttendanceLog.Interfaces;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.ChatCommands.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.AttendanceLog;

public record AttendanceStreakService(
	KoishibotDbContext Database,
	IChatReplyService ChatReplyService
	) : IAttendanceStreakService
{
	public async Task GetUsersAttendanceStreak(string word, string emoji, TwitchUser user)
	{
		var result = await Database.GetAttendanceStreakByUserId(user.Id);
		var data = new UserCountStreakData(user.Name, result, word, emoji);

		await ChatReplyService.Start(Command.CurrentStreak, data, PermissionLevel.Everyone);
	}

	public async Task GetTopAttendanceStreaks(string word, string emoji)
	{
		var topStreaks = await Database.GetTopAttendanceStreaks(10);
		var result = topStreaks.CreateTopStreaksList();

		var data = new TopAttendanceData(emoji, word, result);
		await ChatReplyService.Start(Command.Attendance, data, PermissionLevel.Everyone);
	}

	public async Task GetUserAttendanceCount(TwitchUser user)
	{
		var result = await Database.GetAttendanceByUserId(user.Id);
		if (result is null) { return; }

		var data = new UserCountData(user.Name, result.AttendanceCount);
		await ChatReplyService.Start(Command.Attendance, data, PermissionLevel.Everyone);
	}

	public async Task GetUsersBestStreak(string word, string emoji, TwitchUser user)
	{
		var result = await Database.GetUsersPersonalBestStreak(user.Id);
		var data = new UserCountStreakData(user.Name, result, word, emoji);

		await ChatReplyService.Start(Command.PBStreak, data, PermissionLevel.Everyone);
	}

	public async Task ResetAttendanceStreaks()
	{
		await Database.ResetAttendanceStreaks();
	}
}