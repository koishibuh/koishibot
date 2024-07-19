using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.AttendanceLog.Models;
namespace Koishibot.Core.Features.AttendanceLog;

/// <summary>
/// List of responses to Attendance Commands and auto responses to<br/>
/// chat messages when Attendance is recorded for the day.
/// </summary>
public static class AttendanceChatReply
{
	// == ⚫  == //

	public static async Task PostAttendanceLogged(this IChatMessageService irc, Code code, Attendance a)
	{
		var message = code switch
		{
			Code.StreakStarted		=> $"{a.User.Name}, you've started a stream streak! Attend next stream to keep it up 🔥 (or !optout)",
			Code.StreakContinued	=> $"{a.User.Name}, you're on a {a.StreakCurrentCount} day stream streak! 🔥 Thanks for being here ❤",
			_											=> $"Something bork on PostAttendanceRecorded lol"
		};

		await irc.BotSend(message);
	}

	// == ⚫  == //

	public static async Task PostStreakCount(this IChatMessageService irc, Code code, string username, int count)
	{
		var message = code switch
		{
			Code.CurrentStreak=> $"{username}, your current stream streak count is {count} 🔥",
			Code.CurrentSteak	=> $"{username}, your current stream steak count is {count} 🥩",
			Code.PBStreak			=> $"{username}, your best streak count is currently {count} 🔥",
			Code.PBSteak			=> $"{username}, your best steak count is currently {count} 🥩",
			_									=> $"Something bork on PostStreakCount lol"
		};

		await irc.BotSend(message);
	}

	// == ⚫  == //

	public static async Task PostAttendanceCount(this IChatMessageService irc, Code code, string username, int count)
	{
		var message = code switch
		{
			Code.Attendance				=> $"{username}, you have {count} bananas! 🍌",
			Code.AttendanceCount	=> $"{username}, you have {count} bananas! 🍌 Whatchu making‽",
			_											=> $"Something bork on PostAttendanceCount lol"
		};

		await irc.BotSend(message);
	}

	// == ⚫  == //

	public static async Task OptStatus(this IChatMessageService irc, Code code, string username)
	{
		var message = code switch
		{
			Code.StreakOptOut => $"{username}, you are now opted out of stream streaks (!optin to undo)",
			Code.StreakOptIn	=> $"{username}, you are now opted in to stream streaks (!optout to undo)",
			Code.OptOutError	=> $"{username}, you were already opted out of streaks!",
			Code.OptInError		=> $"{username}, you were already opted into streaks!",
			_									=> $"Something bork on PostOptStatus lol"
		};

		await irc.BotSend(message);
	}

	// == ⚫  == //

	public static async Task PostTopStreaks(this IChatMessageService irc, string topList)
	{
		await irc.BotSend(topList);
	}
}