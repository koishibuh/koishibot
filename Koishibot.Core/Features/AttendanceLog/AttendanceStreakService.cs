//using Koishibot.Core.Features.AttendanceLog.Enums;
//using Koishibot.Core.Features.AttendanceLog.Extensions;
//using Koishibot.Core.Features.AttendanceLog.Interfaces;
//using Koishibot.Core.Features.TwitchUsers.Models;
//using Koishibot.Core.Persistence;
//namespace Koishibot.Core.Features.AttendanceLog;

//public record AttendanceStreakService(
//		KoishibotDbContext Database,
//		IChatMessageService BotIrc
//	)	: IAttendanceStreakService
//{
//	public async Task GetUsersAttendanceStreak(Code code, TwitchUser user)
//	{
//		var result = await Database.GetAttendanceStreakByUserId(user.Id);
//		await BotIrc.PostStreakCount(code, user.Name, result);
//	}

//	public async Task GetTopAttendanceStreaks(Code code)
//	{
//		var topStreaks = await Database.GetTopAttendanceStreaks(10);
//		var result = topStreaks.CreateTopStreaksList(code);
//		await BotIrc.PostTopStreaks(result);
//	}

//	public async Task GetUserAttendanceCount(TwitchUser user)
//	{
//		var result = await Database.GetAttendanceByUserId(user.Id);
//		if (result is null) { return; }

//		var code = result.StreakOrAttendanceMessage();
//		await BotIrc.PostAttendanceCount(code, user.Name, result.AttendanceCount);
//	}

//	public async Task GetUsersBestStreak(Code code, TwitchUser user)
//	{
//		var result = await Database.GetUsersPersonalBestStreak(user.Id);
//		await BotIrc.PostStreakCount(code, user.Name, result);
//	}

//	public async Task ResetAttendanceStreaks()
//	{
//		await Database.ResetAttendanceStreaks();
//	}
//}