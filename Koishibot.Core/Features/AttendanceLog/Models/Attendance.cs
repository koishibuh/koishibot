using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.TwitchUsers.Models;

namespace Koishibot.Core.Features.AttendanceLog.Models;

public class Attendance
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public DateOnly LastUpdated { get; set; }
	public int AttendanceCount { get; set; }
	public int StreakCurrentCount { get; set; }
	public int StreakPersonalBest { get; set; }
	public bool StreakOptOut { get; set; }

	// == ⚫ NAVIGATION == //

	public TwitchUser User { get; set; } = null!;

	// == ⚫ METHODS == //

	public Attendance Set(TwitchUser user)
	{
		User = user;
		LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
		StreakCurrentCount = 1;
		StreakPersonalBest = 1;
		AttendanceCount = 1;
		StreakOptOut = false;

		return this;
	}

	public Attendance UpdateStreakCount(DateOnly lastMandatoryStreamDate)
	{
		if (lastMandatoryStreamDate == LastUpdated
						&& StreakOptOut == false)
		{
			StreakCurrentCount++;
			AttendanceCount++;

			if (StreakPersonalBest < StreakCurrentCount)
			{
				StreakPersonalBest = StreakCurrentCount;
			}
		}
		else
		{
			StreakCurrentCount = 1;
			AttendanceCount++;
		}

		return this;
	}

	public Attendance SetLastUpdatedDate()
	{
		var today = DateOnly.FromDateTime(DateTime.UtcNow);
		LastUpdated = today;
		return this;
	}

	public bool NewStreakStarted()
	{
		return StreakCurrentCount == 1;
	}

	public bool OptedOutFromStreaks()
	{
		return StreakOptOut is true;
	}

	public bool OptStatusChanged(bool updatedOpt)
	{
		return updatedOpt != StreakOptOut;
	}

	public Attendance UpdateOptStatus(bool updatedOpt)
	{
		StreakOptOut = updatedOpt;
		StreakCurrentCount = 1;
		return this;
	}

	public bool WasAlreadyRecordedToday()
	{
		return LastUpdated == DateOnly.FromDateTime(DateTime.Now);
	}

	public Code StreakOrAttendanceMessage()
	{
		return AttendanceCount > 1 
			? Code.AttendanceCount 
			: Code.Attendance;
	}
}