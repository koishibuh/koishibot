using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Extensions;
using Koishibot.Core.Features.ChatCommands;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
namespace Koishibot.Core.Features.AttendanceLog;

public record AttendanceStreakService(
KoishibotDbContext Database,
IChatReplyService ChatReplyService,
ISignalrService Signalr
) : IAttendanceStreakService
{
	public async Task GetUsersAttendanceStreak(string word, string emoji, TwitchUser user)
	{
		var result = await Database.GetAttendanceStreakByUserId(user.Id);

		var data = new { User = user.Name, Number = result, Word = word, Emoji = emoji };
		await ChatReplyService.CreateResponse(Response.CurrentStreak, data);
	}

	public async Task GetTopAttendanceStreaks(string word, string emoji)
	{
		var topStreaks = await Database.GetTopAttendanceStreaks(10);
		var result = topStreaks.CreateTopStreaksList();

		var data = new { Emoji = emoji, Word = word, List = result };
		await ChatReplyService.CreateResponse(Response.TopStreaks, data);
	}

	public async Task GetUserAttendanceCount(TwitchUser user)
	{
		var result = await Database.GetAttendanceByUserId(user.Id);
		if (result is null) return;

		var data = new { User = user.Name, Number = result.AttendanceCount };
		await ChatReplyService.CreateResponse(Response.Attendance, data);
	}

	public async Task GetUsersBestStreak(string word, string emoji, TwitchUser user)
	{
		var result = await Database.GetUsersPersonalBestStreak(user.Id);
		var data = new { User = user.Name, Number = result, Word = word, Emoji = emoji };
		await ChatReplyService.CreateResponse(Response.PbStreak, data);
	}

	public async Task ResetAttendanceStreaks()
	{
		await Database.ResetAttendanceStreaks();
		await Signalr.SendInfo("Attendance reset for the new quarter");
	}
}

/*══════════════════【 INTERFACE 】══════════════════*/
public interface IAttendanceStreakService
{
	Task GetUsersAttendanceStreak(string word, string emoji, TwitchUser user);
	Task GetTopAttendanceStreaks(string word, string emoji);
	Task GetUserAttendanceCount(TwitchUser user);
	Task GetUsersBestStreak(string word, string emoji, TwitchUser user);
	Task ResetAttendanceStreaks();
}