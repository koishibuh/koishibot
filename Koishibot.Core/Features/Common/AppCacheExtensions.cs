using Koishibot.Core.Features.Application.Models;
using Koishibot.Core.Features.Common.Models;
using Koishibot.Core.Features.StreamInformation.Models;
using Koishibot.Core.Persistence.Cache.Enums;

namespace Koishibot.Core.Features.Common;

public static class AppCacheExtensions
{
	public static void AddStreamEvent(this IAppCache cache, StreamEventVm streamEventVm)
	{
		var streamEvents = cache.Get<List<StreamEventVm>>(CacheName.StreamEvents);
		streamEvents ??= new List<StreamEventVm>();

		streamEvents.Add(streamEventVm);
		cache.Add(CacheName.StreamEvents, streamEvents);
	}

	public static bool GetStatusByServiceName(this IAppCache cache, ServiceName serviceName)
	{
		var serviceStatus = cache.Get<ServiceStatus>(CacheName.ServiceStatus)
			?? throw new Exception("Status not found");

		var result = typeof(ServiceStatus).GetProperty(serviceName.ToString())
			?? throw new Exception($"Property '{serviceName.ToString()}' not found in ServiceStatus");

		return result.GetValue(serviceStatus) == Status.Online;
	}

	public static async Task UpdateServiceStatusOnline(this IAppCache cache, ServiceName serviceName) =>
		await cache.UpdateServiceStatus(serviceName, Status.Online);

	public static async Task UpdateServiceStatusOffline(this IAppCache cache, ServiceName serviceName)
	{
		await cache.UpdateServiceStatus(serviceName, Status.Offline);
	}

	public static int? GetCurrentStreamId(this IAppCache cache)
	{
		var streamInfo = cache.Get<CurrentSession>(CacheName.CurrentSession);
		return streamInfo is null ? 0 : streamInfo.LiveStreamId;
	}

	public static int? GetCurrentTwitchStream(this IAppCache cache)
	{
		var session = cache.Get<CurrentSession>(CacheName.CurrentSession);
		return session.LiveStreamId;
	}

	public static int? GetCurrentStreamSessionId(this IAppCache cache)
	{
		var session = cache.Get<CurrentSession>(CacheName.CurrentSession);
		return session.StreamSessionId;
	}

	public static StreamInfo? GetStreamInfo(this IAppCache cache)
	{
		return cache.Get<StreamInfo>(CacheName.StreamInfo);
	}

	public static void CreateTimer(this IAppCache cache)
	{
		var timer = new CurrentTimer();
		cache.AddNoExpire(CacheName.CurrentTimer, timer);
	}

}