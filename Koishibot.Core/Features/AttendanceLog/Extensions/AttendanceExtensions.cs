using Koishibot.Core.Features.AttendanceLog.Models;
using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Features.TwitchUsers.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.AttendanceLog.Extensions;

public static class AttendanceExtensions
{
	// MODELS

	public static string CreateTopStreaksList(this List<AttendanceStreak> attendances)
	{
		return string.Join(" ", attendances.Select((viewer, index) =>
				$"{index + 1}. {viewer.Username} ({viewer.StreakCount})"));
	}

	// CACHE

	public static bool AttendanceDisabled(this IAppCache cache)
	{
		var result = cache.GetStatusByServiceName(ServiceName.Attendance);
		return result is false;
	}

	// public static DateOnly GetLastMandatoryStreamDate(this IAppCache cache)
	// {
	// 	var streamSessions = cache.Get<StreamSessions>(CacheName.StreamSessions)
	// 		?? throw new Exception("StreamSessions not found");
	//
	// 	return streamSessions.LastMandatoryStreamDate;
	// }

	public static IAppCache CreateAttendanceCache(this IAppCache cache)
	{
		cache.AddNoExpire(CacheName.Users, new List<TwitchUser>());
		return cache;
	}

	public static IAppCache CreateCache(this IAppCache cache)
	{
		cache.AddNoExpire(CacheName.StreamEvents, new List<TwitchUser>());
		cache.AddNoExpire(CacheName.CurrentTimer, new CurrentTimer());
		return cache;
	}

	public static IAppCache ClearAttendanceCache(this IAppCache cache)
	{
		cache.AddNoExpire(CacheName.Users, new List<TwitchUser>());
		return cache;
	}

	// DATABASE

	public static async Task<Attendance> UpdateAttendance
		(this KoishibotDbContext database, Attendance attendance)
	{
		database.Update(attendance);
		await database.SaveChangesAsync();
		return attendance;
	}

	public static async Task<Attendance?> GetAttendanceByUserId
		(this KoishibotDbContext database, int userId)
	{
		return await database.Attendances
			.Include(x => x.User)
			.FirstOrDefaultAsync(p => p.UserId == userId);
	}

	public static async Task<int> GetAttendanceStreakByUserId
		(this KoishibotDbContext database, int userId)
	{
		return await database.Attendances
				.AsNoTracking()
				.Where(x => x.UserId == userId)
				.Select(x => x.StreakCurrentCount)
				.FirstOrDefaultAsync();
	}

	public static async Task<int> GetUsersPersonalBestStreak
		(this KoishibotDbContext database, int userId)
	{
		return await database.Attendances
				.AsNoTracking()
				.Where(a => a.UserId == userId)
				.Select(a => a.StreakPersonalBest)
				.FirstOrDefaultAsync();
	}

	public static async Task<List<AttendanceStreak>> GetTopAttendanceStreaks
		(this KoishibotDbContext database, int topCount)
	{
		return await database.Attendances
			.AsNoTracking()
				.Include(a => a.User)
				.OrderByDescending(a => a.StreakCurrentCount)
				.Where(a => a.StreakCurrentCount >= 1
					&& a.User.Name != "elysiagriffin")
				.Take(topCount)
				.Select(a => new AttendanceStreak(a.User.Name, a.StreakCurrentCount))
				.ToListAsync();
	}

	public static async Task ResetAttendanceStreaks
		(this KoishibotDbContext database)
	{
		await database.Attendances
					.Where(x => x.StreakCurrentCount >= 2)
					.ExecuteUpdateAsync(p => p.SetProperty(x => x.StreakCurrentCount, 1));
	}
}