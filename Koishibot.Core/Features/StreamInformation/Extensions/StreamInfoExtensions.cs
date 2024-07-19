using Koishibot.Core.Features.Common;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence;
using Koishibot.Core.Persistence.Cache.Enums;
using System.Text.RegularExpressions;

namespace Koishibot.Core.Features.StreamInformation.Extensions;
public static class StreamInfoExtensions
{
	// MODEL

	public static TwitchStream ConvertStringToTimeSpan(this TwitchStream stream, string duration)
	{
		var formattedDuration = Regex.Replace(duration, @"[hms]", m => m.Value switch
		{
			"h" => ":",
			"m" => ":",
			"s" => string.Empty,
			_ => throw new InvalidOperationException("Invalid duration format")
		});

		stream.Duration = TimeSpan.Parse(formattedDuration);
		return stream;
	}

	// CACHE

	public static bool StreamOnline(this IAppCache cache)
	{
		return cache.GetStatusByServiceName(ServiceName.StreamOnline);
	}

	public static void UpdateStreamInfo(this IAppCache cache, StreamInfo info)
	{
		cache.AddNoExpire(CacheName.StreamInfo, info);
	}


	public static async Task UpdateStreamStatusOnline
		(this IAppCache cache)
	{
		await cache.UpdateServiceStatus(ServiceName.StreamOnline, true);
	}
	public static void AddStreamSessions
		(this IAppCache cache, StreamSessions sessions)
	{
		cache.Add(CacheName.StreamSessions, sessions);
	}


	// DATABASE

	public static async Task<TwitchStream?> GetSessionByTwitchId
		(this KoishibotDbContext database, string twitchStreamId)
	{
		return await database.TwitchStreams
			.FirstOrDefaultAsync(s => s.StreamId == twitchStreamId);
	}

	public static async Task<YearlyQuarter> AddYearlyQuarter
		(this KoishibotDbContext database, YearlyQuarter yearlyQuarter)
	{
		database.Update(yearlyQuarter);
		await database.SaveChangesAsync();
		return yearlyQuarter;
	}

	public static async Task AddStream
		(this KoishibotDbContext database, TwitchStream stream)
	{
		database.Update(stream);
		await database.SaveChangesAsync();
	}
}
