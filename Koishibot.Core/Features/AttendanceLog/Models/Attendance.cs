using Koishibot.Core.Features.AttendanceLog.Enums;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence.Cache.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Koishibot.Core.Features.AttendanceLog.Models;

/*═════════════════【 ENTITY MODEL 】═════════════════*/
public class Attendance
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public int? LastAttendedSessionId { get; set; } // new
	// public DateOnly LastUpdated { get; set; }
	public int AttendanceCount { get; set; }
	public int StreakCurrentCount { get; set; }
	public int StreakPersonalBest { get; set; }
	public bool StreakOptOut { get; set; }

/*══════════════════【 NAVIGATION 】══════════════════*/
	public TwitchUser User { get; set; } = null!;

/*═══════════════════【 METHODS 】═══════════════════*/
	public Attendance Set(TwitchUser user, int? sessionId)
	{
		User = user;
		LastAttendedSessionId = sessionId;
		// LastUpdated = DateOnly.FromDateTime(DateTime.UtcNow);
		StreakCurrentCount = 1;
		StreakPersonalBest = 1;
		AttendanceCount = 1;
		StreakOptOut = false;

		return this;
	}

	// public Attendance UpdateStreakCount(DateOnly lastMandatoryStreamDate)
	public Attendance UpdateStreakCount(int? lastMandatorySessionId)
	{
		if (lastMandatorySessionId == LastAttendedSessionId && StreakOptOut == false)
		{
			StreakCurrentCount++;
			AttendanceCount++;

			if (StreakPersonalBest < StreakCurrentCount)
				StreakPersonalBest = StreakCurrentCount;
		}
		else
		{
			StreakCurrentCount = 1;
			AttendanceCount++;
		}

		return this;
	}

	public Attendance SetLastUpdatedDate(int? lastMandatorySessionId)
	{
		LastAttendedSessionId = lastMandatorySessionId;
		return this;
	}

	public bool NewStreakStarted()
		=> StreakCurrentCount == 1;

	public bool OptedOutFromStreaks()
		=> StreakOptOut is true;

	public bool OptStatusChanged(bool updatedOpt)
		=> updatedOpt != StreakOptOut;

	public Attendance UpdateOptStatus(bool updatedOpt)
	{
		StreakOptOut = updatedOpt;
		StreakCurrentCount = 1;
		return this;
	}

	public bool WasAlreadyRecordedToday(int? lastMandatorySessionId)
		=> LastAttendedSessionId == lastMandatorySessionId;
}

/*═══════════════════【 EXTENSIONS 】═══════════════════*/
public static class AttendanceExtensions
{
	public static IAppCache ClearAttendanceCache(this IAppCache cache)
	{
		cache.AddNoExpire(CacheName.Users, new List<TwitchUser>());
		return cache;
	}
}

/*══════════════════【 CONFIGURATION 】═════════════════*/
public class AttendanceConfig : IEntityTypeConfiguration<Attendance>
{
	public void Configure(EntityTypeBuilder<Attendance> builder)
	{
		builder.ToTable("Attendances");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Id);

		builder.Property(p => p.UserId);

		builder.Property(p => p.AttendanceCount);

		builder.Property(p => p.StreakCurrentCount);

		builder.Property(p => p.StreakPersonalBest);

		builder.Property(p => p.StreakOptOut);
	}
}