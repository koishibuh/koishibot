using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.AdBreak.Extensions;
public static class TimerExtensions
{
	// CACHE
	public static IAppCache AddCurrentTimer
			(this IAppCache cache, CurrentTimer timer)
	{
		cache.Add(CacheName.CurrentTimer, timer, TimeSpan.FromHours(1));
		return cache;
	}

	public static CurrentTimer GetCurrentTimer(this IAppCache cache)
	{
		var result = cache.Get<CurrentTimer>(CacheName.CurrentTimer);
		return result is not null
				? result
				: throw new Exception("Current Timer not found");
	}

	// SIGNALR

	public static async Task UpdateTimerOverlay
			(this ISignalrService signalr, OverlayTimerVm vm)
	{
		await signalr.SendOverlayTimer(vm);
	}

}
